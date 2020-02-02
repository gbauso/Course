using Domain.Model;
using FluentValidation;
using System;

namespace Domain.Validators
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(i => i.Id).NotEqual(Guid.Empty).NotNull().WithErrorCode("COURSE_ID-01");
            RuleFor(i => i.Title).NotEqual(string.Empty).NotNull().WithErrorCode("COURSE_TITLE-01");
            RuleFor(i => i.Capacity).GreaterThan(0).WithErrorCode("COURSE_CAPACITY-01");
            RuleFor(i => i.Lecturer).NotNull().WithErrorCode("COURSE_LECTURER-01");
        }
    }
}
