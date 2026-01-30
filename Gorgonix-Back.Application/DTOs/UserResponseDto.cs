using Gorgonix_Back.Domain.Enums;

namespace Gorgonix_Back.Application.DTOs;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public UserRole Role { get; set; }
    
    // Agregamos esto para que el Front sepa qué perfiles tiene el usuario
    public ICollection<ProfileResponseDto> Profiles { get; set; } = new List<ProfileResponseDto>();
}