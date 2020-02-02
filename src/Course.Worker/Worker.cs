using System.Threading;
using System.Threading.Tasks;
using CrossCutting.ServiceBus;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Worker
{
    public class Worker : IHostedService
    {
        private readonly IQueueSubscribe _ServiceBus;
        private readonly IMessageSubscriber _Subscriber;
        private readonly CourseDbContext _Context;

        public Worker(IQueueSubscribe serviceBus, IMessageSubscriber subscriber, CourseDbContext context)
        {
            _ServiceBus = serviceBus;
            _Subscriber = subscriber;
            _Context = context;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DatabaseInitializer();
            await _ServiceBus.Subscribe(_Subscriber, "course.queue");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _ServiceBus.Dispose();

            return Task.CompletedTask;
        }

        private async Task DatabaseInitializer()
        {
            _Context.Database.Migrate();
            await _Context.Database.EnsureCreatedAsync();
        }
    }
}
