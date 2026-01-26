using System.ComponentModel.DataAnnotations;
using Gorgonix_Back.Domain.Enums;

namespace Gorgonix_Back.Application.DTOs;

public class RegisterDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string UserName { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public UserRole Role { get; set; }
}