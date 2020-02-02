using System;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Exceptions;
using CrossCutting.ServiceBus;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.ServiceBus.Azure
{
    public class AzureServiceBus : IServiceBus
    {
        private readonly string _ConnectionString;
        private QueueClient _Client;

        public AzureServiceBus(IConfiguration configuration)
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
            catch(Exception e)
            {
                throw new BusPublishException(e);
            }
        }

        public Task Subscribe(ISubscriber handler, string queue)
        {
            _Client = new QueueClient(_ConnectionString, queue, ReceiveMode.PeekLock);

            var messageHandlerOptions = new MessageHandlerOptions((e) => { return Task.Delay(100); })
            {
                AutoComplete = false,
                MaxConcurrentCalls = 1
            };

            _Client.RegisterMessageHandler(async (busMessage, concurrentToken) =>
            {
                var token = busMessage.SystemProperties.LockToken;
                try
                {
                    var body = busMessage.Body;
                    if (handler != null)
                    {
                        var message = JsonConvert.DeserializeObject<BusMessage>(Encoding.UTF8.GetString(body));

                        await handler.Handle(message);
                    }

                    await _Client.CompleteAsync(token);
                }
                catch(CommandException)
                {
                    await _Client.DeadLetterAsync(token);
                }
                catch
                {
                    await _Client.AbandonAsync(token);
                }
            }, messageHandlerOptions);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _Client?.CloseAsync().Wait();
        }
    }
}
