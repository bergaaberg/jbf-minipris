using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Features.Cars.Models;
using Minipris.Features.Cars.Requests;
using Microsoft.Extensions.Logging;

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

    private static async Task<Results<Ok<CarInsuranceQuote>, NotFound>> GetQuote(
        [FromRoute] string regNumber,
        [FromServices] CarService carService,
        [FromServices] ILogger<CarsEndpoints> logger)
    {
        var quote = await CarService.GetQuote(regNumber);

        if (quote is null)
        {
            logger.LogWarning("Quote not found for reg {RegNumber}", regNumber);
            return TypedResults.NotFound();
        }

        logger.LogInformation("Quote generated for reg {RegNumber} with price {Price}", regNumber, quote.InsurancePrice);
        return TypedResults.Ok(quote);
    }

    private static async Task<Results<Ok<CarInsuranceQuote>, BadRequest<string>>> GetEstimate(
        [FromBody] CarPriceEstimateRequest request,
        [FromServices] CarService carService,
        [FromServices] ILogger<CarsEndpoints> logger)
    {
        if (request is null)
            return TypedResults.BadRequest("Request body mangler eller er ugyldig.");

        if (string.IsNullOrWhiteSpace(request.Make) || string.IsNullOrWhiteSpace(request.Model))
            return TypedResults.BadRequest("Merke og modell må oppgis.");

        if (request.Year <= 1900 || request.Year > DateTime.Now.Year + 1)
            return TypedResults.BadRequest("Ugyldig årsmodell.");

        if (request.Mileage <= 0)
            return TypedResults.BadRequest("Ugyldig kjørelengde.");

        try
        {
            var quote = await CarService.GetEstimate(request);
            logger.LogInformation("Manual estimate created for {Make} {Model} {Year}, mileage {Mileage}, price {Price}", request.Make, request.Model, request.Year, request.Mileage, quote.InsurancePrice);
            return TypedResults.Ok(quote);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error computing estimate for {Make} {Model} {Year}", request.Make, request.Model, request.Year);
            return TypedResults.BadRequest("Kunne ikke beregne estimat. Prøv igjen senere.");
        }
    }
}
