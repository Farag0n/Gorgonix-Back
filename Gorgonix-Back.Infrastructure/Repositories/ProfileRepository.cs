using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Infrastructure.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ProfileRepository> _logger;

    public ProfileRepository(AppDbContext context, ILogger<ProfileRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Profile>> GetProfilesByUserIdAsync(Guid userId)
    {
        try
        {
            return await _context.Profiles
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo perfiles del usuario {UserId}", userId);
            throw;
        }
    }

    public async Task<Profile?> GetProfileByIdAsync(Guid profileId)
    {
        try
        {
            return await _context.Profiles.FindAsync(profileId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo perfil ID {Id}", profileId);
            throw;
        }
    }

    // --- FAVORITOS ---
    public async Task<IEnumerable<Content>> GetFavoriteContentsAsync(Guid profileId)
    {
        try
        {
            // Join implícito a través de la entidad de relación
            return await _context.UserFavorites
                .Where(f => f.ProfileId == profileId)
                .Include(f => f.Content) // Traemos el objeto Content
                .ThenInclude(c => c.Genre) // Y su género
                .Select(f => f.Content) // Proyectamos solo el Content
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo favoritos del perfil {Id}", profileId);
            throw;
        }
    }

    public async Task<bool> IsContentFavoriteAsync(Guid profileId, Guid contentId)
    {
        try
        {
            return await _context.UserFavorites
                .AnyAsync(f => f.ProfileId == profileId && f.ContentId == contentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando favorito");
            throw;
        }
    }

    public async Task AddFavoriteAsync(Favorite favorite)
    {
        try
        {
            await _context.UserFavorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando favorito al perfil {Id}", favorite.ProfileId);
            throw;
        }
    }

    public async Task RemoveFavoriteAsync(Guid profileId, Guid contentId)
    {
        try
        {
            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(f => f.ProfileId == profileId && f.ContentId == contentId);

            if (favorite != null)
            {
                _context.UserFavorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando favorito");
            throw;
        }
    }
    // -----------------

    public async Task AddAsync(Profile profile)
    {
        try
        {
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando perfil {Name}", profile.Name);
            throw;
        }
    }

    public async Task UpdateAsync(Profile profile)
    {
        try
        {
            _context.Profiles.Update(profile);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando perfil {Id}", profile.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Profile profile)
    {
        try
        {
            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando perfil {Id}", profile.Id);
            throw;
        }
    }
}