namespace DXHRDCHudScaler.Core.Services.UninstallService;

public interface IUninstallService
{
    /// <summary>
    /// Can uninstall patch
    /// </summary>
    /// <param name="pathToBackup">Full path to the backed up exe</param>
    /// <returns></returns>
    bool CanUninstall(string pathToBackup);

    /// <summary>
    /// Uninstall patch
    /// </summary>
    /// <param name="pathToBackup">Full path to backup</param>
    /// <param name="destination">Full path to backup destination</param>
    void Uninstall(string pathToBackup, string destination);
}
