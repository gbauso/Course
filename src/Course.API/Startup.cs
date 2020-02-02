using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using API.Filter;
using Domain.Interfaces;
using Infrastructure.Repository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Domain.Service;
using Infrastructure.UnitOfWork;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(cfg =>
            {
                cfg.Filters.Add(typeof(ErrorHandlingFilter));
            });

            services.AddMediatR(typeof(CourseEnrollmentCommandHandler));

            services.AddDbContext<CourseDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("CourseDbContext"),
                    ac => ac.MigrationsAssembly("Course.Infrastructure"));
            });

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Course", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Course V1");
            });

            InitializeDatabase(app);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void InitializeDatabase(IApplicationBuilder applicationBuilder)
        {
            var context = applicationBuilder.ApplicationServices.GetRequiredService<CourseDbContext>();

            context.Database.EnsureCreated();
            context.Database.Migrate();
        }
    }
}