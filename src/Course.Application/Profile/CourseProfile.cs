using AutoMapper;
using QueryData = Infrastructure.Database.Query.Model;
using Domain.Model;
using System.Linq;

namespace Application.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, QueryData.Course>()
                .ForMember(i => i.AvgAge, f => f.MapFrom(m => m.Enrollments.DefaultIfEmpty().Average(i => i.Student.Age)))
                .ForMember(i => i.MinAge, f => f.MapFrom(m => m.Enrollments.DefaultIfEmpty().Min(i => i.Student.Age)))
                .ForMember(i => i.MaxAge, f => f.MapFrom(m => m.Enrollments.DefaultIfEmpty().Max(i => i.Student.Age)))
                .ForMember(i => i.EnrollmentCount, f => f.MapFrom(m => m.Enrollments.Count()))
                .ForMember(i => i.LecturerName, f => f.MapFrom(m => m.Lecturer.Name))
                .ForMember(i => i.Students, f => f.MapFrom(m => m.Enrollments.Select(i => i.Student.Name)));
        }
    }
}
