using Minipris.Features.Cars;
using Minipris.Features.Cars.Requests;

namespace Tests;

public class CarServiceTests
{
    private readonly CarInfoService _carInfoService = new();
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _carService = new CarService(_carInfoService);
    }

    [Test]
    public async Task GetQuote_WithValidRegNumber_ReturnsQuote()
    {
        var car = _carInfoService.GetCar("AB12345");

        var request = new PriceRequest(
            Make: car!.Make,
            Model: car.Model,
            Year: car.Year,
            RegNumber: "AB12345"
        );

        var quote = await _carService.GetQuote(request);

        await Assert.That(quote).IsNotNull();
        await Assert.That(quote.RegNumber).IsEqualTo("AB 12345");
        await Assert.That(quote.Make).IsEqualTo("Toyota");
    }

    [Test]
    public async Task GetCar_WithUnknownRegNumber_ReturnsNull()
    {
        var car = _carInfoService.GetCar("UNKNOWN");

        await Assert.That(car).IsNull();
    }

    [Test]
    public async Task GetEstimate_ReturnsReviewableCalculations()
    {
        var request = new PriceRequest(Make: "Ford", Model: "Focus", Year: 2020);

        var result = await _carService.GetEstimate(request);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Make).IsEqualTo("Ford");

        await Assert.That(result.Bonus).IsEqualTo(10);
    }
}
