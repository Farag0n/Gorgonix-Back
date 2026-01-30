using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class ProfileCreateDto
{
    [Required]
    public string Name { get; set; }
    public string? PictureUrl { get; set; } // Opcional al crear
    [Required]
    public Guid UserId { get; set; }
}

public class ProfileResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string? PictureUrl { get; set; }
    public Guid UserId { get; set; }
}

public class ProfileUpdateDto
{
    [Required]
    public string Name { get; set; }
    public string? PictureUrl { get; set; }
}