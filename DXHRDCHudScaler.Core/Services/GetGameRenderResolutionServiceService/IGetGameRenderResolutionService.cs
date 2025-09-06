using DXHRDCHudScaler.Core.Models;

namespace DXHRDCHudScaler.Core.Services.GetGameRenderResolutionServiceService;

public interface IGetGameRenderResolutionService
{
    bool TryGetGameResolution(out Resolution? resolution);
}
