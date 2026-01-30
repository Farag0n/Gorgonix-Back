using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Infrastructure.Repositories;

public class ContentRepository : IContentRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ContentRepository> _logger;

    public ContentRepository(AppDbContext context, ILogger<ContentRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Content>> GetAllContentsAsync()
    {
        try
        {
            return await _context.Movies
                .Include(c => c.Genre)
                .AsNoTracking() // Optimización de lectura
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo todos los contenidos");
            throw;
        }
    }

    public async Task<IEnumerable<Content>> GetContentsByGenreAsync(Guid genreId)
    {
        try
        {
            return await _context.Movies
                .Where(c => c.GenreId == genreId)
                .Include(c => c.Genre)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo contenidos por género {GenreId}", genreId);
            throw;
        }
    }

    public async Task<Content?> GetContentByTitleAsync(string title)
    {
        try
        {
            return await _context.Movies
                .Include(c => c.Genre)
                .FirstOrDefaultAsync(c => c.Title == title);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo contenido por título {Title}", title);
            throw;
        }
    }

    public async Task<IEnumerable<Content>> SearchContentsAsync(string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) return new List<Content>();

            return await _context.Movies
                .Include(c => c.Genre)
                .Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm))
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error buscando contenidos con término {Term}", searchTerm);
            throw;
        }
    }

    public async Task<Content?> GetContentByIdAsync(Guid contentId)
    {
        try
        {
            return await _context.Movies
                .Include(c => c.Genre)
                // Opcional: .Include(c => c.Reviews) si quieres ver reseñas al abrir la peli
                .FirstOrDefaultAsync(c => c.Id == contentId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo contenido por ID {Id}", contentId);
            throw;
        }
    }

    public async Task AddAsync(Content content)
    {
        try
        {
            await _context.Movies.AddAsync(content);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando contenido {Title}", content.Title);
            throw;
        }
    }

    public async Task UpdateAsync(Content content)
    {
        try
        {
            _context.Movies.Update(content);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando contenido ID {Id}", content.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Content content)
    {
        try
        {
            _context.Movies.Remove(content);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando contenido ID {Id}", content.Id);
            throw;
        }
    }
}