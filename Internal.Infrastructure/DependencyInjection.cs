using Internal.Application.Common.Interfaces.Persistance;
using Internal.Infrastructure.Persistance;
using Internal.Infrastructure.Persistance.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Internal.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // database context
            var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MovieDbContext>(opt => 
                opt.UseSqlServer(defaultConnectionString));

            // repositories
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieDirectorRepository, MovieDirectorRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepository>();

            return services;
        }
    }
}
