using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Windows;

namespace DXHRDCHudScaler.Core.Tests;

public class TestUninstallService
{
    [Test]
    public void TestCanUninstall()
    {
        var findService = new FindDxhrdcExeService();
        if (findService.TryFind(out var path))
        {
            var sut = new UninstallService();
            Assert.That(sut.CanUninstall(path + ".exe"), Is.False);
        }
    }

    [Test]
    public void TestUninstall()
    {
        var findService = new FindDxhrdcExeService();
        if (findService.TryFind(out var path))
        {
            var sut = new UninstallService();
            sut.Uninstall(path, path + ".exe");
        }
    }
}
