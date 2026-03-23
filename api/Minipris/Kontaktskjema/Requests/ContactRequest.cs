namespace Minipris.Kontaktskjema.Requests;

public record ContactRequest
{
    public required string Regnummer { get; init; }
    public string? Telefonnummer { get; init; }
    public string? Epost { get; init; }
}
