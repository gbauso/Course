using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Domain.Model;
using CrossCutting.Exceptions;
using Domain.Interfaces;

namespace Domain.Service
{
    public class CourseEnrollmentCommandHandler : IRequestHandler<CourseEnrollmentCommand, bool>
    {
        private readonly IStudentRepository _StudentRepository;
        private readonly ICourseRepository _CourseRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public CourseEnrollmentCommandHandler(IStudentRepository studentRepository,
                                              ICourseRepository courseRepository,
                                              IUnitOfWork unitOfWork)
        {
            _StudentRepository = studentRepository;
            _CourseRepository = courseRepository;
            _UnitOfWork = unitOfWork;
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

            course.Enroll(student);

            await _CourseRepository.Update(course);
            await _UnitOfWork.Commit();

            return true;
        }
    }
}
