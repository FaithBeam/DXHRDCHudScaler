namespace DXHRDCHudScaler.Core.Services;

public interface IBackupDxhrdcService
{
    /// <summary>
    /// Backup Deus Ex Human Revolution Director's Cut exe
    /// </summary>
    /// <param name="sourceFileName">Full path to the game's exe</param>
    /// <param name="destFileName">Full path to create the backup</param>
    /// <returns>False if the path to exe does not exist, false if a backup already exists, true if neither</returns>
    bool CanBackup(string sourceFileName, string destFileName);

    void Backup(string sourceFileName, string destFileName);
}
