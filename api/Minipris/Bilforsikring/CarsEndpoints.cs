using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Requests;

namespace Minipris.Bilforsikring;

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
        [FromServices] CarInfoRepository carInfoRepository,
        [FromServices] CarService carService)
    {
        var car = carInfoRepository.GetCar(regNumber);

        if (car is null)
            return TypedResults.NotFound();

        var request = new PriceRequest(
            car.Make,
            car.Model,
            car.Year,
            RegNumber: CarInfoRepository.NormalizeRegNumber(regNumber)
        );

        var quote = await carService.GetQuote(request);
        return TypedResults.Ok(quote);
    }

    private static async Task<Ok<CarInsuranceQuote>> GetEstimate(
        [FromBody] CarPriceEstimateRequest request,
        [FromServices] CarService carService)
    {
        var priceRequest = new PriceRequest(
            request.Make,
            request.Model,
            request.Year
        );

        var quote = await carService.GetEstimate(priceRequest);
        return TypedResults.Ok(quote);
    }
}