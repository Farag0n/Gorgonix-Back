namespace Gorgonix_Back.Domain.Entities;

public class Movie
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string Genre { get; private set; } // TODO cambiar por enum 
    public string PosterUrl { get; private set; }
    public string PosterPublicId { get; private set; }
    public string VideoUrl { get; private set; }
    public string VideoPublicId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public ICollection<UserFavorite> FavoritedBy { get; private set; } = new List<UserFavorite>();

    protected Movie() { }

    public Movie(string title, string description, string genre, string posterUrl, string posterPublicId, string videoUrl, string videoPublicId)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título requerido");
        if (string.IsNullOrWhiteSpace(videoUrl)) throw new ArgumentException("Video requerido");

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Genre = genre;
        PosterUrl = posterUrl;
        PosterPublicId = posterPublicId;
        VideoUrl = videoUrl;
        VideoPublicId = videoPublicId;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string description, string genre)
    {
        if (!string.IsNullOrWhiteSpace(title)) Title = title;
        if (!string.IsNullOrWhiteSpace(description)) Description = description;
        if (!string.IsNullOrWhiteSpace(genre)) Genre = genre;
    }
}