using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class ContentCreateDto
{
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    [Required] public Guid GenreId { get; set; } // Ahora usamos ID
    [Required] public IFormFile PosterFile { get; set; }
    [Required] public IFormFile VideoFile { get; set; }
}

public class ContentUpdateDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid? GenreId { get; set; }
}

public class ContentResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string GenreName { get; set; } // Devolvemos el nombre, no el ID
    public Guid GenreId { get; set; }
    public string PosterUrl { get; set; }
    public string VideoUrl { get; set; }
    public bool IsFavorite { get; set; } // Dependerá del perfil que consulta
}