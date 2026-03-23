using Minipris.Bilforsikring.Services;

namespace Minipris.Tests.Bilforsikring;

public class BilInfoRepositoryTests
{
    private readonly BilInfoRepository _repo = new();

    [Test]
    public async Task GetBil_WithKjentRegnummer_ReturnsBilMedKorrekteDetaljer()
    {
        var bil = _repo.GetBil("AB12345");

        await Assert.That(bil).IsNotNull();
        await Assert.That(bil!.Merke).IsEqualTo("Toyota");
        await Assert.That(bil.Modell).IsEqualTo("Rav4");
        await Assert.That(bil.Arsmodell).IsEqualTo(2020);
    }

    [Test]
    public async Task GetBil_MedSmaBokstaver_ReturnsBil()
    {
        var bil = _repo.GetBil("ab12345");

        await Assert.That(bil).IsNotNull();
    }

    [Test]
    public async Task GetBil_MedMellomrom_ReturnsBil()
    {
        var bil = _repo.GetBil("AB 12345");

        await Assert.That(bil).IsNotNull();
    }

    [Test]
    public async Task GetGrunnpris_WithKjentRegnummer_ReturnsKorrektPris()
    {
        var grunnpris = _repo.GetGrunnpris("AB12345");

        await Assert.That(grunnpris).IsEqualTo(2400);
    }

    [Test]
    public async Task GetGrunnpris_WithUkjentRegnummer_ReturnsNull()
    {
        var grunnpris = _repo.GetGrunnpris("UKJENT");

        await Assert.That(grunnpris).IsNull();
    }
}
