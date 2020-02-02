using CrossCutting.Exceptions;
using Domain.Validators;
using System;
using System.Linq;

namespace Domain.Model
{
    public class Enrollment : IDomain
    {
        public Enrollment()
        {

        }
        public Enrollment(Course course, Student student)
        {
            Course = course;
            CourseId = course.Id;

            Student = student;
            StudentId = student.Id;
        }

        public Guid CourseId { get; private set; }
        public Guid StudentId { get; private set; }
        public virtual Course Course { get; private set; }
        public virtual Student Student { get; private set; }

        public void Validate()
        {
            var validator = new EnrollmentValidator();
            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorMessage));
        }
    }
}
