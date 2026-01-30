using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IGenreService
{
    Task<IEnumerable<GenreDto>> GetAllGenresAsync();
    Task<GenreDto> CreateGenreAsync(GenreCreateDto dto);
}