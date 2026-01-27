namespace Gorgonix_Back.Domain.Entities;

public class Profile
{
    public Guid Id { get; private set; }
    public string Name { get; private  set; }
    public string? PictureUrl { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    
    protected  Profile() { }

    public Profile(string name, string? pictureUrl, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre del perfil no puede estar vacio o ser null.");
        if (userId == Guid.Empty) throw new ArgumentException("El perfil debe pertener a un usuario.");
        
        Id = Guid.NewGuid();
        Name = name;
        PictureUrl = pictureUrl;    
    }

    public void UpdateProfile(string name, string? pictureUrl)
    {
        if  (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El nombre de usuario no puede estar vacio o ser null.");
        Name = name;
        PictureUrl = pictureUrl;
    }

    public void DeleteAllFavorites()
    {
        Favorites.Clear();
    }
}