using System.Runtime.Versioning;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using Microsoft.Win32;

namespace DXHRDCHudScaler.Windows;

[SupportedOSPlatform("windows")]
public class GetGameRenderResolutionServiceService : IGetGameRenderResolutionService
{
    public bool TryGetGameResolution(out Resolution? resolution)
    {
        resolution = null;
        using var key =
            Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HRDC\Graphics")
            ?? Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HR\Graphics");
        var fullscreenWidthValue = key?.GetValue("FullscreenWidth");
        if (fullscreenWidthValue == null)
        {
            return false;
        }

        var fullscreenWidthStr = fullscreenWidthValue.ToString();
        if (string.IsNullOrWhiteSpace(fullscreenWidthStr))
        {
            return false;
        }

        if (!uint.TryParse(fullscreenWidthStr, out var fullscreenWidthUint))
        {
            return false;
        }

        resolution = new Resolution(fullscreenWidthUint, 0);
        return true;
    }
}
