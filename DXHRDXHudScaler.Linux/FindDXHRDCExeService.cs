using DXHRDCHudScaler.Core.Services;

namespace DXHRDXHudScaler.Linux;

public class FindDXHRDCExeService : IFindDxhrdcExeService
{
    public bool TryFind(out string path)
    {
        path = "";
        return false;
    }
}