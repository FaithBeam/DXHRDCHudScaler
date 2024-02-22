using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using DynamicData;
using Windows.Win32;
using Windows.Win32.Graphics.Gdi;

namespace DXHRDCHudScaler.Windows;

public class ResolutionService : IResolutionService
{
    private static long _id;
    private readonly SourceCache<Resolution, long> _resolutionSourceCache = new(x => x.Id);

    public ResolutionService()
    {
        GetResolutions();
        GetCurrentDesktopResolution();
    }

    public IObservable<IChangeSet<Resolution, long>> Connect() => _resolutionSourceCache.Connect();

    public Resolution? CurrentDesktopResolution { get; private set; }

    public void AddResolution(uint width, uint height) =>
        _resolutionSourceCache.AddOrUpdate(new Resolution(_id++, width, height));

    public bool TryAddResolution(uint width, uint height, out Resolution resolution)
    {
        var newRes = new Resolution(_id, width, height);
        resolution = newRes;
        if (_resolutionSourceCache.Items.Any(x => x.Equals(newRes)))
            return false;
        _resolutionSourceCache.AddOrUpdate(newRes);
        _id++;
        return true;
    }

    public Resolution AddResolution(Resolution resolution)
    {
        // give resolution new id if it matches any id already in cache
        if (_resolutionSourceCache.Items.Any(r => r.Id == resolution.Id))
        {
            var newRes = new Resolution(++_id, resolution.Width, resolution.Height);
            _resolutionSourceCache.AddOrUpdate(newRes);
            return newRes;
        }

        _resolutionSourceCache.AddOrUpdate(resolution);
        return resolution;
    }

    private void GetCurrentDesktopResolution()
    {
        if (!OperatingSystem.IsWindows() || !OperatingSystem.IsWindowsVersionAtLeast(5))
            return;

        var devMode = new DEVMODEW();
        if (
            PInvoke.EnumDisplaySettings(
                null,
                ENUM_DISPLAY_SETTINGS_MODE.ENUM_CURRENT_SETTINGS,
                ref devMode
            )
        )
        {
            CurrentDesktopResolution = _resolutionSourceCache.Items.FirstOrDefault(r =>
                r.Width == devMode.dmPelsWidth && r.Height == devMode.dmPelsHeight
            );
        }
    }

    private void GetResolutions()
    {
        var devMode = new DEVMODEW();
        var i = (ENUM_DISPLAY_SETTINGS_MODE)0;
        if (!OperatingSystem.IsWindows() || !OperatingSystem.IsWindowsVersionAtLeast(5))
        {
            throw new Exception("Unsupported windows version");
        }
        while (PInvoke.EnumDisplaySettings(null, i, ref devMode))
        {
            TryAddResolution(devMode.dmPelsWidth, devMode.dmPelsHeight, out _);
            i++;
        }
    }
}
