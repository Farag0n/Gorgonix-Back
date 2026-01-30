using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAllReviewsByProfileIdAsync(Guid profileId);
    Task<IEnumerable<Review>> GetAllReviewsByContentIdAsync(Guid contentId);
    Task<Review?> GetReviewByIdAsync(Guid reviewId); // Corregido retorno
    
    Task AddAsync(Review review);
    Task UpdateAsync(Review review);
    Task DeleteAsync(Review review);
}