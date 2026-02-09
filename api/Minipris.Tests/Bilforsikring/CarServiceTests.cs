using Minipris.Bilforsikring;
using Minipris.Bilforsikring.Requests;

namespace Minipris.Tests.Bilforsikring;

public class CarServiceTests
{
    private readonly CarInfoRepository _carInfoRepository = new();
    private readonly CarService _carService;

    public CarServiceTests()
    {
        _carService = new CarService(_carInfoRepository);
    }

    [Test]
    public async Task GetQuote_WithValidRegNumber_ReturnsQuote()
    {
        //Arrange
        const string validRegistrationNumber = "AB12345";
        const string expectedMake = "Toyota";
        
        //Act
        var car = _carInfoRepository.GetCar(validRegistrationNumber);

        var request = new PriceRequest(
            car!.Make,
            car.Model,
            car.Year,
            RegNumber: validRegistrationNumber
        );

        var quote = await _carService.GetQuote(request);

        //Assert
        await Assert.That(quote.Make).IsEqualTo(expectedMake);
    }

    [Test]
    public async Task GetCar_WithUnknownRegNumber_ReturnsNull()
    {
        //Arrange
        const string invalidRegistrationNumber = "UNKNOWN";
        
        //Act
        var car = _carInfoRepository.GetCar(invalidRegistrationNumber);

        //Assert
        await Assert.That(car).IsNull();
    }

    [Test]
    public async Task GetEstimate_ReturnsReviewableCalculations()
    {
        //Arrange
        var request = new PriceRequest("Ford", "Focus", 2020);

        //Act
        var result = await _carService.GetEstimate(request);

        //Assert
        await Assert.That(result.InsurancePrice).IsGreaterThan(0);
    }
}