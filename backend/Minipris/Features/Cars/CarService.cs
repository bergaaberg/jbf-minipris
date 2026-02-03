using Minipris.Features.Cars.Models;
using Minipris.Features.Cars.Requests;

namespace Minipris.Features.Cars;

public class CarService
{
    private static readonly List<Car> Cars =
    [
        new(RegNumber: "AB12345", Make: "Toyota", Model: "Rav4", Year: 2020),
        new(RegNumber: "CD67890", Make: "Volkswagen", Model: "Golf", Year: 2012),
        new(RegNumber: "EF11111", Make: "Tesla", Model: "Model 3", Year: 2022),
        new(RegNumber: "GH22222", Make: "Nissan", Model: "Qashqai", Year: 2018),
        new(RegNumber: "EC55555", Make: "Hyundai", Model: "Kona", Year: 2021)
    ];

    private static readonly Dictionary<string, int> BasePrices = new()
    {
        ["AB12345"] = 2400,
        ["CD67890"] = 1650,
        ["EF11111"] = 3200,
        ["GH22222"] = 2600,
        ["IJ33333"] = 2100
    };

    public static Task<CarInsuranceQuote?> GetQuote(string regNumber)
    {
        var normalized = regNumber.Replace(" ", "").ToUpperInvariant();
        var car = Cars.FirstOrDefault(c => c.RegNumber == normalized);

        if (car is null)
            return Task.FromResult<CarInsuranceQuote?>(null);

        // Build internal request with assumptions based on reg number lookup
        var priceRequest = new PriceRequest(
            Make: car.Make,
            Model: car.Model,
            Year: car.Year,
            Mileage: 8000, // Default mileage
            Bonus: 70, // Standard startbonus
            RegNumber: normalized
        );

        var basePrice = BasePrices.GetValueOrDefault(normalized, GenerateBasePrice());
        return Task.FromResult<CarInsuranceQuote?>(BuildQuote(priceRequest, basePrice));
    }

    public static Task<CarInsuranceQuote> GetEstimate(CarPriceEstimateRequest request, int? basePrice = null)
    {
        // Build internal request from user-provided parameters
        var priceRequest = new PriceRequest(
            Make: request.Make,
            Model: request.Model,
            Year: request.Year,
            Mileage: request.Mileage,
            Bonus: 70 // Standard startbonus for estimates
        );

        var bp = basePrice ?? GenerateBasePrice();
        return Task.FromResult(BuildQuote(priceRequest, bp));
    }

    private static CarInsuranceQuote BuildQuote(PriceRequest request, int basePrice)
    {
        var price = CalculatePremium(basePrice, request.Bonus, request.Mileage);

        return new CarInsuranceQuote
        {
            RegNumber = request.RegNumber is not null
                ? FormatRegNumber(request.RegNumber)
                : "ESTIMAT",
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            InsurancePrice = price,
            Coverage = "Kasko",
            Bonus = request.Bonus,
            Deductible = 4000,
            CoverageOptions = GenerateCoverageOptions(basePrice, request.Bonus)
        };
    }

    private static int GenerateBasePrice()
    {
        // Genererer en grunnpris før bonus (typisk 1500 - 4500)
        return 1500 + Random.Shared.Next(3000);
    }

    private static int CalculatePremium(int basePrice, int bonus = 0, int mileage = 8000)
    {
        var multiplier = 1.0m - (bonus / 100.0m);
        var price = basePrice * multiplier;

        switch (mileage)
        {
            case <= 8000:
                break;
            case <= 12000:
                price *= 1.1m;
                break;
            default:
                price *= 1.2m;
                break;
        }

        return (int)Math.Round(price);
    }

    private static string FormatRegNumber(string regNumber)
    {
        var clean = regNumber.Replace(" ", "").ToUpperInvariant();
        return clean.Length >= 3 ? $"{clean[..2]} {clean[2..]}" : clean;
    }

    private static List<CoverageOption> GenerateCoverageOptions(int basePrice, int bonus)
    {
        var kaskoPrice = CalculatePremium(basePrice, bonus);

        return
        [
            new()
            {
                Name = "Ansvar",
                Price = (int)(kaskoPrice * 0.6),
                Description = "Dekker skade på andres kjøretøy, eiendom og personer."
            },
            new()
            {
                Name = "Delkasko",
                Price = (int)(kaskoPrice * 0.8),
                Description = "Inkluderer ansvar, pluss tyveri, brann, og glasskade."
            },
            new()
            {
                Name = "Kasko",
                Price = kaskoPrice,
                Description = "Full dekning inkludert skade på eget kjøretøy ved kollisjon/utforkjøring."
            }
        ];
    }
}
