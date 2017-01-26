using CQRSButDifferent.Data;
using NServiceBus.Features;

namespace CQRSButDifferent.SupplierEndpoint
{
    public class EnsureTablesAreCreatedWhenConfiguringEndpoint : Feature
    {
        public EnsureTablesAreCreatedWhenConfiguringEndpoint()
        {
            using (var context = new CqrsButDifferentContext())
            {
                context.Database.Initialize(false);
            }
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
        }
    }
}
