using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.ValueObjects;

namespace Gorgonix_Back.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUserNameAsync(string userName);
    Task<User?> GetUserByEmailAsync(Email email);
    Task<IEnumerable<User>> GetDeletedUsersAsync();
    
    Task<User?> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(User user, Guid userId);
    Task<User?> SoftDeleteUserAsync(Guid userId);
    Task<User?> DeleteUserAsync(Guid userId);
}