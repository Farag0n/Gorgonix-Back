namespace Gorgonix_Back.Domain.Entities;

public class Profile
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? PictureUrl { get; private set; } // Private set
    
    public Guid UserId { get; private set; }
    public User User { get; private set; }

    // Relaciones
    private readonly List<Favorite> _favorites = new();
    public IReadOnlyCollection<Favorite> Favorites => _favorites.AsReadOnly();
    
    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
    
    protected Profile() { }

    public Profile(string name, string? pictureUrl, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre del perfil es requerido.");
        if (userId == Guid.Empty) throw new ArgumentException("Usuario inválido.");
        
        Id = Guid.NewGuid();
        Name = name;
        PictureUrl = pictureUrl;   
        UserId = userId; 
    }

    public void UpdateProfile(string name, string? pictureUrl)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        if (pictureUrl != null) PictureUrl = pictureUrl;
    }
}