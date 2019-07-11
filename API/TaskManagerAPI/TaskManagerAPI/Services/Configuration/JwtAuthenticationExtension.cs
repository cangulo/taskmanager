using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManagerAPI.Resources.AppSettings;
using TaskManagerAPI.Resources.Constants;

namespace TaskManagerAPI.Services.Configuration
{
    public static class JwtAuthenticationExtension
    {
        public static IServiceCollection AddDefaultJwtAuthorization(this IServiceCollection serviceCollection, AppSettings appSettings)
        {
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            serviceCollection.
                AddAuthorization(options =>
                {
                    options.AddJwtDefaultPolicy();
                }).
                AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).
                AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x =>
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

            return serviceCollection;
        }

        public static AuthorizationOptions AddJwtDefaultPolicy(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            options.AddPolicy(ConfigurationConstants.JwtDefaultPolicy, policy);
            return options;
        }
    }
}
