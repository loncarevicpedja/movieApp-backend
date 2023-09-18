using Internal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Internal.Infrastructure.Persistance.Configurations
{
    public class RatingConfigurations : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            builder.ToTable("Internal_Rating");

            builder
                .Property(r => r.Value)
                .IsRequired();

            builder
                .HasOne<Movie>()
                .WithMany(m => m.Ratings)
                .HasForeignKey(r => r.MovieId);

            //builder
            //    .HasOne(r => r.Rater)
            //    .WithMany()
            //    .HasForeignKey(r => r.UserId);
        }
    }
}
