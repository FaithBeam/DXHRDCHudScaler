namespace DXHRDCHudScaler.Core.Services;

public interface ISkipIntroVideosService
{
    bool CanPatch(string pathToExe);
    bool CanUnPatch(string pathToExe);
    SkipIntroVideosPatchStatus GetCurrentPatchStatus(string pathToExe);
    void Patch(string pathToExe);
    void UnPatch(string pathToExe);
}
