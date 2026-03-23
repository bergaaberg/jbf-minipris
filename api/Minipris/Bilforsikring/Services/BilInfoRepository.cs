using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Utilities;

namespace Minipris.Bilforsikring.Services;

public class BilInfoRepository
{
    private static readonly List<Bil> Biler =
    [
        new("AB12345", "Toyota", "Rav4", 2020),
        new("CD67890", "Volkswagen", "Golf", 2012),
        new("EF11111", "Tesla", "Model 3", 2022),
        new("GH22222", "Nissan", "Qashqai", 2018),
        new("EC55555", "Hyundai", "Kona", 2021),
        new("IJ33333", "Toyota", "Corolla", 2019)

    ];

    private static readonly Dictionary<string, int> Grunnpriser = new()
    {
        ["AB12345"] = 2400,
        ["CD67890"] = 1650,
        ["EF11111"] = 3200,
        ["GH22222"] = 2600,
        ["EC55555"] = 2100,
        ["IJ33333"] = 2100
    };

    public Bil? GetBil(string regnummer)
    {
        var normalized = BilforsikringUtilities.NormaliserRegnummer(regnummer);
        return Biler.FirstOrDefault(b => b.Regnummer == normalized);
    }

    public int? GetGrunnpris(string regnummer)
    {
        var normalized = BilforsikringUtilities.NormaliserRegnummer(regnummer);
        return Grunnpriser.TryGetValue(normalized, out var pris) ? pris : null;
    }
}
