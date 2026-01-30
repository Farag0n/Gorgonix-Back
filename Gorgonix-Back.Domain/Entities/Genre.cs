namespace Gorgonix_Back.Domain.Entities;

public class Genre
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    // Opcional: Navegación inversa para saber qué pelis son de este género
    public ICollection<Content> Contents { get; private set; } = new List<Content>();
    
    protected Genre() { }

    public Genre(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("El género requiere nombre.");
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
    }
}