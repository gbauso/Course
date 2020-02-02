using Domain.Model;
using FluentValidation;
using System;

namespace Domain.Validators
{
    public class EnrollmentValidator : AbstractValidator<Enrollment>
    {
        public EnrollmentValidator()
        {
            RuleFor(i => i.CourseId).NotEqual(Guid.Empty).NotNull().WithErrorCode("ENROLLMENT_COURSE-01");
            RuleFor(i => i.StudentId).NotEqual(Guid.Empty).NotNull().WithErrorCode("ENROLLMENT_STUDENT-01");
        }
    }
}
