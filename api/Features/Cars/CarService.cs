using Minipris.Features.Cars.Models;

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
                Coverage = "Kasko"
            });
        }

        var randomQuote = GenerateRandomQuote(normalized);
        return Task.FromResult<CarInsuranceQuote?>(randomQuote);
    }

    private static CarInsuranceQuote GenerateRandomQuote(string regNumber)
    {
        var makes = new[] { "Ford", "Audi", "Mercedes", "Nissan", "Mazda" };
        var models = new[] { "Focus", "A4", "C-klasse", "Qashqai", "CX-5" };
        var index = Random.Shared.Next(makes.Length);

        return new CarInsuranceQuote
        {
            RegNumber = FormatRegNumber(regNumber),
            Make = makes[index],
            Model = models[index],
            Year = 2015 + Random.Shared.Next(9),
            InsurancePrice = CalculateRandomPrice(),
            Coverage = "Kasko"
        };
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
}
