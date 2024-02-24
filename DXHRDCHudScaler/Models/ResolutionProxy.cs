using DXHRDCHudScaler.Core.Models;

namespace DXHRDCHudScaler.Models;

public class ResolutionProxy(IResolution resolution, IResolution? gameResolution) : IResolution
{
    public long Id { get; } = resolution.Id;
    public uint Width { get; } = resolution.Width;

    public override string ToString()
    {
        return gameResolution is null
            ? $"{Width} | {(Width / (double)1280) * 100:0.00}% | ??? (Game resolution not found)"
            : $"{Width} | {(Width / (double)1280) * 100:0.00}% | {(Width / (double)gameResolution.Width) * 100:0.00}%";
    }
}