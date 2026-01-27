namespace Gorgonix_Back.Domain.Entities;

public class Content
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public Guid GenreId { get; private set; }
    public Genre Genre { get; private set; }
    public string PosterUrl { get; private set; }
    public string PosterPublicId { get; private set; }
    public string VideoUrl { get; private set; }
    public string VideoPublicId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public Guid? ReviewId { get; private set; }
    public Review Review { get; private set; }

    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    protected Content() { }

    public Content(string title, string description, Guid genreId, string posterUrl, string posterPublicId, string videoUrl, string videoPublicId, Guid? reviewId = null)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título requerido");
        if (string.IsNullOrWhiteSpace(description)) throw new ArgumentException("La descripcion no puede estar vacia o ser null");
        if (GenreId != Guid.Empty) throw new ArgumentException("Debe tener un genero");
        if (string.IsNullOrWhiteSpace(posterUrl)) throw new ArgumentException("Poster requerido");
        if (string.IsNullOrWhiteSpace(videoUrl)) throw new ArgumentException("Video requerido");

        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        GenreId = genreId;
        PosterUrl = posterUrl;
        PosterPublicId = posterPublicId;
        VideoUrl = videoUrl;
        VideoPublicId = videoPublicId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ReviewId = reviewId;
    }

    public void Update(string title, string description, Guid genreId, string posterUrl, string videoUrl)
    {
        if (!string.IsNullOrWhiteSpace(title)) Title = title;
        if (!string.IsNullOrWhiteSpace(description)) Description = description;
        if (genreId != Guid.Empty ) GenreId = genreId;
        if (string.IsNullOrWhiteSpace(posterUrl)) throw new ArgumentException("Poster requerido");
        if (string.IsNullOrWhiteSpace(videoUrl)) throw new ArgumentException("Video requerido");
        
        Title = title;
        Description = description;
        GenreId = genreId;
        PosterUrl = posterUrl;
        VideoUrl = videoUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddReview(Review review)
    {
        if (review == null && review.Id != Guid.Empty)
            ReviewId = review.Id;
    }
}