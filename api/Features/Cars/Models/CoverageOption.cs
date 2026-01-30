namespace Minipris.Features.Cars.Models;

public record CoverageOption
{
    public required string Name { get; init; }
    public required int Price { get; init; }
    public required string Description { get; init; }
}
