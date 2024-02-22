using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Windows;

namespace DXHRDCHudScaler.Core.Tests;

public class TestBackupDxhrdcService
{
    [Test]
    public void TestCanBackup()
    {
        var findService = new FindDxhrdcExeService();
        if (findService.TryFind(out var pathToExe))
        {
            var sut = new BackupDxhrdcService();
            Assert.That(sut.CanBackup(pathToExe, pathToExe + ".bak"));
        }
    }

    [Test]
    public void TestBackup()
    {
        var findService = new FindDxhrdcExeService();
        if (findService.TryFind(out var pathToExe))
        {
            var sut = new BackupDxhrdcService();
            var backupPath = pathToExe + ".bak";
            sut.Backup(pathToExe, backupPath);
            Assert.That(File.Exists(backupPath));
        }
    }
}
