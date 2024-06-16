using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Windows;
using DXHRDXHudScaler.Linux;

namespace DXHRDCHudScaler.Core.Tests;

public class Tests
{
    [Test]
    public async Task TestCanPatchAsync()
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
            var sut = new PatchService();
            Assert.That(await sut.CanPatchAsync(path, path + ".bak"));
        }
    }

    [Test]
    public async Task TestPatchAsync()
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
            var sut = new PatchService();
            await sut.PatchAsync(path, 3840);
        }
    }
}
