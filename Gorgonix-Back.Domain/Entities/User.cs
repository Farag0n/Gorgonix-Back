using Gorgonix_Back.Domain.Enums;
using Gorgonix_Back.Domain.ValueObjects;

namespace Gorgonix_Back.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string LastName { get; private set; } 
    public Email Email { get; private set; }
    public string UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreateAt { get; private set; } = DateTime.UtcNow;
    public UserRole UserRole { get; private set; }
    public bool IsDeleted { get; private set; }
    
    public string? RefreshToken { get; private set; }
    public DateTime? RefreshTokenExpiresDate { get; private set; }
    
    public ICollection<Profile> Profiles { get; private set; } = new List<Profile>();
    
    protected User() {}
    
    public User(string name, string lastName, string email, string userName, string passwordHash, UserRole userRole, bool isDeleted)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre requerido");
        if(string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Apellido requerido");
        if(string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("Usuario requerido");
        if(string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Contraseña requerida");

        Id = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Email = new Email(email); 
        UserName = userName;
        PasswordHash = passwordHash;
        UserRole = userRole;
        CreateAt = DateTime.UtcNow;
        IsDeleted = false; 
    }
    
    public void UpdateUser(string name, string lastName, string userName)
    {
        if(string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre requerido");
        if(string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Apellido requerido");
        if(string.IsNullOrWhiteSpace(userName)) throw new ArgumentException("UserName requerido");
        Name = name;
        LastName = lastName;
        UserName = userName;
    }

    public void ChangePassword(string newPasswordHash)
    {
        if(string.IsNullOrWhiteSpace(newPasswordHash)) throw new ArgumentException("Hash inválido");
        PasswordHash = newPasswordHash;
    }
    
    public void SoftDelete()
    {
        if (IsDeleted) return;
        IsDeleted = true;
    }
    
    public void UpdateRefreshToken(string token, DateTime expires)
    {
        RefreshToken = token;
        RefreshTokenExpiresDate = expires;
    }
    
    public void RevokeRefreshToken()
    {
        RefreshToken = null;
        RefreshTokenExpiresDate = DateTime.UtcNow;
    }
    
    public void ChangeEmail(string newEmail)
    {
        this.Email = new Email(newEmail);
    }
}