using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Gorgonix_Back.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Content> Movies { get; set; } // Ojo: En tu código original se llamaba Movies, pero es Content
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Favorite> UserFavorites { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Aplicamos todas las configuraciones manuales
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        modelBuilder.ApplyConfiguration(new ContentConfigurations());
        modelBuilder.ApplyConfiguration(new GenreConfigurations());
        modelBuilder.ApplyConfiguration(new ProfileConfigurations());
        modelBuilder.ApplyConfiguration(new ReviewConfigurations());
        modelBuilder.ApplyConfiguration(new FavoriteConfigurations());
    }
}