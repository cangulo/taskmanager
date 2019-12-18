using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace TaskManagerAPI.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task Manager API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Carlos Angulo",
                        Email = "c.angulo.mascarell@outlook.com",
                        Url = new Uri("https://www.linkedin.com/in/angulomascarell")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            return serviceCollection;
        }
    }
}