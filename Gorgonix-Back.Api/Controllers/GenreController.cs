using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GenreController : ControllerBase
{
    private readonly IGenreService _genreService;

    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    // GET: api/genre
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _genreService.GetAllGenresAsync();
        return Ok(genres);
    }

    // POST: api/genre (Solo Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] GenreCreateDto dto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return BadRequest(new { Message = "El nombre es requerido" });

            var genre = await _genreService.CreateGenreAsync(dto);
            return Ok(genre);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}