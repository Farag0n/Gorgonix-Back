using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<IEnumerable<Movie>> SearchAsync(string? name, string? genre);
    Task<Movie?> GetByIdAsync(Guid id);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(Movie movie);
    
    // Métodos para favoritos
    Task AddFavoriteAsync(UserFavorite favorite);
    Task RemoveFavoriteAsync(Guid userId, Guid movieId);
    Task<IEnumerable<Movie>> GetFavoritesByUserIdAsync(Guid userId);
    Task<bool> IsFavoriteAsync(Guid userId, Guid movieId);
}