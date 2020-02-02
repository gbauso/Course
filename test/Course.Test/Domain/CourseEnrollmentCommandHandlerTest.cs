using CrossCutting.Exceptions;
using Domain.Exceptions;
using Domain.Service;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Test.Helpers;
using Xunit;

namespace Test.Domain
{
    public class CourseEnrollmentCommandHandlerTest
    {
        [Theory(DisplayName = "Valid Command")]
        [Trait("Command", "Course")]
        [InlineData(true ,"Carl", 23)]
        [InlineData(true, "Bill", 10)]
        [InlineData(true, "John", 17)]
        public void CourseEnrollmentCommand_Validate_ValidCommand(bool generateValidGuid,
                                                                        string studentName,
                                                                        int studentAge)
        {
            var id = generateValidGuid ? Guid.NewGuid() : Guid.Empty;
            var command = new CourseEnrollmentCommand()
            {
                CourseId = id,
                StudentAge = studentAge,
                StudentName = studentName
            };

            Action validation = () => command.Validate().Wait();

            validation.Should().NotThrow();
        }

        [Theory(DisplayName = "Invalid Command")]
        [Trait("Command", "Course")]
        [InlineData(true, "", 23)]
        [InlineData(false, "Bill", 10)]
        [InlineData(true, "John", 0)]
        [InlineData(true, "John", -1)]
        public void CourseEnrollmentCommand_Validate_InvalidCommand(bool generateValidGuid,
                                                                        string studentName,
                                                                        int studentAge)
        {
            var id = generateValidGuid ? Guid.NewGuid() : Guid.Empty;
            var command = new CourseEnrollmentCommand()
            {
                CourseId = id,
                StudentAge = studentAge,
                StudentName = studentName
            };

            Action validation = () => command.Validate().Wait();

            validation.Should().Throw<ValidationException>();
        }

        [Fact(DisplayName = "New Student")]
        [Trait("Command", "Course")]
        public async Task CourseEnrollmentCommandHandler_Handle_NewStudent()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var handler = MockHelper.GetRequestHandler(courseId);

            var command = new CourseEnrollmentCommand()
            {
                CourseId = courseId,
                StudentAge = 16,
                StudentName = "Carl"
            };

            // Act

            var result = await handler.Handle(command, default);

            // Assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Double Application")]
        [Trait("Command", "Course")]
        public void CourseEnrollmentCommandHandler_Handle_DoubleApplication()
        {
            // Arrange
            var courseId = Guid.NewGuid();
            var billId = Guid.NewGuid();

            var handler = MockHelper.GetRequestHandler(courseId, billId);

            var command = new CourseEnrollmentCommand()
            {
                CourseId = courseId,
                StudentAge = 25,
                StudentName = "Bill"
            };

            // Act

            Action handle = () => handler.Handle(command, default).Wait();

            // Assert
            handle.Should().Throw<DoubleApplicationException>();
        }

        [Fact(DisplayName = "Course Full")]
        [Trait("Command", "Course")]
        public void CourseEnrollmentCommandHandler_Handle_CourseFull()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            var handler = MockHelper.GetRequestHandler(courseId, capacity: 1);

            var command = new CourseEnrollmentCommand()
            {
                CourseId = courseId,
                StudentAge = 24,
                StudentName = "William"
            };

            // Act

            Action handle = () => handler.Handle(command, default).Wait();

            // Assert
            handle.Should().Throw<CourseLimitException>();
        }

        [Fact(DisplayName = "Inexistent Course")]
        [Trait("Command", "Course")]
        public void CourseEnrollmentCommandHandler_Handle_InexistentCourse()
        {
            // Arrange
            var courseId = Guid.NewGuid();

            var handler = MockHelper.GetRequestHandler();

            var command = new CourseEnrollmentCommand()
            {
                CourseId = courseId,
                StudentAge = 24,
                StudentName = "William"
            };

            // Act

            Action handle = () => handler.Handle(command, default).Wait();

            // Assert
            handle.Should().Throw<ElementNotFoundException>();
        }
    }
}
