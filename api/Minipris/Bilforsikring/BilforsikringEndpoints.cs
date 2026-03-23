using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Requests;
using Minipris.Bilforsikring.Responses;
using Minipris.Bilforsikring.Services;
using Minipris.Bilforsikring.Utilities;

namespace Minipris.Bilforsikring;

public static class BilforsikringEndpoints
{
    public static void MapBilforsikringEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        var gruppe = routeGroupBuilder.MapGroup("/bilforsikring");

        gruppe.MapGet("/{regnummer}/tilbud", GetEksaktPris)
            .WithName("GetEksaktPris")
            .WithDescription("Hent forsikringstilbud for en bil basert på registreringsnummer");

        gruppe.MapPost("/estimat", GetPrisanslag)
            .WithName("GetPrisanslag")
            .WithDescription("Hent prisestimat ved å legge inn bilinfo manuelt");
    }

    private static async Task<Results<Ok<BilforsikringstilbudResponse>, NotFound>> GetEksaktPris(
        [FromRoute] string regnummer,
        [FromServices] BilInfoRepository bilInfoRepository,
        [FromServices] BilService bilService)
    {
        var bil = bilInfoRepository.GetBil(regnummer);

        if (bil is null)
            return TypedResults.NotFound();

        var detaljer = new BilPrisdetaljer(
            bil.Merke,
            bil.Modell,
            bil.Arsmodell,
            Regnummer: BilforsikringUtilities.NormaliserRegnummer(regnummer)
        );

        var tilbud = await bilService.GetEksaktPris(detaljer);
        return TypedResults.Ok(tilbud);
    }

    private static async Task<Ok<BilforsikringstilbudResponse>> GetPrisanslag(
        [FromBody] BilPrisanslagRequest request,
        [FromServices] BilService bilService)
    {
        var detaljer = new BilPrisdetaljer(
            request.Merke,
            request.Modell,
            request.Arsmodell
        );

        var tilbud = await bilService.GetPrisanslag(detaljer);
        return TypedResults.Ok(tilbud);
    }
}
