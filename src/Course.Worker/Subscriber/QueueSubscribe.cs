using System;
using System.Text;
using System.Threading.Tasks;
using CrossCutting.Exceptions;
using CrossCutting.Extensions;
using CrossCutting.ServiceBus;
using Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Subscriber
{
    public class QueueSubscribe : IMessageSubscriber
    {
        private readonly IMediator _MediatR;
        private readonly ILogger<QueueSubscribe> _Logger;
        private readonly IServiceProvider _Provider;

        public QueueSubscribe(IMediator mediatR, ILogger<QueueSubscribe> logger, IServiceProvider provider)
        {
            _MediatR = mediatR;
            _Logger = logger;
            _Provider = provider;
        }

        public async Task Handle(BusMessage message)
        {
            using(var scope =  _Provider.CreateScope())
            {
                try
                {
                    var dataBody = Encoding.UTF8.GetString(message.Data);
                    _Logger.LogInformation(dataBody);

                    var command = message.ConvertBusMessageToCommand();

                    await _MediatR.Send(command);
                }
                catch(DomainException de)
                {
                    _Logger.LogError(de, de.Message);
                }
                catch(ValidationException ve)
                {
                    _Logger.LogError(ve, ve.Message);
                }
                catch (CommandException ce)
                {
                    _Logger.LogError(ce, ce.Message);
                }
                catch (Exception ex)
                {
                    _Logger.LogError(ex, ex.Message);
                }
            }
        }
    }
}
