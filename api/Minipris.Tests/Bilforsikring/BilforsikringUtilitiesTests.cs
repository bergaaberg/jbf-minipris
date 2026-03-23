using Minipris.Bilforsikring.Utilities;

namespace Minipris.Tests.Bilforsikring;

public class BilforsikringUtilitiesTests
{
    [Test]
    public async Task NormaliserRegnummer_FjernerMellomrom()
    {
        var result = BilforsikringUtilities.NormaliserRegnummer("AB 12345");

        await Assert.That(result).IsEqualTo("AB12345");
    }

    [Test]
    public async Task NormaliserRegnummer_KonvertererTilStorBokstav()
    {
        var result = BilforsikringUtilities.NormaliserRegnummer("ab12345");

        await Assert.That(result).IsEqualTo("AB12345");
    }

    [Test]
    public async Task FormaterRegnummer_LeggerTilMellomromEtterToBokstaver()
    {
        var result = BilforsikringUtilities.FormaterRegnummer("AB12345");

        await Assert.That(result).IsEqualTo("AB 12345");
    }

    [Test]
    public async Task FormaterRegnummer_KortRegnummer_ReturnererUendret()
    {
        var result = BilforsikringUtilities.FormaterRegnummer("AB");

        await Assert.That(result).IsEqualTo("AB");
    }

    [Test]
    public async Task FormaterRegnummer_NormalisererForFormatering()
    {
        var result = BilforsikringUtilities.FormaterRegnummer("ab 12345");

        await Assert.That(result).IsEqualTo("AB 12345");
    }
}
