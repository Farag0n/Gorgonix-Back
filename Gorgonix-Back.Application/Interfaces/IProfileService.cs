using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IProfileService
{
    Task<ProfileResponseDto> CreateProfileAsync(ProfileCreateDto dto);
    Task<IEnumerable<ProfileResponseDto>> GetProfilesByUserAsync(Guid userId);
    Task<ProfileResponseDto?> GetProfileByIdAsync(Guid id);
    
    // Favoritos (Ahora viven aquí)
    Task<IEnumerable<ContentResponseDto>> GetFavoritesAsync(Guid profileId);
    Task ToggleFavoriteAsync(Guid profileId, Guid contentId);
}