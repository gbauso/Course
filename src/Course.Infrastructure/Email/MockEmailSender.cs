using Domain;
using Domain.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class MockEmailSender : INotifier
    {
        public Task Notify(Notification notification)
        {
            return Task.CompletedTask;
        }
    }
}
