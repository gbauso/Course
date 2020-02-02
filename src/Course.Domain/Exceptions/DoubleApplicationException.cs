namespace Domain.Exceptions
{
    public class DoubleApplicationException : DomainException
    {
        public DoubleApplicationException() : base("COURSE-ENROLLMENT-01")
        {
        }
    }
}
