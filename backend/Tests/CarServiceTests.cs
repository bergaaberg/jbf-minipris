using Minipris.Features.Cars;
using Minipris.Features.Cars.Requests;

namespace Minipris.Tests.Features.Cars;

public class CarServiceTests
{
    [Test]
    public async Task GetQuote_WithValidRegNumber_ReturnsQuote()
    {
        // Act
        var quote = await CarService.GetQuote("AB12345");

        // Assert
        await Assert.That(quote).IsNotNull();
        await Assert.That(quote!.RegNumber).IsEqualTo("AB 12345");
        await Assert.That(quote.Make).IsEqualTo("Toyota");
    }
    
    [Test]
    public async Task GetQuote_WithUnknownRegNumber_ReturnsNull()
    {
        // Act
        var quote = await CarService.GetQuote("UNKNOWN");

        // Assert
        await Assert.That(quote).IsNull();
    }

    [Test]
    public async Task GetEstimate_ReturnsReviewableCalculations()
    {
        // Arrange
        var request = new CarEstimateRequest("Ford", "Focus", 2020);

        // Act
        var result = await CarService.GetEstimate(request);

        // Assert
        await Assert.That(result).IsNotNull();
        await Assert.That(result.Make).IsEqualTo("Ford");
        
        // Intentionally broken test: The bonus logic is hardcoded to 70 in the service,
        // but let's assert it should be 10 for new customers in our "requirements"
        await Assert.That(result.Bonus).IsEqualTo(10); 
    }
}
