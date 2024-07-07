using PatternFinder;

namespace DXHRDCHudScaler.Core.Services;

public enum SkipIntroVideosPatchStatus
{
    Patched,
    UnPatched
}

public class SkipIntroVideosService : ISkipIntroVideosService
{
    private static readonly PatternFinder.Pattern.Byte[] UnpatchedPatternBytes = Pattern.Transform(
        "38??????????740A83F8"
    );
    private static readonly PatternFinder.Pattern.Byte[] PatchedPatternBytes = Pattern.Transform(
        "38??????????909083F8"
    );

    public bool CanPatch(string pathToExe)
    {
        if (!File.Exists(pathToExe))
        {
            return false;
        }

        var exeBytes = File.ReadAllBytes(pathToExe);

        return Pattern.FindAll(exeBytes, UnpatchedPatternBytes, out _);
    }

    public bool CanUnPatch(string pathToExe)
    {
        if (!File.Exists(pathToExe))
        {
            return false;
        }

        var exeBytes = File.ReadAllBytes(pathToExe);

        return Pattern.FindAll(exeBytes, PatchedPatternBytes, out _);
    }

    public SkipIntroVideosPatchStatus GetCurrentPatchStatus(string pathToExe) =>
        CanPatch(pathToExe)
            ? SkipIntroVideosPatchStatus.UnPatched
            : SkipIntroVideosPatchStatus.Patched;

    public void Patch(string pathToExe)
    {
        var exeBytes = File.ReadAllBytes(pathToExe);

        _ = Pattern.Find(exeBytes, UnpatchedPatternBytes, out var offset);

        exeBytes[offset + 6] = 0x90;
        exeBytes[offset + 7] = 0x90;

        File.WriteAllBytes(pathToExe, exeBytes);
    }

    public void UnPatch(string pathToExe)
    {
        var exeBytes = File.ReadAllBytes(pathToExe);

        _ = Pattern.Find(exeBytes, PatchedPatternBytes, out var offset);

        exeBytes[offset + 6] = 0x74;
        exeBytes[offset + 7] = 0x0A;

        File.WriteAllBytes(pathToExe, exeBytes);
    }
}
