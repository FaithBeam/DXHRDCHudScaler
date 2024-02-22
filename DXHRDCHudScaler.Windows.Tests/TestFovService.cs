namespace DXHRDCHudScaler.Windows.Tests;

public class TestFovService
{
    [Test]
    public void TestTryGetFov()
    {
        var sut = new FovService();
        var result = sut.TryGetCurrentFov(out var fov);
        ;
    }

    [Test]
    public void TestSetFov()
    {
        var sut = new FovService();
        sut.SetFov(75);
    }
}
