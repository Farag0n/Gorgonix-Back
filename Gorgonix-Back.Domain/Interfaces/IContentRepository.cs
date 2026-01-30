using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IContentRepository
{
    Task<IEnumerable<Content>> GetAllContentsAsync();
    Task<IEnumerable<Content>> GetContentsByGenreAsync(Guid genreId); // Mejor usar ID
    Task<Content?> GetContentByTitleAsync(string title); // Búsqueda exacta
    Task<IEnumerable<Content>> SearchContentsAsync(string searchTerm); // Búsqueda parcial
    Task<Content?> GetContentByIdAsync(Guid contentId);
    
    Task AddAsync(Content content); // Void o Task es suficiente, el ID ya lo tienes
    Task UpdateAsync(Content content);
    Task DeleteAsync(Content content);
}