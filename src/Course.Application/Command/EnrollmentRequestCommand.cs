using Application.Validator;
using CrossCutting.Exceptions;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Command
{
    public class EnrollmentRequestCommand : IRequest<bool>
    {
        public Guid CourseId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }

        public async Task Validate()
        {
            var validator = new EnrollmentRequestCommandValidator();
            var validationResult = await validator.ValidateAsync(this);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorCode));
        }
    }
}
