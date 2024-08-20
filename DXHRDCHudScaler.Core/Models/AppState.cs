using ReactiveUI;

namespace DXHRDCHudScaler.Core.Models;

public class AppState : ReactiveObject, IAppState
{
    private string? _gameExePath;

    public string? GameExePath
    {
        get => _gameExePath;
        set { this.RaiseAndSetIfChanged(ref _gameExePath, value); }
    }

    public GameType? GetGameType()
    {
        if (string.IsNullOrWhiteSpace(GameExePath))
        {
            return null;
        }

        if (GameExePath.EndsWith("dxhr.exe"))
        {
            return GameType.Original;
        }

        if (GameExePath.EndsWith("DXHRDC.exe"))
        {
            return GameType.DirectorsCut;
        }

        throw new Exception($"Unknown game {GameExePath}");
    }

    public bool GameExePathExists() => File.Exists(GameExePath);
}
