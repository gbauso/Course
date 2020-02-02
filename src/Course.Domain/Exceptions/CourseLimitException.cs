namespace Domain.Exceptions
{
    public class CourseLimitException : DomainException
    {
        public CourseLimitException() : base("COURSE-ENROLLMENT-02")
        {

        }
    }
}
