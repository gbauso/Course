using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface INotifier
    {
        Task Notify(Notification notification);
    }
}
