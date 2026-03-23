using Minipris.Bilforsikring.Models;
using Minipris.Bilforsikring.Services;

namespace Minipris.Tests.Bilforsikring;

public class BilServiceTests
{
    private readonly BilInfoRepository _bilInfoRepository = new();
    private readonly BilService _bilService;

    public BilServiceTests()
    {
        _bilService = new BilService(_bilInfoRepository);
    }

    [Test]
    public async Task GetEksaktPris_WithValidRegNumber_ReturnsTilbud()
    {
        //Arrange
        const string validRegistrationNumber = "AB12345";
        const string expectedMerke = "Toyota";

        //Act
        var bil = _bilInfoRepository.GetBil(validRegistrationNumber);

        var detaljer = new BilPrisdetaljer(
            bil!.Merke,
            bil.Modell,
            bil.Arsmodell,
            Regnummer: validRegistrationNumber
        );

        var tilbud = await _bilService.GetEksaktPris(detaljer);

        //Assert
        await Assert.That(tilbud.Merke).IsEqualTo(expectedMerke);
    }

    [Test]
    public async Task GetBil_WithUnknownRegNumber_ReturnsNull()
    {
        //Arrange
        const string invalidRegistrationNumber = "UNKNOWN";

        //Act
        var bil = _bilInfoRepository.GetBil(invalidRegistrationNumber);

        //Assert
        await Assert.That(bil).IsNull();
    }

    [Test]
    public async Task GetPrisanslag_ReturnsTilbudWithPris()
    {
        //Arrange
        var detaljer = new BilPrisdetaljer("Ford", "Focus", 2020);

        //Act
        var tilbud = await _bilService.GetPrisanslag(detaljer);

        //Assert
        await Assert.That(tilbud.Forsikringspris).IsGreaterThan(0);
    }

    [Test]
    public async Task GetEksaktPris_MedBonus_RedusererForsikringspris()
    {
        // AB12345 har grunnpris 2400, bonus 50% → 2400 * 0.5 = 1200
        var bil = _bilInfoRepository.GetBil("AB12345");
        var detaljer = new BilPrisdetaljer(bil!.Merke, bil.Modell, bil.Arsmodell, Bonus: 50, Regnummer: "AB12345");

        var tilbud = await _bilService.GetEksaktPris(detaljer);

        await Assert.That(tilbud.Forsikringspris).IsEqualTo(1200);
    }

    [Test]
    public async Task GetEksaktPris_ReturnererFormatertRegnummer()
    {
        var bil = _bilInfoRepository.GetBil("AB12345");
        var detaljer = new BilPrisdetaljer(bil!.Merke, bil.Modell, bil.Arsmodell, Regnummer: "AB12345");

        var tilbud = await _bilService.GetEksaktPris(detaljer);

        await Assert.That(tilbud.Regnummer).IsEqualTo("AB 12345");
    }

    [Test]
    public async Task GetEksaktPris_UtenRegnummer_ReturnererEstimat()
    {
        var detaljer = new BilPrisdetaljer("Skoda", "Octavia", 2019);

        var tilbud = await _bilService.GetEksaktPris(detaljer);

        await Assert.That(tilbud.Regnummer).IsEqualTo("ESTIMAT");
    }

    [Test]
    public async Task GetEksaktPris_ReturnererTreDekningsalternativer()
    {
        var bil = _bilInfoRepository.GetBil("AB12345");
        var detaljer = new BilPrisdetaljer(bil!.Merke, bil.Modell, bil.Arsmodell, Regnummer: "AB12345");

        var tilbud = await _bilService.GetEksaktPris(detaljer);

        await Assert.That(tilbud.Dekningsalternativer.Count).IsEqualTo(3);
    }
}
