namespace DXHRDCHudScaler.Windows.Tests;

public class TestFindDxhrdcExeService
{
    [Test]
    public void TestTryFind()
    {
        var sut = new FindDxhrdcExeService();
        sut.TryFind(out var path);
    }
}
