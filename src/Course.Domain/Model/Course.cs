using CrossCutting.Exceptions;
using Domain.Exceptions;
using Domain.Validators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public class Course : IDomain
    {
        public Course()
        {

        }

        public Course(Guid id,string title, int capacity, Lecturer lecturer)
        {
            Id = id;
            Title = title;
            Capacity = capacity;
            Lecturer = lecturer;
            LecturerId = lecturer.Id;
            Enrollments = new HashSet<Enrollment>();
        }

        public Course(string title, int capacity, Lecturer lecturer)
        {
            Id = Guid.NewGuid();
            Title = title;
            Capacity = capacity;
            Lecturer = lecturer;
            LecturerId = lecturer.Id;
            Enrollments = new HashSet<Enrollment>();
        }

        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public int Capacity { get; private set; }
        public Guid LecturerId { get; private set; }
        public DateTime Updated { get; set; }

        public virtual Lecturer Lecturer { get; private set; }
        public virtual ICollection<Enrollment> Enrollments { get; private set; }

        public void Enroll(Student student)
        {
            student.Validate();
            if (Enrollments.Any(i => i.StudentId == student.Id)) throw new DoubleApplicationException();
            if (Enrollments.Count >= Capacity) throw new CourseLimitException();

            var enrollment = new Enrollment(this, student);
            Enrollments.Add(enrollment);
            student.AddEnrollment(enrollment);

            Updated = DateTime.UtcNow;
        }

        public void Validate()
        {
            var validator = new CourseValidator();
            var validationResult = validator.Validate(this);

            if (!validationResult.IsValid) 
                throw new ValidationException(validationResult.Errors.Select(i => i.ErrorCode));
        }
    }
}
