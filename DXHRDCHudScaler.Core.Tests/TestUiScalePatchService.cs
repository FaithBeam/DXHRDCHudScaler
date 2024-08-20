using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Core.Services.UiScalePatchService;
using DXHRDCHudScaler.Windows;
using DXHRDXHudScaler.Linux;

namespace DXHRDCHudScaler.Core.Tests;

public class TestUiScalePatchService
{
    [Test]
    public void TestCanPatch()
    {
        IFindDxhrdcExeService? service = null;
        if (OperatingSystem.IsWindows())
        {
            service = new FindDxhrdcExeService();
        }
        else if (OperatingSystem.IsLinux())
        {
            service = new FindDXHRDCExeService();
        }

        if (service is null)
        {
            throw new Exception();
        }

        if (service.TryFind(out var path))
        {
            var sut = new UiScalePatchService();
            Assert.That(sut.CanPatch(path, path + ".bak"));
        }
    }

    [Test]
    public void TestPatch()
    {
        IFindDxhrdcExeService? service = null;
        if (OperatingSystem.IsWindows())
        {
            service = new FindDxhrdcExeService();
        }

        if (service is null)
        {
            throw new Exception();
        }

        if (service.TryFind(out var path))
        {
            var sut = new UiScalePatchService();
            sut.Patch(path, 3840);
        }
    }
}
