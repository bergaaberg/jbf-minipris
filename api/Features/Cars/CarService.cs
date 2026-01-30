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
        ["AB12345"] = 685,
        ["CD67890"] = 495,
        ["EF11111"] = 890,
        ["GH22222"] = 745,
        ["IJ33333"] = 620
    };

    public static Task<CarInsuranceQuote?> GetQuote(string regNumber)
    {
        var normalized = regNumber.Replace(" ", "").ToUpperInvariant();
        var car = Cars.FirstOrDefault(c => c.RegNumber == normalized);

        if (car is not null)
        {
            var price = BasePrices.GetValueOrDefault(normalized, CalculateRandomPrice());
            return Task.FromResult<CarInsuranceQuote?>(new CarInsuranceQuote
            {
                RegNumber = FormatRegNumber(car.RegNumber),
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                InsurancePrice = price,
                Coverage = "Kasko",
                Bonus = 70,
                Deductible = 4000,
                CoverageOptions = GenerateCoverageOptions(price)
            });
        }

        return Task.FromResult<CarInsuranceQuote?>(null);
    }

    public static Task<CarInsuranceQuote> GetEstimate(CarEstimateRequest request)
    {
        var price = CalculateRandomPrice();

        return Task.FromResult(new CarInsuranceQuote
        {
            RegNumber = "ESTIMAT",
            Make = request.Make,
            Model = request.Model,
            Year = request.Year,
            InsurancePrice = price,
            Coverage = "Kasko",
            Bonus = 70,
            Deductible = 4000,
            CoverageOptions = GenerateCoverageOptions(price)
        });
    }



    private static int CalculateRandomPrice()
    {
        var random = new Random();
        var price = 350 + random.Next(600);
        return price / 5 * 5;
    }

    private static string FormatRegNumber(string regNumber)
    {
        var clean = regNumber.Replace(" ", "").ToUpperInvariant();
        return clean.Length >= 3 ? $"{clean[..2]} {clean[2..]}" : clean;
    }

    private static List<CoverageOption> GenerateCoverageOptions(int kaskoPrice)
    {
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
