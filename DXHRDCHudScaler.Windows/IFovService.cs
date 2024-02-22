namespace DXHRDCHudScaler.Windows;

public interface IFovService
{
    bool TryGetCurrentFov(out uint fov);
    void SetFov(uint fov);
}