using DXHRDCHudScaler.Core.Models;

namespace DXHRDCHudScaler.Core.Services;

public interface IGetGameRenderResolutionService
{
    bool TryGetGameResolution(out Resolution? resolution);
}
