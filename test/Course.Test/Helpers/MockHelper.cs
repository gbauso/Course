using Bogus;
using Domain.Interfaces;
using Domain.Model;
using Domain.Service;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Helpers
{
    public class MockHelper
    {
        public static Guid BillId => Guid.NewGuid(); 

        public static Faker<Lecturer> GetLectureFaker()
        {
            var faker = new Faker<Lecturer>()
                                    .CustomInstantiator(f => new Lecturer(f.Name.FullName()));

            return faker;
        }

        public static Faker<Student> GetStudentFaker()
        {
            var faker = new Faker<Student>()
                                    .CustomInstantiator(f => new Student(f.Name.FullName(), f.Random.Int(10, 75)));

            return faker;
        }

        public static Faker<Course> GetCourseFaker(int? capacity = null)
        {
            var lecturer = GetLectureFaker().Generate();

            var courseFaker = new Faker<Course>()
                                    .CustomInstantiator(f => new Course(
                                        f.Commerce.Product(),
                                        capacity ?? f.Random.Int(),
                                        lecturer)
                                    );

            return courseFaker;

        }

        public static Course GetCourse(int capacity = 3)
        {
            var course = GetCourseFaker(capacity).Generate();

            return course;
        }

        public static IRequestHandler<CourseEnrollmentCommand, bool> GetRequestHandler(Guid? customCourse = null,
                                                                                       Guid? billId = null,
                                                                                       int capacity = 2)
        {
            var handler = new CourseEnrollmentCommandHandler(GetStudentRepository(billId),
                                                             GetCourseRepository(customCourse, billId, capacity),
                                                             GetUnitOfWork());

            return handler;
        }

        private static IStudentRepository GetStudentRepository(Guid? billId = null)
        {
            var mock = new Mock<IStudentRepository>();

            var studentList = GetStudentFaker().Generate(5);
            studentList.Add(new Student(billId ?? BillId, "Bill", 25));

            mock.Setup(o => o.FindStudent(It.IsAny<string>(), It.IsAny<int>()))
                        .Returns<string,int>(
                                    (name, age) => 
                                                Task.FromResult(
                                                    studentList
                                                        .FirstOrDefault(i => i.Name.ToLower() == name.ToLower()
                                                        && i.Age == age)
                                                    )
                                            );

            mock.Setup(o => o.Add(It.IsAny<Student>())).Callback<Student>((student) => studentList.Add(student));

            return mock.Object;
        }

        private static ICourseRepository GetCourseRepository(Guid? customCourse = null,
                                                             Guid? billId = null,
                                                             int capacity = 2)
        {
            var mock = new Mock<ICourseRepository>();
            var courseList = GetCourseFaker().Generate(2);

            courseList.Add(new Course(customCourse ?? Guid.NewGuid(),
                                      "custom",
                                      capacity,
                                      GetLectureFaker().Generate())
                           );

            mock.Setup(o => o.GetCourseInfo(It.Is<Guid>(i => !customCourse.HasValue)))
                .Returns<Guid>((id) =>
                {
                    var course = courseList.FirstOrDefault(i => i.Id == id);
                    return Task.FromResult(course);
                }
                );

            mock.Setup(o => o.GetCourseInfo(It.Is<Guid>(i => customCourse.HasValue)))
                .Returns<Guid>((id) =>
                {
                    var course = courseList.FirstOrDefault(i => i.Id == id);
                    
                    course.Enroll(new Student(billId ?? BillId, "Bill", 25));

                    return Task.FromResult(course);
                });

            mock.Setup(o => o.Update(It.IsAny<Course>()))
                .Callback<Course>((course) => 
                {
                    var original = courseList.Find(i => i.Id == course.Id);
                    courseList.Remove(original);
                    courseList.Add(course);
                });

            return mock.Object;
        }

        private static IUnitOfWork GetUnitOfWork()
        {
            var mock = new Mock<IUnitOfWork>();

            return mock.Object;
        }

    }
}
