using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MediatR;
using AutoMapper;
using API.Filter;
using Microsoft.OpenApi.Models;
using Application.Command;
using CrossCutting.ServiceBus;
using Infrastructure.ServiceBus.Azure;
using Domain.Interfaces;
using Infrastructure.Repository;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Profiles;
using Application.Query;
using Infrastructure.Database.Query;
using Infrastructure.Database.Query.Model;
using System;

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

            services.AddAutoMapper(typeof(CourseProfile));
            services.AddMediatR(typeof(EnrollmentRequestHandler),
                                typeof(SyncDatabaseCommandHandler),
                                typeof(CourseListQueryHandler),
                                typeof(CourseQueryHandler));

            services.AddSingleton<IQueuePublisher, AzurePublisher>();

            services.AddDbContext<CourseDbContext>(cfg =>
            {
                cfg.UseSqlServer(Configuration.GetConnectionString("CourseDbContext"),
                    mssqlOptions =>
                    {
                        mssqlOptions.MigrationsAssembly("Course.Infrastructure");
                        mssqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    });
            });

            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IReadDatabase<Course>, CosmosDbManager<Course>>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Course", Version = "v2" });
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
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "Course V2");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}