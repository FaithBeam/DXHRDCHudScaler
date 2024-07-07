namespace DXHRDCHudScaler.Core.Services;

public interface IAppState
{
    string? GameExePath { get; set; }
    bool GameExePathExists();
}