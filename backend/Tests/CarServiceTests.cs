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
        var request = new CarPriceEstimateRequest("Ford", "Focus", 2020);

        var result = await CarService.GetEstimate(request);

        await Assert.That(result).IsNotNull();
        await Assert.That(result.Make).IsEqualTo("Ford");
        
        await Assert.That(result.Bonus).IsEqualTo(10); 
    }
}
