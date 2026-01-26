using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class MovieCreateDto
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public string Genre { get; set; }
    [Required] public IFormFile PosterFile { get; set; }
    [Required] public IFormFile VideoFile { get; set; }
}

public class MovieUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Genre { get; set; }
}

public class MovieResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Genre { get; set; }
    public string PosterUrl { get; set; }
    public string VideoUrl { get; set; }
    public bool IsFavorite { get; set; }
}