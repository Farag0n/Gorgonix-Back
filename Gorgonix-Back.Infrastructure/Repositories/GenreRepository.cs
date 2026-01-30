using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Infrastructure.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<GenreRepository> _logger;

    public GenreRepository(AppDbContext context, ILogger<GenreRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<ICollection<Genre>> GetAllGenresAsync()
    {
        try
        {
            return await _context.Genres.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo géneros");
            throw;
        }
    }

    public async Task<Genre?> GetGenreByNameAsync(string genreName)
    {
        try
        {
            return await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo género {Name}", genreName);
            throw;
        }
    }

    public async Task<Genre?> GetGenreByIdAsync(Guid genreId)
    {
        try
        {
            return await _context.Genres.FindAsync(genreId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo género ID {Id}", genreId);
            throw;
        }
    }

    public async Task<Genre> CreateGenreAsync(Genre genre)
    {
        try
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync();
            return genre;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando género {Name}", genre.Name);
            throw;
        }
    }

    public async Task<Genre?> UpdateGenreAsync(Genre genre, Guid genreId)
    {
        try
        {
            var existing = await _context.Genres.FindAsync(genreId);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(genre);
            await _context.SaveChangesAsync();
            return existing;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando género ID {Id}", genreId);
            throw;
        }
    }

    public async Task<bool> DeleteGenreAsync(Guid genreId)
    {
        try
        {
            var genre = await _context.Genres.FindAsync(genreId);
            if (genre == null) return false;

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando género ID {Id}", genreId);
            throw;
        }
    }
}