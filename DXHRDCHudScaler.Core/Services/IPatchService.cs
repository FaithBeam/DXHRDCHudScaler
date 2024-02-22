namespace DXHRDCHudScaler.Core.Services;

public interface IPatchService
{
    Task<bool> CanPatchAsync(string pathToExe, string pathToBackup);
    Task PatchAsync(string pathToExe, uint width);
}
