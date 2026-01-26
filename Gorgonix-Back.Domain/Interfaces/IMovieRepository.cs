using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IMovieRepository
{
    Task<IEnumerable<Content>> GetAllAsync();
    Task<IEnumerable<Content>> SearchAsync(string? name, string? genre);
    Task<Content?> GetByIdAsync(Guid id);
    Task AddAsync(Content content);
    Task UpdateAsync(Content content);
    Task DeleteAsync(Content content);
    
    // Métodos para favoritos
    Task AddFavoriteAsync(Favorite favorite);
    Task RemoveFavoriteAsync(Guid userId, Guid movieId);
    Task<IEnumerable<Content>> GetFavoritesByUserIdAsync(Guid userId);
    Task<bool> IsFavoriteAsync(Guid userId, Guid movieId);
}