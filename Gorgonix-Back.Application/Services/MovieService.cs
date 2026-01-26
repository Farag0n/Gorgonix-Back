using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Gorgonix_Back.Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IPhotoService _photoService;
    private readonly ILogger<MovieService> _logger;

    public MovieService(
        IMovieRepository movieRepository, 
        IPhotoService photoService,
        ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _photoService = photoService;
        _logger = logger;
    }

    public async Task<MovieResponseDto> CreateMovieAsync(MovieCreateDto dto)
    {
        try
        {
            _logger.LogInformation("Creando película: {Title}", dto.Title);
            
            var posterResult = await _photoService.AddPhotoAsync(dto.PosterFile);
            
            var videoResult = await _photoService.AddVideoAsync(dto.VideoFile); 
            
            var movie = new Movie(
                dto.Title, 
                dto.Description, 
                dto.Genre, 
                posterResult.Url, 
                posterResult.PublicId, 
                videoResult.Url, 
                videoResult.PublicId
            );

            await _movieRepository.AddAsync(movie);

            return MapToDto(movie, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear película");
            throw;
        }
    }

    public async Task<MovieResponseDto?> UpdateMovieAsync(Guid id, MovieUpdateDto dto)
    {
        try
        {
            _logger.LogInformation("Actualizando película ID: {Id}", id);
            
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) return null;
            
            movie.Update(dto.Title, dto.Description, dto.Genre);
            
            await _movieRepository.UpdateAsync(movie);
            
            return MapToDto(movie, false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar película {Id}", id);
            throw;
        }
    }

    public async Task DeleteMovieAsync(Guid id)
    {
        try
        {
            _logger.LogInformation("Eliminando película ID: {Id}", id);
            
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null) throw new KeyNotFoundException("Película no encontrada");
            
            if (!string.IsNullOrEmpty(movie.PosterPublicId))
                await _photoService.DeletePhotoAsync(movie.PosterPublicId);
                
            if (!string.IsNullOrEmpty(movie.VideoPublicId))
                await _photoService.DeleteVideoAsync(movie.VideoPublicId);
            
            await _movieRepository.DeleteAsync(movie);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar película {Id}", id);
            throw;
        }
    }

    public async Task<IEnumerable<MovieResponseDto>> GetAllMoviesAsync(Guid currentUserId)
    {
        try
        {
            _logger.LogInformation("Obteniendo todas las películas para usuario: {UserId}", currentUserId);
            
            var movies = await _movieRepository.GetAllAsync();
            return await ProcessMoviesWithFavorites(movies, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las películas");
            throw;
        }
    }

    public async Task<IEnumerable<MovieResponseDto>> GetMoviesByNameAsync(string name, Guid currentUserId)
    {
        try
        {
            _logger.LogInformation("Buscando películas por nombre: {Name} para usuario: {UserId}", name, currentUserId);
            
            var movies = await _movieRepository.SearchAsync(name, null);
            return await ProcessMoviesWithFavorites(movies, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar películas por nombre {Name}", name);
            throw;
        }
    }

    public async Task<IEnumerable<MovieResponseDto>> GetMoviesByGenreAsync(string genre, Guid currentUserId)
    {
        try
        {
            _logger.LogInformation("Buscando películas por género: {Genre} para usuario: {UserId}", genre, currentUserId);
            
            var movies = await _movieRepository.SearchAsync(null, genre);
            return await ProcessMoviesWithFavorites(movies, currentUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar películas por género {Genre}", genre);
            throw;
        }
    }

    public async Task<IEnumerable<MovieResponseDto>> GetFavoritesAsync(Guid userId)
    {
        try
        {
            _logger.LogInformation("Obteniendo favoritos para usuario: {UserId}", userId);
            
            var movies = await _movieRepository.GetFavoritesByUserIdAsync(userId);
            return movies.Select(m => MapToDto(m, true));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener favoritos para usuario {UserId}", userId);
            throw;
        }
    }

    public async Task ToggleFavoriteAsync(Guid userId, Guid movieId)
    {
        try
        {
            _logger.LogInformation("Alternando favorito para usuario: {UserId}, película: {MovieId}", userId, movieId);
            
            var isFav = await _movieRepository.IsFavoriteAsync(userId, movieId);
            
            if (isFav)
            {
                await _movieRepository.RemoveFavoriteAsync(userId, movieId);
            }
            else
            {
                await _movieRepository.AddFavoriteAsync(new UserFavorite 
                { 
                    UserId = userId, 
                    MovieId = movieId 
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al alternar favorito para usuario {UserId}, película {MovieId}", userId, movieId);
            throw;
        }
    }

    private async Task<IEnumerable<MovieResponseDto>> ProcessMoviesWithFavorites(IEnumerable<Movie> movies, Guid userId)
    {
        try
        {
            var movieDtos = new List<MovieResponseDto>();
            foreach (var movie in movies)
            {
                bool isFav = await _movieRepository.IsFavoriteAsync(userId, movie.Id);
                movieDtos.Add(MapToDto(movie, isFav));
            }
            return movieDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar películas con favoritos para usuario {UserId}", userId);
            throw;
        }
    }

    private MovieResponseDto MapToDto(Movie movie, bool isFavorite)
    {
        return new MovieResponseDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            PosterUrl = movie.PosterUrl,
            VideoUrl = movie.VideoUrl,
            IsFavorite = isFavorite
        };
    }
}