using System.ComponentModel.DataAnnotations;

namespace Gorgonix_Back.Application.DTOs;

public class UserUpdateDto
{
    public string Name { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}