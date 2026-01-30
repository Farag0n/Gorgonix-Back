using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IReviewService
{
    // Crear
    Task<ReviewResponseDto> AddReviewAsync(ReviewCreateDto dto);
    
    // Leer
    Task<IEnumerable<ReviewResponseDto>> GetReviewsByContentAsync(Guid contentId);
    Task<IEnumerable<ReviewResponseDto>> GetReviewsByProfileAsync(Guid profileId); // <--- Faltaba
    Task<ReviewResponseDto?> GetReviewByIdAsync(Guid reviewId); // <--- Faltaba
    
    // Actualizar
    Task<ReviewResponseDto?> UpdateReviewAsync(Guid reviewId, ReviewUpdateDto dto); // <--- Faltaba
    
    // Borrar
    Task DeleteReviewAsync(Guid reviewId); // <--- Faltaba
}