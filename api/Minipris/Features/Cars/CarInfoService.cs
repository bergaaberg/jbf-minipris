using Minipris.Features.Cars.Models;

namespace Minipris.Features.Cars;

public class CarInfoService
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

    public Car? GetCar(string regNumber)
    {
        var normalized = NormalizeRegNumber(regNumber);
        return Cars.FirstOrDefault(c => c.RegNumber == normalized);
    }

    public int? GetBasePrice(string regNumber)
    {
        var normalized = NormalizeRegNumber(regNumber);
        return BasePrices.TryGetValue(normalized, out var price) ? price : null;
    }

    public static string NormalizeRegNumber(string regNumber) =>
        regNumber.Replace(" ", "").ToUpperInvariant();

    public static string FormatRegNumber(string regNumber)
    {
        var clean = NormalizeRegNumber(regNumber);
        return clean.Length >= 3 ? $"{clean[..2]} {clean[2..]}" : clean;
    }
}
