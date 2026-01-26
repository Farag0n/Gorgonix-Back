namespace Gorgonix_Back.Domain.Entities;

public class Genre
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    
    protected  Genre() { }

    public Genre(string name)
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("El genero no puede estar vacio o ser null.");
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("El genero no puede estar vacio o ser null.");
        Name = name;
    }
}