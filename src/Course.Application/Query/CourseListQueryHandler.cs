using Infrastructure.Database.Query;
using Infrastructure.Database.Query.Model;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Query
{
    public class CourseListQueryHandler : IRequestHandler<CourseListQuery, IEnumerable<SimpleCourse>>
    {
        private readonly IReadDatabase<Course> _ReadDatabase;

        public CourseListQueryHandler(IReadDatabase<Course> readDatabase)
        {
            _ReadDatabase = readDatabase;
        }

        public async Task<IEnumerable<SimpleCourse>> Handle(CourseListQuery request, CancellationToken cancellationToken)
        {
            var list = await _ReadDatabase.GetItems(new[] { "id", "Capacity", "Title", "MaxAge", "MinAge", "AvgAge", "EnrollmentCount" });

            return list as IEnumerable<SimpleCourse>;
        }
    }
}   
