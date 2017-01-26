using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using CQRSButDifferent.Messages.Events;
using NServiceBus;

namespace CQRSButDifferent.OrderEndpoint
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            using (var dbContext = new CqrsButDifferentContext())
            {
                //sum(delta) here to determine if there is enough quantity of the product to place the order
                var productQuantity = 0;
                var results = dbContext.ProductQuantity.Where(x => x.ProductId == message.ProductId);
                if (results.Any())
                {
                    productQuantity = await results.SumAsync(x => x.Delta);
                }

                if ((productQuantity < 20) && (productQuantity > 0))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("We have less than 20 of product 1 remaining. Resupply the vendor soon.");
                }
                
                if (productQuantity < 0)
                {
                    await context.Publish(new InsuffcientProductQuantityForOrder { OrderId = message.OrderId, ProductId = message.ProductId, Quantity = message.Quantity });
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("We are out of product 1. Resupply the vendor now.");
                }
                else
                {
                    //every time someone makes a purchase, we insert a new record with the negative of the quantity of the product they purchased
                    var negativeQuantityForOrder = Math.Abs(message.Quantity) * -1;
                    dbContext.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = negativeQuantityForOrder, TimeStamp = DateTime.Now });
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Order placed.");
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
