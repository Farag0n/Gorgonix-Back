using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Application.Services;

public class ContentService : IContentService
{
    private readonly IContentRepository _contentRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IMediaService _mediaService; // Cloudinary
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ContentService> _logger;

    public ContentService(
        IContentRepository contentRepository,
        IGenreRepository genreRepository,
        IMediaService mediaService,
        IProfileRepository profileRepository,
        ILogger<ContentService> logger)
    {
        _contentRepository = contentRepository;
        _genreRepository = genreRepository;
        _mediaService = mediaService;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<ContentResponseDto> CreateContentAsync(ContentCreateDto dto)
    {
        var genre = await _genreRepository.GetGenreByIdAsync(dto.GenreId);
        if (genre == null) throw new KeyNotFoundException("Género no encontrado");

        var posterResult = await _mediaService.AddPhotoAsync(dto.PosterFile);
        var videoResult = await _mediaService.AddVideoAsync(dto.VideoFile);

        var content = new Content(
            dto.Title,
            dto.Description,
            dto.GenreId,
            posterResult.Url,
            posterResult.PublicId,
            videoResult.Url,
            videoResult.PublicId
        );

        await _contentRepository.AddAsync(content);
        return MapToDto(content, false, genre.Name);
    }

    public async Task<ContentResponseDto?> UpdateContentAsync(Guid id, ContentUpdateDto dto)
    {
        var content = await _contentRepository.GetContentByIdAsync(id);
        if (content == null) return null;

        // Validar cambio de género
        if (dto.GenreId.HasValue && dto.GenreId != content.GenreId)
        {
            var genre = await _genreRepository.GetGenreByIdAsync(dto.GenreId.Value);
            if (genre == null) throw new KeyNotFoundException("Género no encontrado");
        }

        content.Update(
            dto.Title ?? content.Title,
            dto.Description ?? content.Description,
            dto.GenreId ?? content.GenreId,
            content.PosterUrl, // No actualizamos archivos en este endpoint simple
            content.VideoUrl
        );

        await _contentRepository.UpdateAsync(content);
        
        var genreName = content.Genre?.Name ?? (await _genreRepository.GetGenreByIdAsync(content.GenreId))?.Name;
        return MapToDto(content, false, genreName!);
    }

    public async Task DeleteContentAsync(Guid id)
    {
        var content = await _contentRepository.GetContentByIdAsync(id);
        if (content == null) throw new KeyNotFoundException("Contenido no encontrado");

        // Borrar archivos de Cloudinary
        if (!string.IsNullOrEmpty(content.PosterPublicId))
            await _mediaService.DeletePhotoAsync(content.PosterPublicId);

        if (!string.IsNullOrEmpty(content.VideoPublicId))
            await _mediaService.DeleteVideoAsync(content.VideoPublicId);

        await _contentRepository.DeleteAsync(content);
    }

    public async Task<IEnumerable<ContentResponseDto>> GetAllContentsAsync(Guid currentProfileId)
    {
        var contents = await _contentRepository.GetAllContentsAsync();
        return await ProcessContentsWithFavorites(contents, currentProfileId);
    }

    public async Task<IEnumerable<ContentResponseDto>> GetContentsByNameAsync(string name, Guid currentProfileId)
    {
        var contents = await _contentRepository.SearchContentsAsync(name);
        return await ProcessContentsWithFavorites(contents, currentProfileId);
    }

    public async Task<IEnumerable<ContentResponseDto>> GetContentsByGenreAsync(Guid genreId, Guid currentProfileId)
    {
        var contents = await _contentRepository.GetContentsByGenreAsync(genreId);
        return await ProcessContentsWithFavorites(contents, currentProfileId);
    }

    public async Task<ContentResponseDto?> GetContentByIdAsync(Guid id, Guid currentProfileId)
    {
        var content = await _contentRepository.GetContentByIdAsync(id);
        if (content == null) return null;

        var isFav = await _profileRepository.IsContentFavoriteAsync(currentProfileId, content.Id);
        return MapToDto(content, isFav, content.Genre?.Name ?? "Desconocido");
    }

    private async Task<IEnumerable<ContentResponseDto>> ProcessContentsWithFavorites(IEnumerable<Content> contents, Guid profileId)
    {
        var dtos = new List<ContentResponseDto>();
        foreach (var c in contents)
        {
            bool isFav = await _profileRepository.IsContentFavoriteAsync(profileId, c.Id);
            dtos.Add(MapToDto(c, isFav, c.Genre?.Name ?? "Desconocido"));
        }
        return dtos;
    }

    private ContentResponseDto MapToDto(Content content, bool isFavorite, string genreName)
    {
        return new ContentResponseDto
        {
            Id = content.Id,
            Title = content.Title,
            Description = content.Description,
            GenreId = content.GenreId,
            GenreName = genreName,
            PosterUrl = content.PosterUrl,
            VideoUrl = content.VideoUrl,
            IsFavorite = isFavorite
        };
    }
}