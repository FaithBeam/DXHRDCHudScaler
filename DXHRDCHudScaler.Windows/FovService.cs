using System.Runtime.Versioning;
using DXHRDCHudScaler.Core.Services;
using Microsoft.Win32;

namespace DXHRDCHudScaler.Windows;

[SupportedOSPlatform("windows")]
public class FovService : IFovService
{
    public bool TryGetCurrentFov(out uint fov)
    {
        fov = 0;
        using var key =
            Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HRDC")
            ?? Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HR")
            ?? throw new Exception("Error opening Deus Ex: HR registry key");
        if (key.GetValue("g_fov") is not byte[] gFov)
        {
            return false;
        }
        fov = BitConverter.ToUInt32(gFov);
        return true;
    }

    public void SetFov(uint fov)
    {
        using var key =
            Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HRDC", true)
            ?? Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HR", true)
            ?? throw new Exception(@"Error opening Deus Ex: HR registry key");
        var bytes = BitConverter.GetBytes(fov);
        key.SetValue("g_fov", bytes);
    }
}
