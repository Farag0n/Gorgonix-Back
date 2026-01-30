using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Gorgonix_Back.Infrastructure.Repositories;
using Gorgonix_Back.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gorgonix_Back.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string? conn = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                conn,
                ServerVersion.AutoDetect(conn)
            )
        );
        
        // Repositorios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IContentRepository, ContentRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IMediaService, CloudinaryService>();
        
        return services;
    }
}