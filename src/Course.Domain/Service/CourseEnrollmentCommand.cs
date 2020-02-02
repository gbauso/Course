using CrossCutting.Exceptions;
using Domain.Validators;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class CourseEnrollmentCommand : IRequest<bool>
    {
        public Guid CourseId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }

        public async Task Validate()
        {
            var validator = new CourseEnrollmentCommandValidator();
            var validationResult = await validator.ValidateAsync(this);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorCode));

        }
    }
}
