namespace DXHRDCHudScaler.Windows.Tests;

public class TestGetGameResolutionService
{
    [Test]
    public void TestTryGetGameResolution()
    {
        var sut = new GetGameRenderResolutionServiceService();
        var result = sut.TryGetGameResolution(out var resolution);
        Assert.Multiple(() =>
        {
            Assert.That(result);
            Assert.That(resolution, Is.Not.Null);
            Assert.That(resolution!.Width, Is.EqualTo(3840));
        });
    }
}
