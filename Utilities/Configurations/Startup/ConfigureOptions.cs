using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Burak.GoodJobGames.Utilities.Configurations.Startup
{
    public static class ConfigureOptions
    {
        public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}
