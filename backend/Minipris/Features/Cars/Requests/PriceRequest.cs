namespace Minipris.Features.Cars.Requests;

/// <summary>
/// Internal request object used for price calculation.
/// Both reg number lookup and manual estimate flow through this.
/// </summary>
public record PriceRequest(
    string Make,
    string Model,
    int Year,
    int Bonus = 70,
    string? RegNumber = null
);
