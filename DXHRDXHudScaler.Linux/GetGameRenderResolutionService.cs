using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;

namespace DXHRDXHudScaler.Linux;

public class GetGameRenderResolutionService : IGetGameRenderResolutionService
{
    public bool TryGetGameResolution(out Resolution? resolution)
    {
        resolution = null;
        return false;
    }
}