using DXHRDCHudScaler.Core.Models;

namespace DXHRDCHudScaler.Models;

public class ResolutionProxy(IResolution resolution, IResolution desktopResolution) : IResolution
{
    public long Id { get; } = resolution.Id;
    public uint Width { get; } = resolution.Width;

    public override string ToString() =>
        $"{Width} | {(Width / (double)1280) * 100:0.00}% | {(Width / (double)desktopResolution.Width) * 100:0.00}%";
}
