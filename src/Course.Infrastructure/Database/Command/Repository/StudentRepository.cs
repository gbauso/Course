using Domain.Interfaces;
using Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CourseDbContext _Context;

        public StudentRepository(CourseDbContext courseDbContext)
        {
            _Context = courseDbContext;
        }

        public async Task Add(Student student)
        {
            await _Context.Students.AddAsync(student);
        }

        public async Task<Student> FindStudent(string name, int age)
        {
            return await _Context
                                .Students
                                .FirstOrDefaultAsync(i =>
                                    i.Name.ToLower() == name.ToLower() &&
                                    i.Age == age
                                 );
        }
    }
}
