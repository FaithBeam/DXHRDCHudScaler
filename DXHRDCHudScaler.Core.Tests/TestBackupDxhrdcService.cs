using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Core.Services.BackupService;
using DXHRDCHudScaler.Core.Services.FindDXHRDCExeService;

namespace DXHRDCHudScaler.Core.Tests;

public class TestBackupDxhrdcService
{
    [Test]
    public void TestCanBackup()
    {
        var findService = new FindDxhrdcExeServiceWin();
        if (findService.TryFind(out var pathToExe))
        {
            var sut = new BackupDxhrdcService();
            Assert.That(sut.CanBackup(pathToExe, pathToExe + ".bak"));
        }
    }

    [Test]
    public void TestBackup()
    {
        var findService = new FindDxhrdcExeServiceWin();
        if (findService.TryFind(out var pathToExe))
        {
            var sut = new BackupDxhrdcService();
            var backupPath = pathToExe + ".bak";
            sut.Backup(pathToExe, backupPath);
            Assert.That(File.Exists(backupPath));
        }
    }
}
