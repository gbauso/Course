using CrossCutting.ServiceBus;
using MediatR;
using System.Reflection;
using System.Linq;
using System;
using System.Text;
using CrossCutting.Exceptions;

namespace CrossCutting.Extensions
{
    public static class ServiceBusExtensions
    {
        public static IRequest<bool> ConvertBusMessageToCommand(this BusMessage message)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                                               .Where(i => i.FullName.Contains("Domain") || i.FullName.Contains("Application"))
                                               .SelectMany(x => x.GetTypes());

            var type = types.FirstOrDefault(x => x.Name.ToLower()
                                                       .Equals($"{message.MessageType.ToLower()}command"));

            try
            {
                var data = Newtonsoft.Json.JsonConvert.DeserializeObject(Encoding.UTF8.GetString(message.Data), type);
                var command = (IRequest<bool>) data;
                
                return command;
            }
            catch
            {
                throw new CommandException();
            }


        }
    }
}
