using CrossCutting.ServiceBus;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface ISubscriber
    {
        Task Handle(BusMessage message);
    }
}
