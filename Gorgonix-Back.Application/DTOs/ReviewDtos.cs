using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class ReviewCreateDto
{
    [Required] public Guid ContentId { get; set; }
    [Required] public Guid ProfileId { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Body { get; set; }
}

public class ReviewUpdateDto
{
    [Required] public string Title { get; set; }
    [Required] public string Body { get; set; }
}

public class ReviewResponseDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string ProfileName { get; set; } // Para mostrar quién comentó
    public string? ProfilePictureUrl { get; set; }
    public DateTime PublishDate { get; set; }
}