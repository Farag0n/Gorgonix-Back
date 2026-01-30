using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.HasIndex(u => u.UserName).IsUnique();

        // Configuración de la propiedad username
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100); 

        // Configuración del Value Object Email
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(100);

        // Configuración del rol de usuario
        builder.Property(u => u.UserRole)
            .IsRequired()
            .HasMaxLength(20); // Reduje esto a 20, 100 es mucho para un Enum convertido a string

        // Configuración del hash de la contraseña
        builder.Property(u => u.PasswordHash)
            .IsRequired();

        // Configuración del refresh token
        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);

        // =========================================================
        // RELACIÓN CON PROFILES (Uno a Muchos)
        // =========================================================
        // Un Usuario tiene muchos Perfiles
        builder.HasMany(u => u.Profiles)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Si borras al User, se borran sus Perfiles
    }
}