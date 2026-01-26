using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IUserService
{
    Task<UserResponseDto?> GetUserByIdAsync(Guid id);
    Task<UserResponseDto?> GetUserByEmailAsync(string emailStr);
    Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto?> UpdateUserAsync(Guid userId, UserUpdateDto updateUserDto);
    Task<UserResponseDto?> GetUserByUserNameAsync(string userName);
    Task<IEnumerable<UserResponseDto>> GetDeletedUsersAsync();
    Task<UserResponseDto?> SoftDeleteUserAsync(Guid id);
    Task<UserResponseDto?> DeleteUserAsync(Guid id);
    Task<UserResponseDto?> CreateUserAsync(RegisterDto registerDto);
    
    //retorna AccessToken y RefreshToken
    Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(LoginDto userLoginDto);
    Task<(string AccessToken, string RefreshToken)> RegisterAsync(RegisterDto userRegisterDto);
    
    //Metodo para refrescar el token
    Task<(string NewAccessToken, string NewRefreshToken)> RefreshTokenAsync(string accessToken, string refreshToken);
}