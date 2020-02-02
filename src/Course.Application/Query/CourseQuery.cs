using Infrastructure.Database.Query.Model;
using MediatR;
using System;

namespace Application.Query
{
    public class CourseQuery : IRequest<Course>
    {
        public CourseQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
