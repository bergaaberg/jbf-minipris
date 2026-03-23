namespace Minipris.Bilforsikring.Utilities;

public static class BilforsikringUtilities
{
    public static string NormaliserRegnummer(string regnummer)
    {
        return regnummer.Replace(" ", "").ToUpperInvariant();
    }

    public static string FormaterRegnummer(string regnummer)
    {
        var clean = NormaliserRegnummer(regnummer);
        return clean.Length >= 3 ? $"{clean[..2]} {clean[2..]}" : clean;
    }
}