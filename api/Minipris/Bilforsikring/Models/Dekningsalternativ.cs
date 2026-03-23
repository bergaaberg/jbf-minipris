namespace Minipris.Bilforsikring.Models;

public record Dekningsalternativ
{
    public required string Navn { get; init; }
    public required int Pris { get; init; }
    public required string Beskrivelse { get; init; }
}
