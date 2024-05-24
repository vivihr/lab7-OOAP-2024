using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab7real
{
    //клас для представлення замовлення
    class Order
    {
        public string Customer { get; set; }
        public string Size { get; set; }
        public string ShippingMethod { get; set; }
        public string Destination { get; set; }

        public Order(string customer, string size, string shippingMethod, string destination)
        {
            Customer = customer;
            Size = size;
            ShippingMethod = shippingMethod;
            Destination = destination;
        }
    }

    //інтерфейс обробника
    abstract class OrderHandler
    {
        protected OrderHandler nextHandler;

        public void SetNextHandler(OrderHandler nextHandler)
        {
            this.nextHandler = nextHandler;
        }

        public void Handle(Order order)
        {
            Console.WriteLine($"Passing through {this.GetType().Name}");

            if (CanHandle(order))
            {
                Process(order);
            }
            else if (nextHandler != null)
            {
                nextHandler.Handle(order);
            }
            else
            {
                Console.WriteLine("No handler could process the order.");
            }
        }

        protected abstract bool CanHandle(Order order);
        protected abstract void Process(Order order);
    }

    //конкретні обробники
    class CustomerOrderHandler : OrderHandler
    {
        protected override bool CanHandle(Order order)
        {
            return order.Customer == "VIP"; //віп замовник
        }

        protected override void Process(Order order)
        {
            Console.WriteLine($"Handling VIP customer order for {order.Customer}");
        }
    }

    class SizeOrderHandler : OrderHandler
    {
        protected override bool CanHandle(Order order)
        {
            return order.Size == "Large"; //розмір замовлення
        }

        protected override void Process(Order order)
        {
            Console.WriteLine($"Handling large size order for {order.Size}");
        }
    }

    class ShippingMethodOrderHandler : OrderHandler
    {
        protected override bool CanHandle(Order order)
        {
            return order.ShippingMethod == "Express"; //швидкість доставки
        }

        protected override void Process(Order order)
        {
            Console.WriteLine($"Handling express shipping order for {order.ShippingMethod}");
        }
    }

    class DestinationOrderHandler : OrderHandler
    {
        protected override bool CanHandle(Order order)
        {
            return order.Destination == "International"; //міжнародна доставка
        }

        protected override void Process(Order order)
        {
            Console.WriteLine($"Handling international order for {order.Destination}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //введення даних користувачем
            Console.Write("Enter customer type (VIP, Regular): ");
            string customer = Console.ReadLine();

            Console.Write("Enter order size (Small, Medium, Large): ");
            string size = Console.ReadLine();

            Console.Write("Enter shipping method (Standard, Express): ");
            string shippingMethod = Console.ReadLine();

            Console.Write("Enter destination (Local, International): ");
            string destination = Console.ReadLine();

            //створення замовлення
            Order order = new Order(customer, size, shippingMethod, destination);

            //створення обробників і встановлення ланцюжка
            OrderHandler customerHandler = new CustomerOrderHandler();
            OrderHandler sizeHandler = new SizeOrderHandler();
            OrderHandler shippingHandler = new ShippingMethodOrderHandler();
            OrderHandler destinationHandler = new DestinationOrderHandler();

            customerHandler.SetNextHandler(sizeHandler);
            sizeHandler.SetNextHandler(shippingHandler);
            shippingHandler.SetNextHandler(destinationHandler);

            //передача замовлення через ланцюжок обробників
            customerHandler.Handle(order);

            Console.ReadLine();
        }
    }
}
