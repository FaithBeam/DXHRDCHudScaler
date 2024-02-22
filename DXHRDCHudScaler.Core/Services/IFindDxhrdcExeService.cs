namespace DXHRDCHudScaler.Core.Services;

public interface IFindDxhrdcExeService
{
    bool TryFind(out string path);
}