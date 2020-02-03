using CrossCutting.ServiceBus;
using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IQueuePublisher : IDisposable
    {
        Task Publish(BusMessage message, string queue);
    }
}
