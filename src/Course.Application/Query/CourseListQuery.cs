using Infrastructure.Database.Query.Model;
using MediatR;
using System.Collections.Generic;

namespace Application.Query
{
    public class CourseListQuery : IRequest<IEnumerable<SimpleCourse>>
    {

    }
}
