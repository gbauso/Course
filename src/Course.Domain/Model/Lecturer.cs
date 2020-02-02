using CrossCutting.Exceptions;
using Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class Lecturer : Person
    {

        public Lecturer()
        {

        }
        public Lecturer(string name) : base(Guid.NewGuid(), name)
        {
            Courses = new HashSet<Course>();
        }


        public virtual ICollection<Course> Courses { get; protected set; }
        public override void Validate()
        {
            var validator = new LecturerValidator();
            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorCode));
        }
    }
}
