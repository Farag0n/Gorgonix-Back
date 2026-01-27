using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IGenreRepository
{
    Task<ICollection<Genre>> GetAllGenresAsync();
    Task<Genre?> GetGenreByNameAsync(string genreName);
    Task<Genre?> GetGenreByIdAsync(Guid genreId);
    
    Task<Genre> CreateGenreAsync(Genre genre);
    Task<Genre?> UpdateGenreAsync(Genre genre, Guid genreId);
    Task<bool> DeleteGenreAsync(Guid genreId);
}