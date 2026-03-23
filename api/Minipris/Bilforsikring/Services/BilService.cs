using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Requests;
using Minipris.Bilforsikring.Responses;
using Minipris.Bilforsikring.Utilities;

namespace Minipris.Bilforsikring.Services;

public class BilService(BilInfoRepository bilInfoRepository)
{
    public Task<BilforsikringstilbudResponse> GetEksaktPris(BilPrisdetaljer detaljer)
    {
        int grunnpris;
        if (detaljer.Regnummer is not null)
            grunnpris = bilInfoRepository.GetGrunnpris(detaljer.Regnummer) ?? GenererGrunnpris();
        else
            grunnpris = GenererGrunnpris();

        return Task.FromResult(BuildTilbud(detaljer, grunnpris));
    }

    public Task<BilforsikringstilbudResponse> GetPrisanslag(BilPrisdetaljer detaljer)
    {
        var grunnpris = GenererGrunnpris();
        return Task.FromResult(BuildTilbud(detaljer, grunnpris));
    }

    private static BilforsikringstilbudResponse BuildTilbud(BilPrisdetaljer detaljer, int grunnpris)
    {
        var pris = BeregnPremie(grunnpris, detaljer.Bonus);

        string regnummer;
        if (detaljer.Regnummer is not null)
            regnummer = BilforsikringUtilities.FormaterRegnummer(detaljer.Regnummer);
        else
            regnummer = "ESTIMAT";

        return new BilforsikringstilbudResponse
        {
            Regnummer = regnummer,
            Merke = detaljer.Merke,
            Modell = detaljer.Modell,
            Arsmodell = detaljer.Arsmodell,
            Forsikringspris = pris,
            Dekning = "Kasko",
            Bonus = detaljer.Bonus,
            Egenandel = 4000,
            Dekningsalternativer = GenererDekningsalternativer(grunnpris, detaljer.Bonus)
        };
    }

    private static int GenererGrunnpris()
    {
        return 1500 + Random.Shared.Next(3000);
    }

    private static int BeregnPremie(int grunnpris, int bonus = 0)
    {
        var multiplier = 1.0m - bonus / 100.0m;
        var pris = grunnpris * multiplier;
        return (int)pris;
    }

    private static List<Dekningsalternativ> GenererDekningsalternativer(int grunnpris, int bonus)
    {
        var kaskoPrice = BeregnPremie(grunnpris, bonus);

        return
        [
            new()
            {
                Navn = "Ansvar",
                Pris = (int)(kaskoPrice * 0.6),
                Beskrivelse = "Dekker skade på andres kjøretøy, eiendom og personer."
            },
            new()
            {
                Navn = "Delkasko",
                Pris = (int)(kaskoPrice * 0.8),
                Beskrivelse = "Inkluderer ansvar, pluss tyveri, brann, og glasskade."
            },
            new()
            {
                Navn = "Kasko",
                Pris = kaskoPrice,
                Beskrivelse = "Full dekning inkludert skade på eget kjøretøy ved kollisjon/utforkjøring."
            }
        ];
    }
}
