using AutoMapper;
using Domain.Interfaces;
using Infrastructure.Database.Query;
using Infrastructure.Database.Query.Model;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Command
{
    public class SyncDatabaseCommandHandler : IRequestHandler<SyncDatabaseCommand, bool>
    {
        private readonly ICourseRepository _CourseRepository;
        private readonly IReadDatabase<Course> _CourseQuery;
        private readonly IMapper _Mapper;

        public SyncDatabaseCommandHandler(ICourseRepository courseRepository,
                                          IReadDatabase<Course> courseQuery,
                                          IMapper mapper)
        {
            _CourseRepository = courseRepository;
            _CourseQuery = courseQuery;
            _Mapper = mapper;
        }

        public async Task<bool> Handle(SyncDatabaseCommand request, CancellationToken cancellationToken)
        {
            var reportIds = _CourseQuery.GetItems(new[] { "id" }).Result.Select(i => Guid.Parse(i.Id));

            var syncCourse = await _CourseRepository
                                        .GetCourses(i =>
                                                        i.Updated >= request.LastExecution
                                                        || !reportIds.Contains(i.Id)
                                                   );

            foreach (var item in syncCourse)
            {
                await _CourseQuery.RemoveItem(item.Id.ToString());
                await _CourseQuery.AddItem(_Mapper.Map<Course>(item));
            }

            return true;
        }
    }
}
