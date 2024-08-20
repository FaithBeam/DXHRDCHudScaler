namespace DXHRDCHudScaler.Core.Models;

public interface IAppState
{
    string? GameExePath { get; set; }
    bool GameExePathExists();
    GameType? GetGameType();
}
