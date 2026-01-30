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

        if (car is not null)
        {
            var basePrice = BasePrices.GetValueOrDefault(normalized, GenerateBasePrice());
            var bonus = 70; // Standard startbonus
            var price = CalculatePremium(basePrice, bonus);

            return Task.FromResult<CarInsuranceQuote?>(new CarInsuranceQuote
            {
                RegNumber = FormatRegNumber(car.RegNumber),
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                InsurancePrice = price,
                Coverage = "Kasko",
                Bonus = bonus,
                Deductible = 4000,
                CoverageOptions = GenerateCoverageOptions(basePrice, bonus)
            });
        }

        return Task.FromResult<CarInsuranceQuote?>(null);
    }

    public static Task<CarInsuranceQuote> GetEstimate(CarEstimateRequest request)
    {
        var basePrice = GenerateBasePrice();
        var bonus = 70; // Standard startbonus for estimates
        var price = CalculatePremium(basePrice, bonus);

        return Task.FromResult(new CarInsuranceQuote
        {
            RegNumber = "ESTIMAT",
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            InsurancePrice = price,
            Coverage = "Kasko",
            Bonus = bonus,
            Deductible = 4000,
            CoverageOptions = GenerateCoverageOptions(basePrice, bonus)
        });
    }

    private static int GenerateBasePrice()
    {
        var random = new Random();
        // Genererer en grunnpris før bonus (typisk 1500 - 4500)
        return 1500 + random.Next(3000);
    }

    private static int CalculatePremium(int basePrice, int bonus = 0)
    {
        // Enkel prismodell: Reduserer prisen med bonus-prosenten
        decimal multiplier = 1.0m - (bonus / 100.0m);
        var price = basePrice * multiplier;
        return (int)price;
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
                Price = (int)(kaskoPrice * 0.6), // Ansvar er billigere
                Description = "Dekker skade på andres kjøretøy, eiendom og personer."
            },
            new()
            {
                Name = "Delkasko",
                Price = (int)(kaskoPrice * 0.8), // Delkasko er litt billigere enn kasko
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
