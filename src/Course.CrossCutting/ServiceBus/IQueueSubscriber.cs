using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IQueueSubscriber : IDisposable
    {
        Task Subscribe(IMessageSubscriber subscriber, string queue);
    }
}
