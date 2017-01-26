using System;
using System.Threading.Tasks;
using CQRSButDifferent.Messages.Commands;
using NServiceBus;

namespace CQRSButDifferent.SnapshotEndpoint
{
    public class ScheduleSnapshot : IWantToRunWhenEndpointStartsAndStops
    {
        public async Task Start(IMessageSession session)
        {
            await session.ScheduleEvery(TimeSpan.FromMinutes(1), pipelineContext => pipelineContext.SendLocal(new CreateSnapshotOfProductQuantity())).ConfigureAwait(false);
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}