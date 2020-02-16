using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.CQRS.TasksCQ.CommandValidators;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.Extensions;
using TaskManagerAPI.Extensions.AutofacModules;
using TaskManagerAPI.Mappers;
using TaskManagerAPI.Models.FE.Validators.APIRequests;
using TaskManagerAPI.Models.Mappers;
using TaskManagerAPI.Pipeline;
using TaskManagerAPI.Resources.AppSettings;
using TaskManagerAPI.StartupConfiguration.Extensions;

namespace TaskManagerAPI
{
    public class Startup
    {
        public readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Configured Entity Framework Core, DB, JWT Token, BE Services and FluentValidations for the input
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConfiguration(_configuration.GetSection("Logging"));
                builder.AddDebug();
                builder.AddConsole();
                builder.AddEventSourceLogger();
                builder.AddApplicationInsights();
            });

            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            services.AddEFDBContext(appSettings, _configuration);

            services.AddDefaultJwtAuthorization(appSettings);

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(LoginCommand));
            services.AddAutoMapper(typeof(DomainProfile), typeof(CQMapperProfile));

            #region Basic MVC HTTP

            services.AddHttpContextAccessor();
            services.AddCors();
            services
                .AddControllers()
                .AddNewtonsoftJson()
                .AddFluentValidation(fvc => fvc
                    .RegisterValidatorsFromAssemblies(
                        new Assembly[]{
                            typeof(LoginRequestValidator).Assembly,
                            typeof(DeleteTaskCommandValidator).Assembly
                        }));

            #endregion Basic MVC HTTP
        }

        // Register services directly with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new EntityFrameworkModule());
            builder.RegisterModule(new BeServicesModule());
            builder.RegisterModule(new RepositoriesModule());
            builder.RegisterModule(new ErrorsHelpersModule());
            builder.RegisterModule(new CQRSModule());
        }

        /// <summary>
        /// DB Initialized and Custom Exception Handler configured
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="logger"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            #region EF Database

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IDbInitializer>().StartDbContext();
            }

            #endregion EF Database

            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development environment");
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();
            app.UseCors(x =>
                x.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseAuthentication();

            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}