using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Gorgonix_Back.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly AppDbContext _context;

    public MovieRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync() => await _context.Movies.ToListAsync();

    public async Task<IEnumerable<Movie>> SearchAsync(string? name, string? genre)
    {
        var query = _context.Movies.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(m => m.Title.Contains(name));

        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(m => m.Genre.Contains(genre));

        return await query.ToListAsync();
    }

    public async Task<Movie?> GetByIdAsync(Guid id) => await _context.Movies.FindAsync(id);

    public async Task AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Movie movie)
    {
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }

    // Favoritos
    public async Task AddFavoriteAsync(UserFavorite favorite)
    {
        await _context.UserFavorites.AddAsync(favorite);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFavoriteAsync(Guid userId, Guid movieId)
    {
        var favorite = await _context.UserFavorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.MovieId == movieId);
        
        if (favorite != null)
        {
            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Movie>> GetFavoritesByUserIdAsync(Guid userId)
    {
        return await _context.UserFavorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Movie)
            .Select(f => f.Movie)
            .ToListAsync();
    }

    public async Task<bool> IsFavoriteAsync(Guid userId, Guid movieId)
    {
        return await _context.UserFavorites.AnyAsync(f => f.UserId == userId && f.MovieId == movieId);
    }
}