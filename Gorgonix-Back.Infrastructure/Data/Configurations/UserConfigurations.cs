using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gorgonix_Back.Infrastructure.Data.Configurations;

public class UserConfigurations :  IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.HasKey(u => u.Id);
        
        builder.HasIndex(u => u.UserName).IsUnique();

        //configuracion de la propiedad username
        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(100); 

        //configuracion del value object Email
        builder.Property(u => u.Email)
            .HasConversion(
                email => email.Value,
                value => new Email(value))
            .IsRequired()
            .HasMaxLength(100);

        //configuracion del rol de usuario
        builder.Property(u => u.UserRole)
            .IsRequired()
            .HasMaxLength(100);

        //configuracion del hash de la contraseña
        builder.Property(u => u.PasswordHash)
            .IsRequired();

        //configracion del refresh token es opcional y tiene tamaño maximo
        builder.Property(u => u.RefreshToken)
            .HasMaxLength(500);
    }
}