using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CQRSButDifferent.Data;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SnapshotEndpoint
{
    public class CreateSnapshotOfProductQuantityHandler : IHandleMessages<CreateSnapshotOfProductQuantity>
    {
        public async Task Handle(CreateSnapshotOfProductQuantity message, IMessageHandlerContext context)
        {
            using (var dbContext = new CqrsButDifferentContext())
            {
                //stamp out "one minute ago"
                var oneMinuteAgo = DateTime.Now.AddMinutes(-1);

                //get the prouduct quantity as of one minute ago
                var productQuantity = 0;
                var results = dbContext.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo);
                if (results.Any())
                {
                    productQuantity = await results.SumAsync(x => x.Delta);
                }

                //delete all records less than or equal to one minute ago
                dbContext.ProductQuantity.RemoveRange(dbContext.ProductQuantity.Where(x => x.ProductId == 1 && x.TimeStamp <= oneMinuteAgo).ToList());

                //insert the new delta based on the product quantity at one minute ago
                dbContext.ProductQuantity.Add(new ProductQuantity { ProductId = 1, Delta = productQuantity, TimeStamp = DateTime.Now });

                await dbContext.SaveChangesAsync();

                Console.WriteLine("Snapshot Created.");
            }
        }
    }
}
