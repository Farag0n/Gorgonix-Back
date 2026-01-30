namespace Gorgonix_Back.Domain.Entities;

public class Review
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public DateTime PublishDate { get; private set; }

    // Relaciones
    public Guid ContentId { get; private set; }
    public Content Content { get; private set; } // Navegación
    
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } // Navegación
    
    protected Review (){}

    public Review(Guid contentId, Guid profileId, string title, string body)
    {
        if (contentId == Guid.Empty) throw new ArgumentException("ContentId requerido");
        if (profileId == Guid.Empty) throw new ArgumentException("ProfileId requerido");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título requerido");

        Id = Guid.NewGuid();
        ContentId = contentId;
        ProfileId = profileId;
        Title = title;
        Body = body;
        PublishDate = DateTime.UtcNow;
    }

    public void UpdateReview(string title, string body)
    {
        if (!string.IsNullOrWhiteSpace(title)) Title = title;
        if (!string.IsNullOrWhiteSpace(body)) Body = body;
    }
}