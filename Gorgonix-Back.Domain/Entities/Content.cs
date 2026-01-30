namespace Gorgonix_Back.Domain.Entities;

public class Content
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string PosterUrl { get; private set; }
    public string PosterPublicId { get; private set; }
    public string VideoUrl { get; private set; }
    public string VideoPublicId { get; private set; }
    
    // Relaciones
    public Guid GenreId { get; private set; }
    public Genre Genre { get; private set; }
    
    // Colecciones (Solo lectura hacia afuera para proteger la lista)
    private readonly List<Review> _reviews = new();
    public IReadOnlyCollection<Review> Reviews => _reviews.AsReadOnly();
    
    private readonly List<Favorite> _favoritedBy = new();
    public IReadOnlyCollection<Favorite> FavoritedBy => _favoritedBy.AsReadOnly();

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Content() { }

    public Content(string title, string description, Guid genreId, string posterUrl, string posterPublicId, string videoUrl, string videoPublicId)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Título requerido");
        if (genreId == Guid.Empty) throw new ArgumentException("Debe tener un genero válido");

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
    }

    public void Update(string title, string description, Guid genreId, string posterUrl, string videoUrl)
    {
        // Validaciones...
        Title = title;
        Description = description;
        GenreId = genreId;
        if(!string.IsNullOrEmpty(posterUrl)) PosterUrl = posterUrl;
        if(!string.IsNullOrEmpty(videoUrl)) VideoUrl = videoUrl;
        UpdatedAt = DateTime.UtcNow;
    }
}