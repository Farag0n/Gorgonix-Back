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
    public bool IsDeleted { get; set; }
}