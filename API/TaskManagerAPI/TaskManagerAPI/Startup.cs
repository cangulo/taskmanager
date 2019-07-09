using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using TaskManagerAPI.BL.AuthProcess;
using TaskManagerAPI.EF.Context;
using TaskManagerAPI.EF.DbInitializer;
using TaskManagerAPI.EF.MigrationManager;
using TaskManagerAPI.Resources.AppSettings;

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


        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region LogggerFactory

            services.AddSingleton<ILogger>((ctx) =>
            {
                return _loggerFactory.CreateLogger<object>();
            });

            #endregion

            #region EF Database

            this._logger.LogInformation("Configuring BD EF");

            services.
                AddEntityFrameworkSqlServer().
                AddDbContext<TaskManagerDbContext>(opt =>
                    opt.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaskManagerApi;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

            services.AddTransient<IDBMigrationsManager, DBMigrationsManager>((ctx) =>
            {
                return new DBMigrationsManager(ctx.GetService<TaskManagerDbContext>(), ctx.GetService<ILogger<DBMigrationsManager>>());
            });
            services.AddTransient<IDbInitializer, DbInitializer>((ctx) =>
            {
                return new DbInitializer(ctx.GetService<TaskManagerDbContext>(), ctx.GetService<IDBMigrationsManager>(), ctx.GetService<ILogger<DbInitializer>>());
            });
            services.AddScoped<ITaskManagerDbContext, TaskManagerDbContext>();

            #endregion

            #region BL - Services 

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenCreator, TokenCreator>();

            #endregion

            #region App Settings

            var appSettingsSection = _configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            #endregion

            #region JWT Authentication

            var key = Encoding.ASCII.GetBytes(appSettingsSection.Get<AppSettings>().Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion

            #region Basic MVC HTTP

            services.AddCors();
            services.AddMvc();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                // I'm using Serilog here, but use the logging solution of your choice.
                this._logger.LogError(ex, "Failed to migrate or seed database");
            }

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
