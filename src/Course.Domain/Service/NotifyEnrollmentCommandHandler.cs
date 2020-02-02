using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class NotifyEnrollmentCommandHandler : IRequestHandler<NotifyEnrollmentCommand, bool>
    {
        private readonly INotifier _Notifier;

        public NotifyEnrollmentCommandHandler(INotifier notifier)
        {
            _Notifier = notifier;
        }

        public async Task<bool> Handle(NotifyEnrollmentCommand request, CancellationToken cancellationToken)
        {
            await _Notifier.Notify(request.Notification);

            return true;
        }
    }
}
