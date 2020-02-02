using System.Threading;
using System.Threading.Tasks;
using CrossCutting.ServiceBus;
using MediatR;

namespace Application.Command
{
    public class EnrollmentRequestHandler : IRequestHandler<EnrollmentRequestCommand, bool>
    {
        private readonly IQueuePublisher _Publisher;

        public EnrollmentRequestHandler(IQueuePublisher publisher)
        {
            _Publisher = publisher;
        }

        public async Task<bool> Handle(EnrollmentRequestCommand request, CancellationToken cancellationToken)
        {
            var message = new BusMessage("CourseEnrollment", request);

            await _Publisher.Publish(message, "course.queue");

            return true;
        }
    }
}
