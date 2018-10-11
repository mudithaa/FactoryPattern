using Autofac;
using System;

namespace FactoryPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ContainerBuilder();

            //builder.RegisterGeneric(typeof(Factory<IMyBaseInterface>)).AsImplementedInterfaces();builder.RegisterType<Car>().AsImplementedInterfaces();
            builder.RegisterType<Car>().AsImplementedInterfaces();
            builder.RegisterType<Bus>().AsImplementedInterfaces();
            builder.RegisterType<Consumer>().AsSelf();
            builder.RegisterGeneric(typeof(NewFactory<>)).AsImplementedInterfaces();
            
            var container = builder.Build();

            var consumer = container.Resolve<Consumer>();

            consumer.Print();
            Console.ReadLine();
        }
    }

    interface INewFactory<T> where T : IMyBaseInterface
    {
        T CreateInstance(string input);
    }

    class NewFactory<T> : INewFactory<T> where T : IMyBaseInterface
    {
        private readonly T Instance;
        public NewFactory(ILifetimeScope scope)
        {
            Instance = scope.Resolve<T>();
        }

        public T CreateInstance(string input)
        {
            return Instance;
        }
    }

    interface IFactory<T> where T : IMyBaseInterface
    {
        T CreateInstance(string name);
    }

    public class Factory<T> : IFactory<T> where T : IMyBaseInterface, new () 
    {
        public T CreateInstance(string name)
        {
            var TObject = new T();

            //TObject.FactoryMethod = Format;
            return TObject;
        }

        string Format(int value)
        {
            return string.Format($"Your input was {value}", value);
        }
    }

    public interface IMyBaseInterface
    {
    }

    public interface ICar : IMyBaseInterface
    {
        void MakeCar();
    }

    public class Car : ICar
    {
        public Car(IBus bus)
        {

        }

        public void MakeCar()
        {
            Console.WriteLine("Car made");
        }
    }

    public interface IBus : IMyBaseInterface
    {
        void MakeBus();
    }

    public class Bus : IBus
    {
        public void MakeBus()
        {
            Console.WriteLine("Bus made");
        }
    }

    class Consumer
    {
        public INewFactory<ICar> Factory { get; set; }
        public IFactory<IBus> OtherFactory { get; set; }
        public Consumer(INewFactory<ICar> factory)
        {
            //, IFactory<IBus> otherFactory
            Factory = factory;
            //OtherFactory = otherFactory;
        }

        public void Print()
        {
            var concreteClass = Factory.CreateInstance("Muditha");
            concreteClass.MakeCar();

            //var otherClass = OtherFactory.CreateInstance("Muditha");
            //otherClass.MakeBus();
        }
    }
}
