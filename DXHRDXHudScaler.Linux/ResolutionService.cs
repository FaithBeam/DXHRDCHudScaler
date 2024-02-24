using System.Diagnostics;
using System.Text.RegularExpressions;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using DynamicData;

namespace DXHRDXHudScaler.Linux;

public class ResolutionService : IResolutionService
{
    private static long _id;
    private readonly SourceCache<Resolution, long> _resolutionSourceCache = new(x => x.Id);

    public ResolutionService()
    {
        GetResolutions();
    }

    public IObservable<IChangeSet<Resolution, long>> Connect() => _resolutionSourceCache.Connect();

    public void AddResolution(uint width, uint height) =>
        _resolutionSourceCache.AddOrUpdate(new Resolution(width, height));

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
        _resolutionSourceCache.AddOrUpdate(resolution);
        return resolution;
    }

    public Resolution? CurrentDesktopResolution  => _resolutionSourceCache.Items.FirstOrDefault();
    
    private const string FileName = "/bin/bash";
    private const string Cmd = "-c xrandr";
    private readonly Regex _resolutionRx = new Regex(@"\d{3,}x\d{3,}", RegexOptions.Compiled | RegexOptions.IgnoreCase);


    private void GetResolutions()
    {
        var psi = new ProcessStartInfo
        {
            FileName = FileName,
            Arguments = Cmd,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        using var process = Process.Start(psi);
        var output = "";
        if (process != null)
        {
            process.WaitForExit();
            output = process.StandardOutput.ReadToEnd();
        }

        if (string.IsNullOrWhiteSpace(output))
        {
            return;
        }

        var matches = _resolutionRx.Matches(output);
        foreach (Match m in matches)
        {
            var split = m.Value.Split('x');
            TryAddResolution(uint.Parse(split[0]), uint.Parse(split[1]), out _);
        }
    }
}