namespace Gorgonix_Back.Domain.Entities;

public class Favorite
{
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; }
    public Guid ContentId { get; set; }
    public Content Content { get; set; }
}