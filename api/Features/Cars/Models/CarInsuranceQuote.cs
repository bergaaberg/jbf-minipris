namespace Minipris.Features.Cars.Models;

public record CarInsuranceQuote
{
    public required string RegNumber { get; init; }
    public required string Make { get; init; }
    public required string Model { get; init; }
    public required int Year { get; init; }
    public required int InsurancePrice { get; init; }
    public required string Coverage { get; init; }
}
