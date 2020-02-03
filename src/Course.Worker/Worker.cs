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
        private readonly IQueueSubscriber _ServiceBus;
        private readonly IMessageSubscriber _Subscriber;
        private readonly CourseDbContext _Context;

        public Worker(IQueueSubscriber serviceBus, IMessageSubscriber subscriber, CourseDbContext context)
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
            await _Context.Database.EnsureCreatedAsync();
            await _Context.Database.MigrateAsync();
        }
    }
}
