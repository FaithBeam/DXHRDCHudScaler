namespace DXHRDCHudScaler.Core.Services.UninstallService;

public class UninstallService : IUninstallService
{
    /// <summary>
    /// Can uninstall patch
    /// </summary>
    /// <param name="pathToBackup">Full path to the backed up exe</param>
    /// <returns></returns>
    public bool CanUninstall(string pathToBackup) => File.Exists(pathToBackup);

    /// <summary>
    /// Uninstall patch
    /// </summary>
    /// <param name="pathToBackup">Full path to backup</param>
    /// <param name="destination">Full path to backup destination</param>
    public void Uninstall(string pathToBackup, string destination)
    {
        if (File.Exists(pathToBackup))
        {
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            File.Move(pathToBackup, destination);
        }
    }
}
