using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;

namespace Gorgonix_Back.Application.Services;

public class ProfileService : IProfileService
{
    private readonly IProfileRepository _profileRepository;
    private readonly IContentRepository _contentRepository;

    public ProfileService(IProfileRepository profileRepository, IContentRepository contentRepository)
    {
        _profileRepository = profileRepository;
        _contentRepository = contentRepository;
    }

    public async Task<ProfileResponseDto> CreateProfileAsync(ProfileCreateDto dto)
    {
        // Opcional: Validar que el usuario no tenga más de X perfiles (ej: 4)
        var profiles = await _profileRepository.GetProfilesByUserIdAsync(dto.UserId);
        if (profiles.Count() >= 4)
            throw new InvalidOperationException("Límite de perfiles alcanzado");

        var profile = new Profile(dto.Name, dto.PictureUrl, dto.UserId);
        await _profileRepository.AddAsync(profile);
        return MapToDto(profile);
    }

    public async Task<IEnumerable<ProfileResponseDto>> GetProfilesByUserAsync(Guid userId)
    {
        var profiles = await _profileRepository.GetProfilesByUserIdAsync(userId);
        return profiles.Select(MapToDto);
    }

    public async Task<ProfileResponseDto?> GetProfileByIdAsync(Guid id)
    {
        var profile = await _profileRepository.GetProfileByIdAsync(id);
        return profile == null ? null : MapToDto(profile);
    }

    // --- FAVORITOS ---

    public async Task<IEnumerable<ContentResponseDto>> GetFavoritesAsync(Guid profileId)
    {
        var contents = await _profileRepository.GetFavoriteContentsAsync(profileId);
        
        // Mapeamos manualmente. Como son favoritos, IsFavorite es true siempre.
        return contents.Select(c => new ContentResponseDto
        {
            Id = c.Id,
            Title = c.Title,
            Description = c.Description,
            GenreId = c.GenreId,
            GenreName = c.Genre?.Name ?? "Desconocido",
            PosterUrl = c.PosterUrl,
            VideoUrl = c.VideoUrl,
            IsFavorite = true 
        });
    }

    public async Task ToggleFavoriteAsync(Guid profileId, Guid contentId)
    {
        var isFav = await _profileRepository.IsContentFavoriteAsync(profileId, contentId);
        
        if (isFav)
        {
            await _profileRepository.RemoveFavoriteAsync(profileId, contentId);
        }
        else
        {
            // Validamos que la película exista antes de agregarla
            var content = await _contentRepository.GetContentByIdAsync(contentId);
            if (content == null) throw new KeyNotFoundException("Contenido no encontrado");

            var favorite = new Favorite { ProfileId = profileId, ContentId = contentId };
            await _profileRepository.AddFavoriteAsync(favorite);
        }
    }

    private ProfileResponseDto MapToDto(Profile profile)
    {
        return new ProfileResponseDto
        {
            Id = profile.Id,
            Name = profile.Name,
            PictureUrl = profile.PictureUrl,
            UserId = profile.UserId
        };
    }
}