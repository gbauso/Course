using System;
using System.Collections.Generic;
using System.Text;

namespace CrossCutting.Exceptions
{
    public class BusPublishException : Exception
    {
        public BusPublishException(Exception innerException) : base(string.Empty, innerException)
        {

        }
    }
}
