using System;
using System.Threading.Tasks;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SupplierEndpoint
{
    public class ResupplyVendorHandler : IHandleMessages<ResupplyVendor>
    {
        public async Task Handle(ResupplyVendor message, IMessageHandlerContext context)
        {
            using (var dbContext = new CqrsButDifferentContext())
            {
                dbContext.ProductQuantity.Add(new ProductQuantity { ProductId = message.ProductId, Delta = message.Quantity, TimeStamp = DateTime.Now });
                await dbContext.SaveChangesAsync();

                Console.WriteLine("Vendor Resupplied.");
            }
        }
    }
}
