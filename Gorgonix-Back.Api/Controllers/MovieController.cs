using System.Security.Claims;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = GetCurrentUserId();
            var movies = await _movieService.GetAllMoviesAsync(userId);
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpGet("search/name/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest(new { Message = "El nombre de búsqueda es requerido" });

            var userId = GetCurrentUserId();
            var movies = await _movieService.GetMoviesByNameAsync(name, userId);
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpGet("search/genre/{genre}")]
    public async Task<IActionResult> GetByGenre(string genre)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(genre))
                return BadRequest(new { Message = "El género de búsqueda es requerido" });

            var userId = GetCurrentUserId();
            var movies = await _movieService.GetMoviesByGenreAsync(genre, userId);
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpGet("favorites")]
    public async Task<IActionResult> GetMyFavorites()
    {
        try
        {
            var userId = GetCurrentUserId();
            var movies = await _movieService.GetFavoritesAsync(userId);
            return Ok(movies);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpPost("{id}/favorite")]
    public async Task<IActionResult> ToggleFavorite(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            await _movieService.ToggleFavoriteAsync(userId, id);
            return Ok(new { Message = "Lista de favoritos actualizada" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] MovieCreateDto createDto)
    {
        try
        {
            var movie = await _movieService.CreateMovieAsync(createDto);
            return Ok(movie);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] MovieUpdateDto updateDto)
    {
        try
        {
            var updatedMovie = await _movieService.UpdateMovieAsync(id, updateDto);
            
            if (updatedMovie == null)
                return NotFound(new { Message = "Película no encontrada" });

            return Ok(updatedMovie);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _movieService.DeleteMovieAsync(id);
            return Ok(new { Message = "Película eliminada" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    private Guid GetCurrentUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
    }
}