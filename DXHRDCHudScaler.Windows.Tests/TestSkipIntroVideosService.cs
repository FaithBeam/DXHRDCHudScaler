using DXHRDCHudScaler.Core.Services;

namespace DXHRDCHudScaler.Windows.Tests;

public class TestSkipIntroVideosService
{
    private const string Path =
        @"C:\Program Files (x86)\Steam\steamapps\common\Deus Ex - Human Revolution\dxhr.exe";

    [Test]
    public void CanPatch_True()
    {
        var sut = new SkipIntroVideosService();

        Assert.That(sut.CanPatch(Path));
    }

    [Test]
    public void CanUnPatch_True()
    {
        var sut = new SkipIntroVideosService();

        Assert.That(sut.CanUnPatch(Path));
    }

    [Test]
    public void Patch()
    {
        var sut = new SkipIntroVideosService();

        sut.Patch(Path);
    }

    [Test]
    public void UnPatch()
    {
        var sut = new SkipIntroVideosService();

        sut.UnPatch(Path);
    }
}
