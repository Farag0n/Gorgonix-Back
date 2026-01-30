using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IContentService
{
    Task<ContentResponseDto> CreateContentAsync(ContentCreateDto dto);
    Task<ContentResponseDto?> UpdateContentAsync(Guid id, ContentUpdateDto dto);
    Task DeleteContentAsync(Guid id);
    
    // El currentProfileId es necesario para saber si es favorito
    Task<IEnumerable<ContentResponseDto>> GetAllContentsAsync(Guid currentProfileId);
    Task<IEnumerable<ContentResponseDto>> GetContentsByNameAsync(string name, Guid currentProfileId);
    Task<IEnumerable<ContentResponseDto>> GetContentsByGenreAsync(Guid genreId, Guid currentProfileId);
    Task<ContentResponseDto?> GetContentByIdAsync(Guid id, Guid currentProfileId);
}