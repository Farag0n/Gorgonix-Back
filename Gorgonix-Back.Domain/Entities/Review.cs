namespace Gorgonix_Back.Domain.Entities;

public class Review
{
    public Guid Id { get; private set; }
    public Guid ContentId { get; private set; }
    public Content Content { get; private set; }
    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; }
    public DateTime PublishDate { get; private set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    
    protected Review (){}

    public Review(Guid contentId, Guid profileId, string title, string body)
    {
        Id = Guid.NewGuid();
        ContentId = contentId;
        ProfileId = profileId;
        Title = title;
        Body = body;
        PublishDate = DateTime.UtcNow;
    }

    public void UpdateReview(string title, string body)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("El titulo no puede estar vacio o ser null.");
        if (string.IsNullOrWhiteSpace(body)) throw new ArgumentException("El cuerpo no puede estar vacio o ser null");
        Title = title;
        Body = body;
    }
    
    
}