using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IContentRepository
{
    Task<IEnumerable<Content?>> GetAllContentsAsync();
    Task<IEnumerable<Content?>> GetContentsByGenreAsync(Genre genre);
    Task<Content?> GetContentByTitleAsync(string title);
    Task<Content?> GetContentByIdAsync(Guid contentId);
    
    Task<Content> CreateContentAsync(Content content);
    Task<Content?> UpdateContentAsync(Content content, Guid contentId);
    Task<Content?> DeleteContentAsync(Guid contentId);
}