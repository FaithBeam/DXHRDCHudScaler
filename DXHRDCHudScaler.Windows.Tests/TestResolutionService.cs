using DynamicData;

namespace DXHRDCHudScaler.Windows.Tests;

public class TestResolutionService
{
    [Test]
    public void GetResolutions()
    {
        var sut = new ResolutionService();
        var cs = sut.Connect().Bind(out var resolutions).Subscribe();
        Assert.That(resolutions, Is.Unique);
    }

    [Test]
    public void TestCurrentDesktopResolution()
    {
        var sut = new ResolutionService();
        Assert.Multiple(() =>
        {
            Assert.That(sut.CurrentDesktopResolution, Is.Not.Null);
            Assert.That(sut.CurrentDesktopResolution.Width, Is.EqualTo(3840));
            Assert.That(sut.CurrentDesktopResolution.Height, Is.EqualTo(2160));
        });
    }
}
