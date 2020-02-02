using Application.Command;
using FluentValidation;
using System;

namespace Application.Validator
{
    public class EnrollmentRequestCommandValidator : AbstractValidator<EnrollmentRequestCommand>
    {
        public EnrollmentRequestCommandValidator()
        {
            RuleFor(i => i.CourseId).NotEqual(Guid.Empty).NotNull().WithErrorCode("ENROLL_REQUEST_ID-01");
            RuleFor(i => i.StudentName).NotNull().NotEqual(string.Empty).WithErrorCode("ENROLL_REQUEST_STUDENT_NAME-01");
            RuleFor(i => i.StudentAge).GreaterThan(0).WithErrorCode("ENROLL_REQUEST_STUDENT_AGE-01");
        }
    }
}
