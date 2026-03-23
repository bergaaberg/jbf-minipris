namespace Minipris.Bilforsikring.Models;

public record BilPrisdetaljer(
    string Merke,
    string Modell,
    int Arsmodell,
    int Bonus = 70,
    string? Regnummer = null
);
