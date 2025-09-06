using DXHRDCHudScaler.Core.Models;

namespace DXHRDCHudScaler.Core.Services.GetGameRenderResolutionServiceService;

public class GetGameRenderResolutionServiceLinux : IGetGameRenderResolutionService
{
    public bool TryGetGameResolution(out Resolution? resolution)
    {
        resolution = null;
        return false;
    }
}
