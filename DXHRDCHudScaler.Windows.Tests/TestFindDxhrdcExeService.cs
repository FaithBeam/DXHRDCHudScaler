namespace DXHRDCHudScaler.Windows.Tests;

public class TestFindDxhrdcExeService
{
    [Test]
    public void TestTryFind()
    {
        var sut = new FindDxhrdcExeService();
        var result = sut.TryFind(out var path);
        ;
    }
}
