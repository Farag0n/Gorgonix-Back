namespace Gorgonix_Back.Domain.Entities;

public class UserFavorite
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid MovieId { get; set; }
    public Movie Movie { get; set; }
}