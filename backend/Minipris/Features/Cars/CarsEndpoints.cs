using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Features.Cars.Requests;

namespace Minipris.Features.Cars;

public static class CarsEndpoints
{
    public static void MapCarsEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        var carsGroup = routeGroupBuilder.MapGroup("/bilforsikring");

        carsGroup.MapGet("/{regNumber}/tilbud", GetQuote)
            .WithName("GetCarInsuranceQuote")
            .WithDescription("Hent forsikringstilbud for en bil basert på registreringsnummer");

        carsGroup.MapPost("/estimat", GetEstimate)
            .WithName("GetCarInsuranceEstimate")
            .WithDescription("Hent prisestimat ved å legge inn bilinfo manuelt");
    }

    private static async Task<Results<Ok<Models.CarInsuranceQuote>, NotFound>> GetQuote(
        [FromRoute] string regNumber,
        [FromServices] CarService carService)
    {
        var quote = await CarService.GetQuote(regNumber);

        if (quote is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(quote);
    }

    private static async Task<Ok<Models.CarInsuranceQuote>> GetEstimate(
        [FromBody] CarEstimateRequest request,
        [FromServices] CarService carService)
    {
        var quote = await CarService.GetEstimate(request);
        return TypedResults.Ok(quote);
    }
}
