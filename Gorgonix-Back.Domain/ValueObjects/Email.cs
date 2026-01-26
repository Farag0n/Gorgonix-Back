namespace Gorgonix_Back.Domain.ValueObjects;

public sealed class Email
{
    public string Value { get; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email vacío o nulo");
        
        if (!value.Contains("@"))
            throw new ArgumentException("Email inválido");
        
        if (!value.Contains("."))
            throw new ArgumentException("Email invalido");
        
        Value = value;
    }
    
    public override bool Equals(object? obj) => obj is Email other && Value == other.Value;
    
    public override int GetHashCode() => Value.GetHashCode();
}