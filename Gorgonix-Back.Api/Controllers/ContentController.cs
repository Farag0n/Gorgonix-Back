using System.Security.Claims;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService;
    }

    // ================= PUBLIC / USER ROUTES =================
    // Requieren saber qué Perfil está navegando para mostrar sus favoritos
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var profileId = GetProfileIdFromHeader(); // <--- IMPORTANTE
            var contents = await _contentService.GetAllContentsAsync(profileId);
            return Ok(contents);
        }
        catch (ArgumentException ex) // Si no envían el header
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var profileId = GetProfileIdFromHeader();
            var content = await _contentService.GetContentByIdAsync(id, profileId);
            if (content == null) return NotFound();
            return Ok(content);
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
            var profileId = GetProfileIdFromHeader();
            var contents = await _contentService.GetContentsByNameAsync(name, profileId);
            return Ok(contents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("search/genre/{genreId:guid}")]
    public async Task<IActionResult> GetByGenre(Guid genreId)
    {
        try
        {
            var profileId = GetProfileIdFromHeader();
            var contents = await _contentService.GetContentsByGenreAsync(genreId, profileId);
            return Ok(contents);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // ================= ADMIN ROUTES =================

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromForm] ContentCreateDto createDto)
    {
        try
        {
            var content = await _contentService.CreateContentAsync(createDto);
            return Ok(content);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ContentUpdateDto updateDto)
    {
        try
        {
            var updated = await _contentService.UpdateContentAsync(id, updateDto);
            if (updated == null) return NotFound();
            return Ok(updated);
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
            await _contentService.DeleteContentAsync(id);
            return Ok(new { Message = "Contenido eliminado" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // --- Helper para obtener el Perfil actual ---
    private Guid GetProfileIdFromHeader()
    {
        // El frontend debe enviar un header "X-Profile-Id" cuando el usuario selecciona un perfil
        if (Request.Headers.TryGetValue("X-Profile-Id", out var profileIdString) && 
            Guid.TryParse(profileIdString, out var profileId))
        {
            return profileId;
        }
        
        // Si no hay header, lanzamos excepción o retornamos Empty (según tu regla de negocio)
        // Para Netflix, es obligatorio tener un perfil activo para ver contenido
        throw new ArgumentException("Cabecera 'X-Profile-Id' requerida");
    }
}