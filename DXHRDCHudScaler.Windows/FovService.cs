using System.Security.AccessControl;
using System.Text;
using Microsoft.Win32;

namespace DXHRDCHudScaler.Windows;

public class FovService : IFovService
{
    public bool TryGetCurrentFov(out uint fov)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException(
                "Unreachable code detected trying to get game's render resolution"
            );
        }

        fov = 0;
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HRDC");
        if (key?.GetValue("g_fov") is not byte[] gFov)
            return false;
        fov = BitConverter.ToUInt32(gFov);
        return true;
    }

    public void SetFov(uint fov)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new InvalidOperationException(
                "Unreachable code detected trying to get game's render resolution"
            );
        }
        using var key = Registry.CurrentUser.OpenSubKey(@"Software\Eidos\Deus Ex: HRDC", true);
        if (key is null)
        {
            throw new Exception(
                @"Unable to open registry key CurrentUser Software\Eidos\Deus Ex: HRDC"
            );
        }
        var bytes = BitConverter.GetBytes(fov);
        key.SetValue("g_fov", bytes);
    }
}
