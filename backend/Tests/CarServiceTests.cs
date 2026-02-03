using Minipris.Features.Cars;
using Minipris.Features.Cars.Requests;

namespace Tests;

public class CarServiceTests
{
    [Test]
    public async Task GetQuote_WithValidRegNumber_ReturnsQuote()
    {
        var quote = await CarService.GetQuote("AB12345");

        await Assert.That(quote).IsNotNull();
        await Assert.That(quote!.RegNumber).IsEqualTo("AB 12345");
        await Assert.That(quote.Make).IsEqualTo("Toyota");
    }

    [Test]
    public async Task GetQuote_WithUnknownRegNumber_ReturnsNull()
    {
        var quote = await CarService.GetQuote("UNKNOWN");

        await Assert.That(quote).IsNull();
    }

    [Test]
    public async Task GetEstimate_ReturnsReviewableCalculations()
    {
        var request = new CarPriceEstimateRequest("Ford", "Focus", 2020, 8000);

        var result = await CarService.GetEstimate(request);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Make).IsEqualTo("Ford");
        await Assert.That(result.Bonus).IsEqualTo(70);
    }

    [Test]
    public async Task GetEstimate_WithMileage_AdjustsPrice()
    {
        var basePrice = 2000;

        // 8000 -> standard
        var r8000 = new CarPriceEstimateRequest("Ford", "Focus", 2020, 8000);
        var res8000 = await CarService.GetEstimate(r8000, basePrice);
        var expected8000 = (int)Math.Round(basePrice * (1 - 70 / 100.0m));
        await Assert.That(res8000.InsurancePrice).IsEqualTo(expected8000);

        // 12000 -> +10%
        var r12000 = new CarPriceEstimateRequest("Ford", "Focus", 2020, 12000);
        var res12000 = await CarService.GetEstimate(r12000, basePrice);
        var expected12000 = (int)Math.Round(basePrice * (1 - 70 / 100.0m) * 1.1m);
        await Assert.That(res12000.InsurancePrice).IsEqualTo(expected12000);

        // 16000 -> +20%
        var r16000 = new CarPriceEstimateRequest("Ford", "Focus", 2020, 16000);
        var res16000 = await CarService.GetEstimate(r16000, basePrice);
        var expected16000 = (int)Math.Round(basePrice * (1 - 70 / 100.0m) * 1.2m);
        await Assert.That(res16000.InsurancePrice).IsEqualTo(expected16000);
    }
}
