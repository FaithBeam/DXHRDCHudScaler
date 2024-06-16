using System.Runtime.Versioning;
using DXHRDCHudScaler.Core.Services;
using Microsoft.Win32;

namespace DXHRDCHudScaler.Windows;

[SupportedOSPlatform("windows")]
public class FindDxhrdcExeService : IFindDxhrdcExeService
{
    public bool TryFind(out string path)
    {
        return TryFindDefault(out path) || TryFindGogInstall(out path);
    }

    private static bool TryFindDefault(out string path)
    {
        path = "";
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"
            );
            if (key is not null)
            {
                var subKeyNames = key.GetSubKeyNames();
                foreach (var subKeyName in subKeyNames)
                {
                    using var subKey = key.OpenSubKey(subKeyName);
                    if (subKey is not null)
                    {
                        var displayName = subKey.GetValue("DisplayName")?.ToString();
                        if (!string.IsNullOrWhiteSpace(displayName))
                        {
                            if (displayName.Contains("Deus Ex: Human Revolution"))
                            {
                                var installLocation = subKey
                                    .GetValue("InstallLocation")
                                    ?.ToString();
                                if (!string.IsNullOrWhiteSpace(installLocation))
                                {
                                    var exeName = "DXHRDC.exe";
                                    if (displayName == "Deus Ex: Human Revolution")
                                    {
                                        exeName = "dxhr.exe";
                                    }
                                    path = Path.Combine(installLocation, exeName);
                                    return File.Exists(path);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }

    private static bool TryFindGogInstall(out string path)
    {
        path = "";
        try
        {
            using var key = Registry.LocalMachine.OpenSubKey(
                @"SOFTWARE\WOW6432Node\GOG.com\Games\1370227705"
            );
            if (key is not null)
            {
                var exeValue = key.GetValue("exe")?.ToString();
                if (!string.IsNullOrWhiteSpace(exeValue))
                {
                    path = exeValue;
                    return true;
                }
            }
        }
        catch (Exception)
        {
            return false;
        }

        return false;
    }
}
