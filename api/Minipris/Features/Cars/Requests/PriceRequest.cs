namespace Minipris.Features.Cars.Requests;

public record PriceRequest(
    string Make,
    string Model,
    int Year,
    int Bonus = 70,
    string? RegNumber = null
);
