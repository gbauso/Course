using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IQueueSubscribe : IDisposable
    {
        Task Subscribe(IMessageSubscriber subscriber, string queue);
    }
}
