using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Minipris.Features.Cars;

public static class CarsEndpoints
{
    public static void MapCarsEndpoints(this RouteGroupBuilder routeGroupBuilder)
    {
        var carsGroup = routeGroupBuilder.MapGroup("/bilforsikring");

        carsGroup.MapGet("/{regNumber}/tilbud", GetQuote)
            .WithName("GetCarInsuranceQuote")
            .WithDescription("Hent forsikringstilbud for en bil basert på registreringsnummer");
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
}
