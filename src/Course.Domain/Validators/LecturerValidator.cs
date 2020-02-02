using Domain.Model;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Validators
{
    public class LecturerValidator : AbstractValidator<Lecturer>
    {
        public LecturerValidator()
        {
            RuleFor(i => i.Id).NotEqual(Guid.Empty).NotNull().WithErrorCode("LECTURER_ID-01");
            RuleFor(i => i.Name).NotEqual(string.Empty).NotNull().WithErrorCode("LECTURER_NAME-01");
        }
    }
}
