using CrossCutting.Exceptions;
using CrossCutting.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ServiceBus.Azure
{
    public class AzurePublisher : IQueuePublisher
    {
        private readonly string _ConnectionString;
        private QueueClient _Client;

        public AzurePublisher(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ServiceBus");
            _ConnectionString = connectionString;
        }

        public async Task Publish(BusMessage message, string queue)
        {
            _Client = new QueueClient(_ConnectionString, queue, ReceiveMode.PeekLock);

            var busMessage = new Message
            {
                Body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message, Formatting.Indented))
            };

            try
            {
                await _Client.SendAsync(busMessage);
            }
            catch (Exception e)
            {
                throw new BusPublishException(e);
            }
        }

        public void Dispose()
        {
            _Client?.CloseAsync().Wait();
        }
    }
}
