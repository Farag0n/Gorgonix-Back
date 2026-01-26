namespace Gorgonix_Back.Domain.Entities;

public class Profile
{
    public Guid Id { get; private set; }
    public string Name { get; private  set; }
    public string PictureUrl { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }

    public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
    
    protected  Profile() { }

    public Profile(string name, string pictureUrl, Guid userId)
    {
        
    }
}