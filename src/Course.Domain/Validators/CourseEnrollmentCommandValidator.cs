using Domain.Service;
using FluentValidation;
using System;

namespace Domain.Validators
{
    public class CourseEnrollmentCommandValidator : AbstractValidator<CourseEnrollmentCommand>
    {
        public CourseEnrollmentCommandValidator()
        {
            RuleFor(i => i.CourseId).NotNull().NotEqual(Guid.Empty).WithErrorCode("COMMAND-COURSE-01");
            RuleFor(i => i.StudentName).NotNull().NotEqual(string.Empty).WithErrorCode("COMMAND-STUDENT_NAME-01");
            RuleFor(i => i.StudentAge).GreaterThan(0).WithErrorCode("COMMAND-STUDENT_AGE-01");
        }
    }
}
