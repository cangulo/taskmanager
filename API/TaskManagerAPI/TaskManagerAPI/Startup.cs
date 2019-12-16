using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.Models.FE.Validators.APIRequests;
using TaskManagerAPI.Models.Mappers;
using TaskManagerAPI.Resources.AppSettings;
using TaskManagerAPI.Pipeline;
using TaskManagerAPI.StartupConfiguration.Extensions;
using TaskManagerAPI.Extensions;
using MediatR;
using TaskManagerAPI.CQRS.Authorization.Commands;
using TaskManagerAPI.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

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

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConfiguration(_configuration.GetSection("Logging"));
                builder.AddDebug();
                builder.AddConsole();
                builder.AddEventSourceLogger();
                builder.AddApplicationInsights();
                builder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Information);
            });



            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            services.AddEntityFrameworkDbConfiguration(appSettings, _configuration);

            services.AddDefaultJwtAuthorization(appSettings);

            services.AddBeServices();
            services.AddBeRepositories();
            services.AddBeErrorsHelpers();

            services.AddSwaggerConfiguration();

            services.AddMediatR(typeof(LoginCommand));
            services.AddAutoMapper(typeof(DomainProfile), typeof(CQMapperProfile));

            #region Basic MVC HTTP

            services.AddHttpContextAccessor();
            services.AddCors();
            services.AddMvc().AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

            #endregion
        }

        /// <summary>
        /// DB Initialized and Custom Exception Handler configured
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app)
        {
            #region EF Database

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
            {
                serviceScope.ServiceProvider.GetService<IDbInitializer>().StartDbContext();
            }

            #endregion

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseMvc();
        }
    }
}
