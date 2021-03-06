using Domain.Interfaces;
using Infrastructure;
using Infrastructure.Repository;
using Infrastructure.ServiceBus.Azure;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Worker.Subscriber;
using MediatR;
using Domain.Service;
using CrossCutting.ServiceBus;
using Infrastructure.Email;
using Serilog;
using System;

namespace Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IQueueSubscriber, AzureSubscriber>();
                    services.AddSingleton<IQueuePublisher, AzurePublisher>();
                    services.AddSingleton<IMessageSubscriber, QueueSubscribe>();

                    services.AddSingleton<INotifier, MockEmailSender>();

                    services.AddSingleton<IStudentRepository, StudentRepository>();
                    services.AddSingleton<ICourseRepository, CourseRepository>();
                    services.AddSingleton<IUnitOfWork, UnitOfWork>();

                    services.AddMediatR(typeof(CourseEnrollmentCommandHandler));

                    services.AddDbContext<CourseDbContext>(cfg =>
                    {
                        cfg.UseSqlServer(hostContext.Configuration.GetSection("ConnectionStrings")["CourseDbContext"],
                            mssqlOptions =>
                            {
                                mssqlOptions.MigrationsAssembly("Course.Infrastructure");
                                mssqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 3,
                                    maxRetryDelay: TimeSpan.FromSeconds(10),
                                    errorNumbersToAdd: null);
                            }
                            );
                    }, ServiceLifetime.Singleton, ServiceLifetime.Singleton);

                    services.AddHostedService<Worker>();
                }).ConfigureLogging((host, log) =>
                {
                    var logger = new LoggerConfiguration()
                                .WriteTo.Console()
                                .WriteTo.File(host.Configuration.GetSection("Logging")["Path"])
                                .CreateLogger();

                    log.AddSerilog(logger);
                });
    }
}
