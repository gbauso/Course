using Infrastructure.Database.Query.Model;
using Newtonsoft.Json;

namespace Infrastructure.Database.Query.Model
{
    public class SimpleCourse : IQueryModel
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Title { get; set; }
        public int Capacity { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public int AvgAge { get; set; }
        public int EnrollmentCount { get; set; }
    }
}
