using Domain.Model;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IStudentRepository
    {
        Task Add(Student student);
        Task<Student> FindStudent(string name, int age);
    }
}
