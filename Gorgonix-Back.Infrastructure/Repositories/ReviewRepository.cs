using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Infrastructure.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(AppDbContext context, ILogger<ReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Review>> GetAllReviewsByProfileIdAsync(Guid profileId)
    {
        try
        {
            return await _context.Reviews
                .Where(r => r.ProfileId == profileId)
                .Include(r => r.Content) // Saber de qué peli habló
                .OrderByDescending(r => r.PublishDate)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo reviews del perfil {Id}", profileId);
            throw;
        }
    }

    public async Task<IEnumerable<Review>> GetAllReviewsByContentIdAsync(Guid contentId)
    {
        try
        {
            return await _context.Reviews
                .Where(r => r.ContentId == contentId)
                .Include(r => r.Profile) // Saber quién comentó
                .OrderByDescending(r => r.PublishDate)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo reviews del contenido {Id}", contentId);
            throw;
        }
    }

    public async Task<Review?> GetReviewByIdAsync(Guid reviewId)
    {
        try
        {
            return await _context.Reviews.FindAsync(reviewId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo review ID {Id}", reviewId);
            throw;
        }
    }

    public async Task AddAsync(Review review)
    {
        try
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error agregando review");
            throw;
        }
    }

    public async Task UpdateAsync(Review review)
    {
        try
        {
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando review {Id}", review.Id);
            throw;
        }
    }

    public async Task DeleteAsync(Review review)
    {
        try
        {
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando review {Id}", review.Id);
            throw;
        }
    }
}