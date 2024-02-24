using PatternFinder;

namespace DXHRDCHudScaler.Core.Services;

public class PatchService : IPatchService
{
    private const string Pattern1 = "81 FF ?? ?? ?? ?? 7C 05 BF ?? ?? ?? ?? DB 44 24";
    private const string Pattern2 =
        "81 FE ?? ?? ?? ?? 7D 08 8B CE 89 74 24 20 EB 09 B9 ?? ?? ?? ?? 89 4C 24 20";
    private static readonly PatternFinder.Pattern.Byte[] Pattern1Bytes = Pattern.Transform(
        Pattern1
    );
    private static readonly PatternFinder.Pattern.Byte[] Pattern2Bytes = Pattern.Transform(
        Pattern2
    );

    public async Task<bool> CanPatchAsync(string pathToExe, string pathToBackup)
    {
        if (!File.Exists(pathToExe) || File.Exists(pathToBackup))
        {
            return false;
        }

        var exeBytes = await File.ReadAllBytesAsync(pathToExe);

        var patternFound =
            Pattern.Find(exeBytes, Pattern1Bytes, out _)
            && Pattern.Find(exeBytes, Pattern2Bytes, out _);

        Pattern.FindAll(exeBytes, Pattern1Bytes, out var offsets1);
        Pattern.FindAll(exeBytes, Pattern2Bytes, out var offsets2);

        if (!patternFound)
        {
            throw new Exception($"Pattern not found exception in {pathToExe}");
        }

        return patternFound;
    }

    public async Task PatchAsync(string pathToExe, uint width)
    {
        if (!File.Exists(pathToExe))
        {
            throw new FileNotFoundException(pathToExe);
        }

        var exeBytes = await File.ReadAllBytesAsync(pathToExe);

        if (
            Pattern.Find(exeBytes, Pattern1Bytes, out var offset1)
            && Pattern.Find(exeBytes, Pattern2Bytes, out var offset2)
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

            await File.WriteAllBytesAsync(pathToExe, exeBytes);
        }
        else
        {
            throw new Exception($"Unable to find pattern bytes for {pathToExe}");
        }
    }
}
