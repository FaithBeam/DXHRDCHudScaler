using ReactiveUI;

namespace DXHRDCHudScaler.Core.Services;

public class AppState : ReactiveObject, IAppState
{
    private string? _gameExePath;

    public string? GameExePath
    {
        get => _gameExePath;
        set => this.RaiseAndSetIfChanged(ref _gameExePath, value);
    }

    public bool GameExePathExists() => File.Exists(GameExePath);
}
