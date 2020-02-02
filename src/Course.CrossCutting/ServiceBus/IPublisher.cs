using CrossCutting.ServiceBus;
using System.Threading.Tasks;

namespace CrossCutting.ServiceBus
{
    public interface IPublisher
    {
        Task Publish(BusMessage message, string queue);
    }
}
