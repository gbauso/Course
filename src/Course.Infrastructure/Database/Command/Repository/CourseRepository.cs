using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly CourseDbContext _Context;

        public CourseRepository(CourseDbContext courseDbContext)
        {
            _Context = courseDbContext;
        }

        public Task Update(Course course)
        {
            course.SetUpdatedDate();
            _Context.Entry(course).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public async Task<Course> GetCourseInfo(Guid id)
        {
            return await _Context
                                .Courses
                                .Include(i => i.Enrollments)
                                    .ThenInclude(e => e.Student)
                                .Include(i => i.Lecturer)
                                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<IEnumerable<Course>> GetCourses(Func<Course, bool> predicate)
        {
            return Task.FromResult(_Context
                                    .Courses
                                    .AsNoTracking()
                                    .Include(i => i.Enrollments)
                                        .ThenInclude(e => e.Student)
                                    .Include(i => i.Lecturer)
                                    .Where(predicate)
                                   );
        }
    }
}
