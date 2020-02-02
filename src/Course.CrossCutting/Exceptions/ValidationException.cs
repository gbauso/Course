using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace CrossCutting.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException() : base()
        {
        }

        public ValidationException(string message) : base(SerializeMessage(message))
        {
        }

        public ValidationException(IEnumerable<string> message)  : base(SerializeMessage(message))
        {
        }

        private static string SerializeMessage(IEnumerable<string> message)
        {
            return JsonConvert.SerializeObject(message);
        }

        private static string SerializeMessage(string message)
        {
            var arrayMessage = new[] { message };
            return SerializeMessage(arrayMessage);
        }
    }
}
