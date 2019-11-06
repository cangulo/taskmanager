using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.Models.FE.Validators.APIRequests;
using TaskManagerAPI.Models.Mappers;
using TaskManagerAPI.Resources.AppSettings;
using TaskManagerAPI.Pipeline;
using TaskManagerAPI.StartupConfiguration.Extensions;
using TaskManagerAPI.Extensions;

namespace TaskManagerAPI
{
    public class Startup
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        public readonly IConfiguration _configuration;

        public Startup(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<Startup>();
            _configuration = configuration;
        }


        /// <summary>
        /// Configured Entity Framework Core, DB, JWT Token, BE Services and FluentValidations for the input
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILogger>((ctx) =>
            {
                return _loggerFactory.CreateLogger<object>();
            });

            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            AppSettings appSettings = appSettingsSection.Get<AppSettings>();
            this._logger.LogInformation("Configuring BD EF");
            services.AddEntityFrameworkDbConfiguration(appSettings, _configuration);

            services.AddDefaultJwtAuthorization(appSettings);

            services.AddBeServices();

            services.AddSwaggerConfiguration();

            #region Basic MVC HTTP

            services.AddAutoMapper(typeof(DomainProfile));
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            #region EF Database

            try
            {
                this._logger.LogInformation("Starting DB EF");
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                    .CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<IDbInitializer>().StartDbContext();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Failed to migrate or seed database");
            }

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCustomExceptionHandler(env, _logger);

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

            app.UseMvc();
        }
    }
}
