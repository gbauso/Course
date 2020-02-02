using CrossCutting.ServiceBus;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IMessageSubscriber
    {
        Task Handle(BusMessage message);
    }
}
