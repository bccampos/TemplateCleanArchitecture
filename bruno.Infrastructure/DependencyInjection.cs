using bruno.Application.Common.Interfaces.Authentication;
using bruno.Application.Common.Interfaces.Persistence;
using bruno.Application.Common.Interfaces.Services;
using bruno.Infrastructure.Authentication;
using bruno.Infrastructure.Authentication.Services;
using bruno.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace bruno.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;    
        }

    }
}
