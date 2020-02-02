using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IServiceBus : IPublisher, IDisposable
    {
        Task Subscribe(ISubscriber subscriber, string queue);
    }
}
