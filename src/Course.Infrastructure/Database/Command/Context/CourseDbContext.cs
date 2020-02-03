using Microsoft.EntityFrameworkCore;
using Domain.Model;
using System;

namespace Infrastructure
{
    public class CourseDbContext : DbContext
    {
        public CourseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Ignore<Person>();

            modelBuilder.Entity<Lecturer>(cfg =>
            {
                cfg.ToTable("Lecturer");
                cfg.HasKey(i => i.Id);
                cfg.Property(i => i.Name).HasMaxLength(100).IsRequired();
                cfg.HasMany(i => i.Courses).WithOne(i => i.Lecturer);
            });

            modelBuilder.Entity<Course>(cfg =>
            {
                cfg.ToTable("Course");
                cfg.HasKey(i => i.Id);
                cfg.Property(i => i.Title).HasMaxLength(75).IsRequired();
                cfg.HasMany(i => i.Enrollments).WithOne(i => i.Course);
                cfg.HasOne(i => i.Lecturer).WithMany(i => i.Courses);
            });

            modelBuilder.Entity<Student>(cfg =>
            {
                cfg.ToTable("Student");
                cfg.HasKey(i => i.Id);
                cfg.Property(i => i.Name).HasMaxLength(100).IsRequired();
                cfg.HasMany(i => i.Enrollments).WithOne(i => i.Student);
                cfg.Property(i => i.Age).IsRequired();
            });


            modelBuilder.Entity<Enrollment>(cfg =>
            {
                cfg.ToTable("Enrollment");
                cfg.HasKey(i => new { i.CourseId, i.StudentId });
                cfg.HasIndex(i => new { i.CourseId, i.StudentId }).IsUnique();
            });
                
                
        }
    }
}
