using Domain.Model;
using FluentValidation;
using System;

namespace Domain.Validators
{
    public class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(i => i.Id).NotEqual(Guid.Empty).NotNull().WithErrorCode("STUDENT_ID-01");
            RuleFor(i => i.Name).NotNull().NotEqual(string.Empty).WithErrorCode("STUDENT_NAME-01");
            RuleFor(i => i.Age).GreaterThan(0).WithErrorCode("STUDENT_AGE-01");
        }
    }
}
