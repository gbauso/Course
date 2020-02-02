using Infrastructure.Database.Query;
using Infrastructure.Database.Query.Model;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Query
{
    public class CourseQueryHandler : IRequestHandler<CourseQuery, Course>
    {
        private readonly IReadDatabase<Course> _ReadDatabase;

        public CourseQueryHandler(IReadDatabase<Course> readDatabase)
        {
            _ReadDatabase = readDatabase;
        }

        public async Task<Course> Handle(CourseQuery request, CancellationToken cancellationToken)
        {
            return await _ReadDatabase.GetItem(request.Id.ToString());
        }
    }
}
