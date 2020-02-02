using System;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IBusSubscriber : IDisposable
    {
        Task Subscribe(IMessageSubscriber subscriber, string queue);
    }
}
