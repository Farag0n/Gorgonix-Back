using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Gorgonix_Back.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    
    public DbSet<User> Users { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new UserConfigurations());
    }
}