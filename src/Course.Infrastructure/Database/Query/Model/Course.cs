namespace Infrastructure.Database.Query.Model
{
    public class Course : SimpleCourse
    {
        public string LecturerName { get; set; }
        public string[] Students { get; set; }

    }
}
