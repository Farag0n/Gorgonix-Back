using Gorgonix_Back.Domain.Entities;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetProfilesByUserNameAsync(string userName);
    Task<IEnumerable<Profile>> GetProfilesByUserIdAsync(Guid userId);
    Task<Profile?> GetProfileByIdAsync(Guid profileId);
    Task<Profile?> GetProfileByNameAsync(string profileName);
    Task<Profile?> GetFavoritesByProfileIdAsync(Guid profileId);
    
    Task<Profile?> CreateProfileAsync(Profile profile);
    Task<Profile?> UpdateProfileAsync(Profile profile, Guid profileId);
    Task<bool> DeleteProfileAsync(Guid profileId);
    
}