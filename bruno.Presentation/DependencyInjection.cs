using Microsoft.Extensions.DependencyInjection;

namespace bruno.Presentation
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContracts(this IServiceCollection services)
        { 
            return services;    
        }

    }
}
