using Minipris.Bilforsikring.Models;

namespace Minipris.Bilforsikring;

public class CarInfoRepository
{
    private static readonly List<Car> Cars =
    [
        new("AB12345", "Toyota", "Rav4", 2020),
        new("CD67890", "Volkswagen", "Golf", 2012),
        new("EF11111", "Tesla", "Model 3", 2022),
        new("GH22222", "Nissan", "Qashqai", 2018),
        new("EC55555", "Hyundai", "Kona", 2021)
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

    public static string NormalizeRegNumber(string regNumber)
    {
        return regNumber.Replace(" ", "").ToUpperInvariant();
    }

    public static string FormatRegNumber(string regNumber)
    {
        var clean = NormalizeRegNumber(regNumber);
        return clean.Length >= 3 ? $"{clean[..2]} {clean[2..]}" : clean;
    }
}