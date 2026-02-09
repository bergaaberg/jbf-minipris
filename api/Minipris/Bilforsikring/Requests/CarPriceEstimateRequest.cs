namespace Minipris.Bilforsikring.Requests;

public record CarPriceEstimateRequest(string Make, string Model, int Year);