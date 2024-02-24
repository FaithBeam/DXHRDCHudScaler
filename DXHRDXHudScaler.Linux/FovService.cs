using DXHRDCHudScaler.Core.Services;

namespace DXHRDXHudScaler.Linux;

public class FovService : IFovService
{
    public bool TryGetCurrentFov(out uint fov)
    {
        fov = 75;
        return false;
    }

    public void SetFov(uint fov)
    {
    }
}