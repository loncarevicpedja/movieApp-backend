using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Internal.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplication(this IServiceCollection services)
        {
            // automapper config
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
