using System.Security.Claims;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    // 1. Crear Reseña
    [HttpPost]
    public async Task<IActionResult> AddReview([FromBody] ReviewCreateDto dto)
    {
        try
        {
            var review = await _reviewService.AddReviewAsync(dto);
            return Ok(review);
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

    // 2. Ver Reseñas de una Película
    [HttpGet("content/{contentId:guid}")]
    public async Task<IActionResult> GetByContent(Guid contentId)
    {
        var reviews = await _reviewService.GetReviewsByContentAsync(contentId);
        return Ok(reviews);
    }

    // 3. Ver Reseñas de un Perfil (Mis reseñas)
    [HttpGet("profile/{profileId:guid}")]
    public async Task<IActionResult> GetByProfile(Guid profileId)
    {
        // TODO: Validar que el profileId pertenezca al usuario actual para mayor seguridad
        var reviews = await _reviewService.GetReviewsByProfileAsync(profileId);
        return Ok(reviews);
    }

    // 4. Ver una reseña específica
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var review = await _reviewService.GetReviewByIdAsync(id);
        if (review == null) return NotFound(new { Message = "Reseña no encontrada" });
        return Ok(review);
    }

    // 5. Actualizar Reseña
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ReviewUpdateDto dto)
    {
        try
        {
            // TODO: Validar que el usuario sea dueño de la reseña
            var updatedReview = await _reviewService.UpdateReviewAsync(id, dto);
            if (updatedReview == null) return NotFound(new { Message = "Reseña no encontrada" });
            
            return Ok(updatedReview);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // 6. Eliminar Reseña
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            // TODO: Validar que el usuario sea dueño de la reseña o sea Admin
            await _reviewService.DeleteReviewAsync(id);
            return Ok(new { Message = "Reseña eliminada" });
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
}