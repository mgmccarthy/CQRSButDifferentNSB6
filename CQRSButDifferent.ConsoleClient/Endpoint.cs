using System;
using System.Threading.Tasks;
using NServiceBus;

namespace CQRSButDifferent.ConsoleClient
{
    public static class Endpoint
    {
        public static IEndpointInstance Instance { get; private set; }

        public static async Task Init()
        {
            Console.Title = "CQRSButDifferent.ConsoleClientNSB6";

            var endpointConfiguration = new EndpointConfiguration("CQRSButDifferent.ConsoleClientNSB6");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendOnly();

            Instance = await NServiceBus.Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
    }
}
