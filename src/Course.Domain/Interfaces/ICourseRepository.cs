using Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetCourses(Func<Course, bool> predicate);
        Task<Course> GetCourseInfo(Guid id);
        Task Update(Course course);

    }
}
