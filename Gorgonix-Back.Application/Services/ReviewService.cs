using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IContentRepository _contentRepository;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(
        IReviewRepository reviewRepository, 
        IProfileRepository profileRepository, 
        IContentRepository contentRepository,
        ILogger<ReviewService> logger)
    {
        _reviewRepository = reviewRepository;
        _profileRepository = profileRepository;
        _contentRepository = contentRepository;
        _logger = logger;
    }

    public async Task<ReviewResponseDto> AddReviewAsync(ReviewCreateDto dto)
    {
        var profile = await _profileRepository.GetProfileByIdAsync(dto.ProfileId);
        if (profile == null) throw new KeyNotFoundException("Perfil no encontrado");
        
        var content = await _contentRepository.GetContentByIdAsync(dto.ContentId);
        if (content == null) throw new KeyNotFoundException("Contenido no encontrado");

        var review = new Review(dto.ContentId, dto.ProfileId, dto.Title, dto.Body);
        await _reviewRepository.AddAsync(review);

        return MapToDto(review, profile.Name, profile.PictureUrl);
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByContentAsync(Guid contentId)
    {
        var reviews = await _reviewRepository.GetAllReviewsByContentIdAsync(contentId);
        // Mapeamos
        return reviews.Select(r => MapToDto(r, r.Profile?.Name ?? "Usuario", r.Profile?.PictureUrl));
    }

    public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByProfileAsync(Guid profileId)
    {
        var reviews = await _reviewRepository.GetAllReviewsByProfileIdAsync(profileId);
        return reviews.Select(r => MapToDto(r, r.Profile?.Name ?? "Yo", r.Profile?.PictureUrl));
    }

    public async Task<ReviewResponseDto?> GetReviewByIdAsync(Guid reviewId)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
        // Nota: Si el repo no trae el Profile incluido, el nombre podría salir null
        // Asegúrate que tu repositorio use .Include(r => r.Profile)
        return review == null ? null : MapToDto(review, review.Profile?.Name ?? "Desconocido", review.Profile?.PictureUrl);
    }

    public async Task<ReviewResponseDto?> UpdateReviewAsync(Guid reviewId, ReviewUpdateDto dto)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
        if (review == null) return null;

        // Aquí usamos el método de dominio que creaste
        review.UpdateReview(dto.Title, dto.Body);
        
        await _reviewRepository.UpdateAsync(review);
        
        return MapToDto(review, review.Profile?.Name ?? "Usuario", review.Profile?.PictureUrl);
    }

    public async Task DeleteReviewAsync(Guid reviewId)
    {
        var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
        if (review == null) throw new KeyNotFoundException("Reseña no encontrada");

        await _reviewRepository.DeleteAsync(review);
    }

    // Helper para mapear y no repetir código
    private ReviewResponseDto MapToDto(Review review, string profileName, string? profilePic)
    {
        return new ReviewResponseDto
        {
            Id = review.Id,
            Title = review.Title,
            Body = review.Body,
            PublishDate = review.PublishDate,
            ProfileName = profileName,
            ProfilePictureUrl = profilePic
        };
    }
}