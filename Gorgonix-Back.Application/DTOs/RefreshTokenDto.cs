using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class RefreshTokenDto
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}