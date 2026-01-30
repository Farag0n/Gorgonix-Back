using Gorgonix_Back.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class ProfileConfigurations : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("Profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Relación con User
        builder.HasOne(p => p.User)
            .WithMany(u => u.Profiles)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Si se borra el usuario, se borran los perfiles
    }
}