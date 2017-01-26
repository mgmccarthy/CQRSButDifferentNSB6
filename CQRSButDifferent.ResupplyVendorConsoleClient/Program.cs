using System;
using System.Threading.Tasks;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.ResupplyVendorConsoleClient
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

            Console.WriteLine("Press 'Enter' to resupply the vendor with 100 of product 1. To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                Console.WriteLine("Resupply vendor.");
                await Endpoint.Instance.Send(new ResupplyVendor { ProductId = 1, Quantity = 100 });
            }
        }
    }
}
