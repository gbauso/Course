using MediatR;

namespace Domain.Service
{
    public class NotifyEnrollmentCommand : IRequest<bool>
    {
        public EmailNotification Notification { get; set; }
    }
}
