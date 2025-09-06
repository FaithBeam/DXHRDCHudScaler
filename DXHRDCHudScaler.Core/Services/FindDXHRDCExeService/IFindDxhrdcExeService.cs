namespace DXHRDCHudScaler.Core.Services.FindDXHRDCExeService;

public interface IFindDxhrdcExeService
{
    bool TryFind(out string path);
}
