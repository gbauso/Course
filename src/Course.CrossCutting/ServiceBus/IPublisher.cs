using CrossCutting.ServiceBus;
using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IPublisher : IDisposable
    {
        Task Publish(BusMessage message, string queue);
    }
}
