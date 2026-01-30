namespace Gorgonix_Back.Application.DTOs;

public class GenreDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class GenreCreateDto
{
    public string Name { get; set; }
}