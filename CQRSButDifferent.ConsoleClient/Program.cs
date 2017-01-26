using System;
using System.Threading;
using System.Threading.Tasks;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.ConsoleClient
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        private static async Task AsyncMain()
        {
            await Endpoint.Init();

            Console.WriteLine("Press 'Enter' to start placing orders. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                var random = new Random();
                for (var i = 1; i <= int.MaxValue; i++)
                {
                    var productQuantity = random.Next(1, 6);
                    await Endpoint.Instance.Send(new PlaceOrder { OrderId = i, ProductId = 1, Quantity = productQuantity });
                    Console.WriteLine("An order with {0} of product 1 has been placed.", productQuantity);
                    await Task.Delay(1000);
                }
            }
        }
    }
}
