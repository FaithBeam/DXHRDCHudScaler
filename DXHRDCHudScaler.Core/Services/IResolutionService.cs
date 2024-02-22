using DXHRDCHudScaler.Core.Models;
using DynamicData;

namespace DXHRDCHudScaler.Core.Services;

public interface IResolutionService
{
    public IObservable<IChangeSet<Resolution, long>> Connect();
    void AddResolution(uint width, uint height);
    bool TryAddResolution(uint width, uint height, out Resolution resolution);
    Resolution AddResolution(Resolution resolution);
    Resolution? CurrentDesktopResolution { get; }
}
