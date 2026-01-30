using System.Security.Claims;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gorgonix_Back.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Por defecto, todo requiere estar logueado
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }
    
    // GET: api/User
    [HttpGet]
    [Authorize(Roles = "Admin")] // Solo Admin ve todos
    public async Task<IActionResult> GetAll()
    {
        try 
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo usuarios");
            return StatusCode(500, "Error interno del servidor");
        }
    }
    
    // GET: api/User/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        // Seguridad: Solo el Admin o el dueño de la cuenta pueden ver sus detalles completos
        var currentUserId = GetCurrentUserId();
        if (!User.IsInRole("Admin") && currentUserId != id)
        {
            return Forbid();
        }
        
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound(new { Message = "Usuario no encontrado" });
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo usuario {Id}", id);
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // GET: api/User/username/{username}
    [HttpGet("username/{username}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByUserName(string username)
    {
        try
        {
            var user = await _userService.GetUserByUserNameAsync(username);
            
            // Tu servicio devuelve null si no encuentra, así que validamos
            if (user == null) return NotFound(new { Message = "Usuario no encontrado" });
            
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // GET: api/User/email/{email}
    [HttpGet("email/{email}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null) return NotFound(new { Message = "Usuario no encontrado" });
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    // GET: api/User/deleted
    [HttpGet("deleted")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetDeletedUsers()
    {
        try
        {
            var deletedUsers = await _userService.GetDeletedUsersAsync();
            return Ok(deletedUsers);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // POST: api/User (Creación manual por Admin)
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try
        {
            var createdUser = await _userService.CreateUserAsync(registerDto);
            
            if (createdUser == null) 
                return BadRequest(new { Message = "No se pudo crear el usuario" });
            
            return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
        }
        catch (InvalidOperationException ex) // Captura duplicados (Email/Username)
        {
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando usuario");
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // PUT: api/User/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto userUpdateDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        // Seguridad: Solo Admin o el propio usuario
        var currentUserId = GetCurrentUserId();
        if (!User.IsInRole("Admin") && currentUserId != id)
        {
            return Forbid();
        }

        try
        {
            var updatedUser = await _userService.UpdateUserAsync(id, userUpdateDto);
            
            if (updatedUser == null) 
                return NotFound(new { Message = "Usuario no encontrado" });

            return Ok(updatedUser);
        }
        catch (InvalidOperationException ex) // Captura si intenta actualizar a un email/username que ya existe
        {
            return Conflict(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando usuario {Id}", id);
            return StatusCode(500, new { Message = "Error interno al actualizar el usuario" });
        }
    }
    
    // DELETE: api/User/{id} (Hard Delete)
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")] // Solo admin borra físicamente
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var deletedUser = await _userService.DeleteUserAsync(id);
            if (deletedUser == null) return NotFound(new { Message = "Usuario no encontrado" });

            return Ok(new { Message = "Usuario eliminado permanentemente", User = deletedUser });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando usuario {Id}", id);
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // DELETE: api/User/soft/{id} (Soft Delete)
    [HttpDelete("soft/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SoftDelete(Guid id)
    {
        try
        {
            var deletedUser = await _userService.SoftDeleteUserAsync(id);
            if (deletedUser == null) return NotFound(new { Message = "Usuario no encontrado o ya eliminado" });

            return Ok(new { Message = "Usuario desactivado correctamente", User = deletedUser });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando usuario {Id}", id);
            return BadRequest(new { Message = ex.Message });
        }
    }
    
    // Helper para extraer ID del token
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (Guid.TryParse(userIdClaim, out Guid userId))
        {
            return userId;
        }
        
        return Guid.Empty;
    }
}