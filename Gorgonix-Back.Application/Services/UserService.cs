using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Gorgonix_Back.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Gorgonix_Back.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly TokenService _tokenService;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUserRepository userRepository, 
        TokenService tokenService,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _logger = logger;
    }

    // CREATE (Solo Admin usa esto, es diferente a Register)
    public async Task<UserResponseDto?> CreateUserAsync(RegisterDto registerDto)
    {
        try
        {
             var emailVo = new Email(registerDto.Email);
            if (await _userRepository.GetUserByEmailAsync(emailVo) != null)
                throw new InvalidOperationException("El email ya existe");

            if (await _userRepository.GetUserByUserNameAsync(registerDto.UserName) != null)
                throw new InvalidOperationException("El usuario ya existe");

            var user = new User(
                registerDto.Name,
                registerDto.LastName,
                registerDto.Email, 
                registerDto.UserName,
                HashPassword(registerDto.Password),
                registerDto.Role, // Aquí se respeta el rol que manda el admin
                false
            );

            await _userRepository.CreateUserAsync(user);
            return MapToUserResponseDto(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando usuario desde Admin");
            throw;
        }
    }

    public async Task<(string AccessToken, string RefreshToken)> AuthenticateAsync(LoginDto loginDto)
    {
        try
        {
            var emailVo = new Email(loginDto.Email);
            var user = await _userRepository.GetUserByEmailAsync(emailVo);

            if (user == null || user.PasswordHash != HashPassword(loginDto.Password))
                throw new InvalidCredentialException("Credenciales inválidas");

            return await GenerateAuthTokensAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en autenticación");
            throw;
        }
    }

    public async Task<(string AccessToken, string RefreshToken)> RegisterAsync(RegisterDto registerDto)
    {
        try 
        {
            var emailVo = new Email(registerDto.Email);
            if (await _userRepository.GetUserByEmailAsync(emailVo) != null)
                throw new InvalidOperationException("El email ya existe");

            if (await _userRepository.GetUserByUserNameAsync(registerDto.UserName) != null)
                throw new InvalidOperationException("El usuario ya existe");

            var user = new User(
                registerDto.Name,
                registerDto.LastName,
                registerDto.Email, 
                registerDto.UserName,
                HashPassword(registerDto.Password),
                registerDto.Role,
                false
            );

            await _userRepository.CreateUserAsync(user);
            return await GenerateAuthTokensAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registro");
            throw;
        }
    }

    public async Task<(string NewAccessToken, string NewRefreshToken)> RefreshTokenAsync(string accessToken, string refreshToken)
    {
        try
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim)) throw new SecurityTokenException("Token inválido");
            
            User? user;
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                user = await _userRepository.GetUserByIdAsync(userId);
            }
            else
            {
                user = null;
            }
            
            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiresDate <= DateTime.UtcNow)
                throw new SecurityTokenException("Token inválido o expirado");

            return await GenerateAuthTokensAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refrescando token");
            throw;
        }
    }

    // --- Helper para no repetir código de tokens ---
    private async Task<(string, string)> GenerateAuthTokensAsync(User user)
    {
        var accessToken = _tokenService.GenerateAccessToken(user.Id, user.Email.Value, user.UserRole.ToString());
        var refreshToken = _tokenService.GenerateRefreshToken();
        var days = _tokenService.GetRefreshTokenExpirationDays();

        user.UpdateRefreshToken(refreshToken, DateTime.UtcNow.AddDays(days));
        await _userRepository.UpdateUserAsync(user, user.Id);
        
        return (accessToken, refreshToken);
    }

    // --- MÉTODOS CRUD COMPLETADOS ---
    
    public async Task<UserResponseDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user == null ? null : MapToUserResponseDto(user);
    }

    public async Task<UserResponseDto?> GetUserByEmailAsync(string emailStr)
    {
        try 
        {
            var emailVo = new Email(emailStr);
            var user = await _userRepository.GetUserByEmailAsync(emailVo);
            return user == null ? null : MapToUserResponseDto(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error buscando por email {Email}", emailStr);
            throw;
        }
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToUserResponseDto);
    }

    public async Task<UserResponseDto?> UpdateUserAsync(Guid userId, UserUpdateDto userUpdateDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(userId);
        if (existingUser == null) return null;

        // Validaciones de unicidad si cambia email o username
        if (existingUser.Email.Value != userUpdateDto.Email)
        {
             var emailVo = new Email(userUpdateDto.Email);
             if (await _userRepository.GetUserByEmailAsync(emailVo) != null)
                throw new InvalidOperationException("El email ya está en uso");
             existingUser.ChangeEmail(userUpdateDto.Email);
        }
        
        if (existingUser.UserName != userUpdateDto.UserName)
        {
            if (await _userRepository.GetUserByUserNameAsync(userUpdateDto.UserName) != null)
                throw new InvalidOperationException("Username ya en uso");
        }

        existingUser.UpdateUser(userUpdateDto.Name, userUpdateDto.LastName, userUpdateDto.UserName);

        if (!string.IsNullOrEmpty(userUpdateDto.Password))
        {
            existingUser.ChangePassword(HashPassword(userUpdateDto.Password));
        }

        await _userRepository.UpdateUserAsync(existingUser, userId);
        return MapToUserResponseDto(existingUser);
    }

    public async Task<UserResponseDto?> GetUserByUserNameAsync(string userName)
    {
        var user = await _userRepository.GetUserByUserNameAsync(userName);
        return user == null ? null : MapToUserResponseDto(user);
    }

    public async Task<IEnumerable<UserResponseDto>> GetDeletedUsersAsync()
    {
        var users = await _userRepository.GetDeletedUsersAsync();
        return users.Select(MapToUserResponseDto);
    }

    public async Task<UserResponseDto?> SoftDeleteUserAsync(Guid id)
    {
        var user = await _userRepository.SoftDeleteUserAsync(id);
        return user == null ? null : MapToUserResponseDto(user);
    }

    public async Task<UserResponseDto?> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.DeleteUserAsync(id);
        return user == null ? null : MapToUserResponseDto(user);
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
    
    private UserResponseDto MapToUserResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email.Value,
            UserName = user.UserName,
            Role = user.UserRole,
            Profiles = user.Profiles?.Select(p => new ProfileResponseDto 
            {
                Id = p.Id,
                Name = p.Name,
                PictureUrl = p.PictureUrl,
                UserId = p.UserId
            }).ToList() ?? new List<ProfileResponseDto>()
        };
    }
}