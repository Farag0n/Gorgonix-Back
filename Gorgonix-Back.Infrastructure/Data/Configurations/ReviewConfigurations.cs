using Gorgonix_Back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class ReviewConfigurations : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Body)
            .IsRequired()
            .HasMaxLength(2000);

        // Relación con Profile
        builder.HasOne(r => r.Profile)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProfileId)
            .OnDelete(DeleteBehavior.NoAction); // Evitar ciclos de borrado
    }
}