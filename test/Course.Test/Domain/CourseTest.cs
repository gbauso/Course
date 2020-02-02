using Domain.Exceptions;
using FluentAssertions;
using System;
using System.Linq;
using Test.Helpers;
using Xunit;

namespace Test.Domain
{
    public class CourseTest
    {
        [Fact(DisplayName = "Enroll to a not full course")]
        [Trait("Domain", "Course")]
        public void Domain_Course_Enroll_NotFullCourse()
        {
            // Arrange

            var course = MockHelper.GetCourse(1);

            // Act

            var student = MockHelper.GetStudentFaker().Generate();
            Action enrollment =  () => course.Enroll(student);

            // Assert

            enrollment.Should().NotThrow();
            course.Enrollments.Should().HaveCount(1);
            course.Enrollments.Any(i => i.StudentId == student.Id).Should().BeTrue();
            student.Enrollments.Any(i => i.CourseId == course.Id).Should().BeTrue();
        }

        [Fact(DisplayName = "Try to enroll twice")]
        [Trait("Domain", "Course")]
        public void Domain_Course_Enroll_EnrollTwice_ShouldThrowDomainException()
        {
            // Arrange

            var course = MockHelper.GetCourse(1);

            // Act

            var student = MockHelper.GetStudentFaker().Generate();
            Action firstEnrollment = () => course.Enroll(student);
            Action secondEnrollment = () => course.Enroll(student);

            // Assert

            // Function
            firstEnrollment.Should().NotThrow();
            secondEnrollment.Should().Throw<DomainException>();

            // Outcome
            course.Enrollments.Should().HaveCount(1);
            course.Enrollments.Any(i => i.StudentId == student.Id).Should().BeTrue();

            student.Enrollments.Any(i => i.CourseId == course.Id).Should().BeTrue();
        }

        [Fact(DisplayName = "Try to enroll in a full course")]
        [Trait("Domain", "Course")]
        public void Domain_Course_Enroll_EnrollFullCourse_ShouldThrowDomainException()
        {
            // Arrange

            var course = MockHelper.GetCourse(1);

            // Act

            var students = MockHelper.GetStudentFaker().Generate(2);

            Action firstEnrollment = () => course.Enroll(students.ElementAtOrDefault(0));
            Action secondEnrollment = () => course.Enroll(students.ElementAtOrDefault(1));

            // Assert

            firstEnrollment.Should().NotThrow();
            secondEnrollment.Should().Throw<DomainException>();

            course.Enrollments.Should().HaveCount(1);
            course.Enrollments.Any(i => i.StudentId == students.ElementAtOrDefault(0).Id).Should().BeTrue();
            course.Enrollments.Any(i => i.StudentId == students.ElementAtOrDefault(1).Id).Should().BeFalse();

            students.ElementAtOrDefault(0).Enrollments.Any(i => i.CourseId == course.Id).Should().BeTrue();
            students.ElementAtOrDefault(1).Enrollments.Any(i => i.CourseId == course.Id).Should().BeFalse();
        }

    }
}
