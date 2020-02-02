using CrossCutting.Exceptions;
using System;

namespace Domain.Exceptions
{
    public class DomainException : ValidationException
    {
        public DomainException(string errorCode) : base(errorCode)
        {

        }
    }
}
