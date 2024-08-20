namespace DXHRDCHudScaler.Core.Services.UiScalePatchService;

public interface IUiScalePatchService
{
    bool CanPatch(string pathToExe, string pathToBackup);
    void Patch(string pathToExe, uint width);
}
