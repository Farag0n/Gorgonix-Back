using Gorgonix_Back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class ContentConfigurations : IEntityTypeConfiguration<Content>
{
    public void Configure(EntityTypeBuilder<Content> builder)
    {
        builder.ToTable("Contents");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // Relación con Genre (Uno a Muchos)
        builder.HasOne(c => c.Genre)
            .WithMany(g => g.Contents)
            .HasForeignKey(c => c.GenreId)
            .OnDelete(DeleteBehavior.Restrict); // Evita borrar el género si tiene contenidos

        // Relación con Reviews (Uno a Muchos)
        builder.HasMany(c => c.Reviews)
            .WithOne(r => r.Content)
            .HasForeignKey(r => r.ContentId)
            .OnDelete(DeleteBehavior.Cascade); // Si borras la peli, se borran las reviews
    }
}