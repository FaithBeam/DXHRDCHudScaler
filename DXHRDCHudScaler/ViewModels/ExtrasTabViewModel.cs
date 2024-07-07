using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Models;
using DynamicData;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public class ExtrasTabViewModel : ViewModelBase, IExtrasTabViewModel
{
    private bool _applyBtnChecked;
    private bool _skipIntroVideosCheckboxChecked;
    private bool _skipIntroVideosCheckboxEnabled;

    private readonly IAppState _appState;
    private readonly ISkipIntroVideosService _skipIntroVideosService;
    private SourceCache<Job, string> _jobCache = new(x => x.Name);

    public ExtrasTabViewModel(
        IScreen screen,
        IAppState appState,
        ISkipIntroVideosService skipIntroVideosService
    )
    {
        HostScreen = screen;
        _appState = appState;
        _skipIntroVideosService = skipIntroVideosService;
        _skipIntroVideosCheckboxEnabled = _appState.GameExePathExists();

        this.WhenAnyValue(x => x._appState.GameExePath)
            .Subscribe(x =>
            {
                _jobCache.Clear();
                SkipIntroVideosCheckboxChecked = CheckboxCheckedLogicOnGameExeChanged();
                SkipIntroVideosCheckboxEnabled = _appState.GameExePathExists();
            });

        this.WhenAnyValue(x => x.SkipIntroVideosCheckboxChecked)
            .Skip(1)
            .Subscribe(skipIntroVideosChecked =>
            {
                if (
                    string.IsNullOrWhiteSpace(_appState.GameExePath)
                    || !_appState.GameExePathExists()
                )
                {
                    return;
                }

                if (skipIntroVideosChecked)
                {
                    _jobCache.AddOrUpdate(
                        new Job(
                            "SkipIntroVideosEnable",
                            () => _skipIntroVideosService.Patch(_appState.GameExePath!)
                        )
                    );
                }
                else
                {
                    if (_jobCache.Items.Any(x => x.Name == "SkipIntroVideosEnable"))
                    {
                        _jobCache.Remove("SkipIntroVideosEnable");
                    }
                    else
                    {
                        _jobCache.AddOrUpdate(
                            new Job(
                                "SkipIntroVideosDisable",
                                () => _skipIntroVideosService.UnPatch(_appState.GameExePath!)
                            )
                        );
                    }
                }
            });

        _jobCache
            .Connect()
            .Subscribe(x =>
            {
                CheckIfApplyBtnShouldBeEnabled();
            });
        ApplyCmd = ReactiveCommand.Create(() =>
        {
            foreach (var job in _jobCache.Items)
            {
                job.Action();
            }

            _jobCache.Clear();
        });
    }

    private bool CheckboxCheckedLogicOnGameExeChanged()
    {
        if (!SkipIntroVideosCheckboxEnabled || string.IsNullOrWhiteSpace(_appState.GameExePath))
        {
            return SkipIntroVideosCheckboxChecked;
        }
        return _skipIntroVideosService.GetCurrentPatchStatus(_appState.GameExePath) switch
        {
            SkipIntroVideosPatchStatus.Patched => true,
            SkipIntroVideosPatchStatus.UnPatched => false,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public bool ApplyBtnEnabled
    {
        get => _applyBtnChecked;
        set => this.RaiseAndSetIfChanged(ref _applyBtnChecked, value);
    }

    public bool SkipIntroVideosCheckboxEnabled
    {
        get => _skipIntroVideosCheckboxEnabled;
        set => this.RaiseAndSetIfChanged(ref _skipIntroVideosCheckboxEnabled, value);
    }

    public bool SkipIntroVideosCheckboxChecked
    {
        get => _skipIntroVideosCheckboxChecked;
        set => this.RaiseAndSetIfChanged(ref _skipIntroVideosCheckboxChecked, value);
    }

    public ReactiveCommand<Unit, Unit> ApplyCmd { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; }

    /// <summary>
    /// If the job cache has any items, that means there are changes that could be applied and therefore the apply button should be enabled.
    /// </summary>
    private void CheckIfApplyBtnShouldBeEnabled()
    {
        ApplyBtnEnabled = _jobCache.Items.Any();
    }
}
