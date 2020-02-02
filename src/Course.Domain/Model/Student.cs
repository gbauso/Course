using CrossCutting.Exceptions;
using Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class Student : Person
    {
        public Student()
        {
                
        }

        public Student(Guid id, string name, int age, ICollection<Enrollment> enrollments = default) : base(id, name)
        {
            Age = age;
            Enrollments = enrollments ?? new HashSet<Enrollment>();
        }

        public Student(string name, int age) : base(Guid.NewGuid(), name)
        {
            Name = name;
            Age = age;
            Enrollments = new HashSet<Enrollment>();
        }

        public int Age { get; private set; }
        public virtual ICollection<Enrollment> Enrollments { get; private set; }

        public void AddEnrollment(Enrollment enrollment)
        {
            enrollment.Validate();
            Enrollments.Add(enrollment);
        }

        public override void Validate()
        {
            var validator = new StudentValidator();
            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorCode));
        }
    }
}
