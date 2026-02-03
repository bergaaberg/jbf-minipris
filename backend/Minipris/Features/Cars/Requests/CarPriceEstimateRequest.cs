namespace Minipris.Features.Cars.Requests;

public record CarPriceEstimateRequest(string Make, string Model, int Year, int Mileage);
