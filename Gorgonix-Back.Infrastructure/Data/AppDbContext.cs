using CloudinaryDotNet.Actions;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Gorgonix_Back.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    public DbSet<Content> Movies { get; set; }
    public DbSet<Favorite> UserFavorites { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfigurations());
        
        //TODO refactorizar esto, dejarlo asi para testear
        
        // Configuración de Movie
        modelBuilder.Entity<Content>().HasKey(m => m.Id);
        
        // Configuración de Favoritos (Many-to-Many)
        modelBuilder.Entity<Favorite>()
            .HasKey(uf => new { uf.UserId, uf.MovieId });

        modelBuilder.Entity<Favorite>()
            .HasOne(uf => uf.User)
            .WithMany()
            .HasForeignKey(uf => uf.UserId);
        //TODO navegacion de user a favoritos
        modelBuilder.Entity<Favorite>()
            .HasOne(uf => uf.Content)
            .WithMany(m => m.FavoritedBy)
            .HasForeignKey(uf => uf.MovieId);
    }
}