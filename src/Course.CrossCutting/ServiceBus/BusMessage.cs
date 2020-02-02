using Newtonsoft.Json;
using System;
using System.Text;

namespace CrossCutting.ServiceBus
{
    public class BusMessage
    {
        public BusMessage(string type, object data)
        {
            MessageId = Guid.NewGuid().ToString();
            MessageType = type;
            Data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        }

        [JsonConstructor]
        public BusMessage(string messageId, string messageType, byte[] data)
        {
            MessageId = messageId;
            MessageType = messageType;
            Data = data;
        }

        public string MessageId { get; private set; }
        public string MessageType { get; private set; }
        public byte[] Data { get; private set; }

    }
}
