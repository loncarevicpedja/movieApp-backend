using Internal.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Internal.Infrastructure.Persistance
{
    public class MovieDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<WatchList> WatchLists { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<MovieDirector> Directors { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // add all the configurations from assembly
            builder.ApplyConfigurationsFromAssembly(typeof(MovieDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
