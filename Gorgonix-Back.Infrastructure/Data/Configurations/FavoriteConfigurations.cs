using Gorgonix_Back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class FavoriteConfigurations : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");

        // Llave compuesta (ProfileId + ContentId)
        builder.HasKey(f => new { f.ProfileId, f.ContentId });

        builder.HasOne(f => f.Profile)
            .WithMany(p => p.Favorites)
            .HasForeignKey(f => f.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(f => f.Content)
            .WithMany(c => c.FavoritedBy)
            .HasForeignKey(f => f.ContentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}