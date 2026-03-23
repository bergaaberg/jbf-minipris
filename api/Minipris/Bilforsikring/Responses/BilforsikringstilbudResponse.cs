using Minipris.Bilforsikring.Models;

namespace Minipris.Bilforsikring.Responses;

public record BilforsikringstilbudResponse
{
    public required string Regnummer { get; init; }
    public required string Merke { get; init; }
    public required string Modell { get; init; }
    public required int Arsmodell { get; init; }
    public required int Forsikringspris { get; init; }
    public required string Dekning { get; init; }
    public required int Bonus { get; init; }
    public required int Egenandel { get; init; }
    public required List<Dekningsalternativ> Dekningsalternativer { get; init; }
}
