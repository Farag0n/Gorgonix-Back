using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IReviewRepository
{
    Task<IEnumerable<Review?>> GetAllReviewsByProfile(Guid profileId);
    Task<IEnumerable<Review?>> GetAllReviewsByContent(Guid contentId);
    Task<Content?> GetReviewByIdAsync(Guid reviewId);
    
    Task<Content> CreateReviewAsync(Review review);
    Task<Content?> UpdateReviewAsync(Review review, Guid reviewId);
    Task<Content?> DeleteReviewAsync(Guid reviewId);

}