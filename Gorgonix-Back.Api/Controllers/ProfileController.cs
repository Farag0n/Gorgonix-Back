using System.Security.Claims;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    // 1. Obtener mis perfiles
    [HttpGet("my-profiles")]
    public async Task<IActionResult> GetMyProfiles()
    {
        var userId = GetCurrentUserId();
        var profiles = await _profileService.GetProfilesByUserAsync(userId);
        return Ok(profiles);
    }

    // 2. Obtener un perfil específico (Validando propiedad)
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var profile = await _profileService.GetProfileByIdAsync(id);
        if (profile == null) return NotFound();

        // Seguridad: Solo dejar ver si es el dueño
        if (profile.UserId != GetCurrentUserId()) return Forbid();

        return Ok(profile);
    }

    // 3. Crear Perfil
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProfileCreateDto dto)
    {
        try
        {
            dto.UserId = GetCurrentUserId(); // Forzar ID del token
            var profile = await _profileService.CreateProfileAsync(dto);
            return Ok(profile);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message }); // Límite de perfiles
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // 4. Ver Favoritos
    [HttpGet("{profileId:guid}/favorites")]
    public async Task<IActionResult> GetFavorites(Guid profileId)
    {
        if (!await IsProfileOwner(profileId)) return Forbid();

        var favorites = await _profileService.GetFavoritesAsync(profileId);
        return Ok(favorites);
    }

    // 5. Agregar/Quitar Favorito
    [HttpPost("{profileId:guid}/toggle-favorite/{contentId:guid}")]
    public async Task<IActionResult> ToggleFavorite(Guid profileId, Guid contentId)
    {
        try
        {
            if (!await IsProfileOwner(profileId)) return Forbid();

            await _profileService.ToggleFavoriteAsync(profileId, contentId);
            return Ok(new { Message = "Favoritos actualizados" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // --- Helpers ---
    private Guid GetCurrentUserId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(idClaim, out var id) ? id : Guid.Empty;
    }

    private async Task<bool> IsProfileOwner(Guid profileId)
    {
        var profile = await _profileService.GetProfileByIdAsync(profileId);
        return profile != null && profile.UserId == GetCurrentUserId();
    }
}