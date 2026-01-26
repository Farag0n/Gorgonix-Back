using Gorgonix_Back.Application.DTOs;

namespace Gorgonix_Back.Application.Interfaces;

public interface IMovieService
{
    Task<MovieResponseDto> CreateMovieAsync(MovieCreateDto dto);
    Task<MovieResponseDto?> UpdateMovieAsync(Guid id, MovieUpdateDto dto); // <--- Nuevo
    Task DeleteMovieAsync(Guid id);
    Task<IEnumerable<MovieResponseDto>> GetAllMoviesAsync(Guid currentUserId);
    Task<IEnumerable<MovieResponseDto>> GetMoviesByNameAsync(string name, Guid currentUserId);
    Task<IEnumerable<MovieResponseDto>> GetMoviesByGenreAsync(string genre, Guid currentUserId);
    
    Task<IEnumerable<MovieResponseDto>> GetFavoritesAsync(Guid userId);
    Task ToggleFavoriteAsync(Guid userId, Guid movieId);
}