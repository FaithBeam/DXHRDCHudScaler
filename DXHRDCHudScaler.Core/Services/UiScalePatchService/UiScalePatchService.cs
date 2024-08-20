using PatternFinder;

namespace DXHRDCHudScaler.Core.Services.UiScalePatchService;

public class UiScalePatchService : IUiScalePatchService
{
    private static readonly PatternFinder.Pattern.Byte[] UiScalePattern1Bytes = Pattern.Transform(
        "81 FF ?? ?? ?? ?? 7C 05 BF ?? ?? ?? ?? DB 44 24"
    );

    private static readonly PatternFinder.Pattern.Byte[] UiScalePattern2Bytes = Pattern.Transform(
        "81 FE ?? ?? ?? ?? 7D 08 8B CE 89 74 24 20 EB 09 B9 ?? ?? ?? ?? 89 4C 24 20"
    );

    public bool CanPatch(string pathToExe, string pathToBackup)
    {
        if (!File.Exists(pathToExe) || File.Exists(pathToBackup))
        {
            return false;
        }

        var exeBytes = File.ReadAllBytes(pathToExe);

        var patternFound =
            Pattern.Find(exeBytes, UiScalePattern1Bytes, out _)
            && Pattern.Find(exeBytes, UiScalePattern2Bytes, out _);

        Pattern.FindAll(exeBytes, UiScalePattern1Bytes, out _);
        Pattern.FindAll(exeBytes, UiScalePattern2Bytes, out _);

        if (!patternFound)
        {
            throw new Exception($"Pattern not found exception in {pathToExe}");
        }

        return patternFound;
    }

    public void Patch(string pathToExe, uint width)
    {
        if (!File.Exists(pathToExe))
        {
            throw new FileNotFoundException(pathToExe);
        }

        var exeBytes = File.ReadAllBytes(pathToExe);

        if (
            Pattern.Find(exeBytes, UiScalePattern1Bytes, out var offset1)
            && Pattern.Find(exeBytes, UiScalePattern2Bytes, out var offset2)
        )
        {
            var widthBytes = BitConverter.GetBytes(width);

            exeBytes[offset1 + 2] = widthBytes[0];
            exeBytes[offset1 + 3] = widthBytes[1];
            exeBytes[offset1 + 4] = 0;
            exeBytes[offset1 + 5] = 0;
            exeBytes[offset1 + 9] = widthBytes[0];
            exeBytes[offset1 + 10] = widthBytes[1];
            exeBytes[offset1 + 11] = 0;
            exeBytes[offset1 + 12] = 0;

            exeBytes[offset2 + 2] = widthBytes[0];
            exeBytes[offset2 + 3] = widthBytes[1];
            exeBytes[offset2 + 4] = 0;
            exeBytes[offset2 + 5] = 0;
            exeBytes[offset2 + 17] = widthBytes[0];
            exeBytes[offset2 + 18] = widthBytes[1];
            exeBytes[offset2 + 19] = 0;
            exeBytes[offset2 + 20] = 0;

            File.WriteAllBytes(pathToExe, exeBytes);
        }
        else
        {
            throw new Exception($"Unable to find pattern bytes for {pathToExe}");
        }
    }
}
