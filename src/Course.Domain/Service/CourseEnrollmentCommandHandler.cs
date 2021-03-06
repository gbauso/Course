﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Model;
using CrossCutting.Exceptions;
using Domain.Interfaces;
using CrossCutting.ServiceBus;

namespace Domain.Service
{
    public class CourseEnrollmentCommandHandler : IRequestHandler<CourseEnrollmentCommand, bool>
    {
        private readonly IStudentRepository _StudentRepository;
        private readonly ICourseRepository _CourseRepository;
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IQueuePublisher _Publisher;

        public CourseEnrollmentCommandHandler(IStudentRepository studentRepository,
                                              ICourseRepository courseRepository,
                                              IUnitOfWork unitOfWork,
                                              IQueuePublisher publisher)
        {
            _StudentRepository = studentRepository;
            _CourseRepository = courseRepository;
            _UnitOfWork = unitOfWork;
            _Publisher = publisher;
        }

        public async Task<bool> Handle(CourseEnrollmentCommand request, CancellationToken cancellationToken)
        {
            var course = await _CourseRepository.GetCourseInfo(request.CourseId);
            if (course == default)
                throw new ElementNotFoundException();

            var student = await _StudentRepository.FindStudent(request.StudentName, request.StudentAge);
            if (student == default)
            {
                student = new Student(request.StudentName, request.StudentAge);
                await _StudentRepository.Add(student);
            }

            var success = true;
            try
            {
                course.Enroll(student);

                await _CourseRepository.Update(course);
                await _UnitOfWork.Commit();
            }
            catch
            {
                success = false;
                throw;
            }
            finally
            {
                await _Publisher.Publish(new BusMessage("NotifyEnrollment",
                                                        new { Notification = new EmailNotification(success) }),
                                         "course.queue");
            }

            return true;
        }
    }
}
