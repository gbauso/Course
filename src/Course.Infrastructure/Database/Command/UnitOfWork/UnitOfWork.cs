using Domain.Interfaces;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CourseDbContext _Context;

        public UnitOfWork(CourseDbContext context)
        {
            _Context = context;
        }

        public async Task Commit()
        {
            await _Context.SaveChangesAsync();
        }
    }
}
