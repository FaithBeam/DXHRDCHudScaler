﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Core.Services.BackupService;
using DXHRDCHudScaler.Core.Services.UiScalePatchService;
using DXHRDCHudScaler.Core.Services.UninstallService;
using DXHRDCHudScaler.Models;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public class MainTabViewModel : ViewModelBase, IMainTabViewModel
{
    private readonly IResolutionService _resolutionService;
    private readonly IUiScalePatchService _uiScalePatchService;
    private readonly IBackupDxhrdcService _backupDxhrdcService;
    private readonly IUninstallService _uninstallService;
    private string? _browseTextBox;
    private ResolutionProxy? _selectedResolution;
    private readonly ReadOnlyObservableCollection<ResolutionProxy> _resolutions;
    private readonly ObservableAsPropertyHelper<bool>? _canPatch;
    private readonly ObservableAsPropertyHelper<bool>? _patchCmdIsExecuting;
    private readonly ObservableAsPropertyHelper<bool>? _canUninstall;
    private readonly ObservableAsPropertyHelper<bool>? _uninstallCmdIsExecuting;
    private bool _modalOpen;
    private readonly IAppState _appState;

    public MainTabViewModel(
        IScreen screen,
        IAppState appState,
        IResolutionService resolutionService,
        IGetGameRenderResolutionService gameRenderResolutionService,
        IUiScalePatchService uiScalePatchService,
        IBackupDxhrdcService backupDxhrdcService,
        IUninstallService uninstallService,
        IFindDxhrdcExeService findDxhrdcExeService
    )
    {
        HostScreen = screen;
        _appState = appState;
        _resolutionService = resolutionService;
        _uiScalePatchService = uiScalePatchService;
        _backupDxhrdcService = backupDxhrdcService;
        _uninstallService = uninstallService;

        AddResolutionCmd = ReactiveCommand.CreateFromTask(ShowAddCustomResolutionDialogAsync);
        AddResolutionInteraction = new Interaction<Unit, Resolution?>();

        BrowseCmd = ReactiveCommand.CreateFromTask(ShowOpenFileDialogAsync);
        BrowseInteraction = new Interaction<Unit, IStorageFile?>();

        var canPatch = this.WhenAnyValue(x => x.CanPatch, selector: canPatchBool => canPatchBool);
        PatchCmd = ReactiveCommand.Create(Patch, canPatch);
        _patchCmdIsExecuting = PatchCmd.IsExecuting.ToProperty(
            this,
            x => x.PatchCmdIsExecuting,
            scheduler: RxApp.MainThreadScheduler
        );
        _canPatch = this.WhenAnyValue(
                x => x.BrowseTextBox,
                x => x.PatchCmdIsExecuting,
                x => x.UninstallCmdIsExecuting
            )
            .Select(x =>
            {
                if (x.Item2 || x.Item3 || string.IsNullOrWhiteSpace(x.Item1))
                {
                    return false;
                }
                var backupPath = x.Item1 + ".bak";
                return _uiScalePatchService.CanPatch(x.Item1, backupPath);
            })
            .ToProperty(this, x => x.CanPatch, scheduler: RxApp.MainThreadScheduler);

        var canUninstall = this.WhenAnyValue(
            x => x.CanUninstall,
            selector: canUninstallBool => canUninstallBool
        );
        UninstallCmd = ReactiveCommand.Create(Uninstall, canUninstall);
        _uninstallCmdIsExecuting = UninstallCmd.IsExecuting.ToProperty(
            this,
            x => x.UninstallCmdIsExecuting,
            scheduler: RxApp.MainThreadScheduler
        );
        _canUninstall = this.WhenAnyValue(
                x => x.BrowseTextBox,
                x => x.PatchCmdIsExecuting,
                x => x.UninstallCmdIsExecuting
            )
            .Select(x =>
            {
                if (x.Item2 || x.Item3 || string.IsNullOrWhiteSpace(x.Item1))
                {
                    return false;
                }
                var backupPath = x.Item1 + ".bak";
                return _uninstallService.CanUninstall(backupPath);
            })
            .ToProperty(this, x => x.CanUninstall, scheduler: RxApp.MainThreadScheduler);

        if (gameRenderResolutionService.TryGetGameResolution(out var resolution))
        {
            GameRenderResolution = resolution;
        }
        var resolutionsSource = resolutionService
            .Connect()
            .Sort(
                SortExpressionComparer<Resolution>
                    .Ascending(x => x.Width)
                    .ThenByAscending(x => x.Height)
            )
            .Transform(r => new ResolutionProxy(r, GameRenderResolution!))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out _resolutions)
            .Subscribe();
        if (GameRenderResolution is not null)
        {
            SelectedResolution = Resolutions.FirstOrDefault(r =>
                r.Width == GameRenderResolution!.Width
            );
        }
        else
        {
            SelectedResolution = Resolutions.FirstOrDefault();
        }

        if (findDxhrdcExeService.TryFind(out var path))
        {
            BrowseTextBox = path;
        }

        this.WhenAnyValue(x => x.BrowseTextBox).Subscribe(x => _appState.GameExePath = x);
    }

    private void Uninstall()
    {
        if (string.IsNullOrWhiteSpace(BrowseTextBox))
        {
            throw new NullReferenceException(nameof(BrowseTextBox));
        }
        var pathToBackup = BrowseTextBox + ".bak";
        var destination = BrowseTextBox;
        _uninstallService.Uninstall(pathToBackup, destination);
        ModalText = "Uninstalled!";
        ModalOpen = true;
    }

    private void Patch()
    {
        if (string.IsNullOrWhiteSpace(BrowseTextBox) || SelectedResolution is null)
        {
            throw new NullReferenceException(
                $"{nameof(BrowseTextBox)} or {nameof(SelectedResolution)} null"
            );
        }

        var backupPath = BrowseTextBox + ".bak";
        _backupDxhrdcService.Backup(BrowseTextBox, backupPath);
        _uiScalePatchService.Patch(BrowseTextBox, SelectedResolution.Width);
        ModalText = "Patched! You may close this application.";
        ModalOpen = true;
    }

    public string? ModalText { get; set; }

    private async Task ShowOpenFileDialogAsync()
    {
        var storageFile = await BrowseInteraction.Handle(Unit.Default);
        if (storageFile is not null)
        {
            BrowseTextBox = storageFile.Path.LocalPath;
        }
    }

    private async Task ShowAddCustomResolutionDialogAsync()
    {
        var resolution = await AddResolutionInteraction.Handle(Unit.Default);
        if (resolution is not null)
        {
            var result = _resolutionService.AddResolution(resolution);
            SelectedResolution = Resolutions.FirstOrDefault(r => r.Id == result.Id);
        }
    }

    public string? BrowseTextBox
    {
        get => _browseTextBox;
        set => this.RaiseAndSetIfChanged(ref _browseTextBox, value);
    }

    public string ResolutionComboToolTip =>
        GameRenderResolution is null
            ? $"Scale width | % of game's default scale width (1280) | % of game render width (???)\n\nScaling tops out at 100% of the game's render width"
            : $"Scale width | % of game's default scale width (1280) | % of game render width ({GameRenderResolution!.Width})\n\nScaling tops out at 100% of the game's render width";

    public ResolutionProxy? SelectedResolution
    {
        get => _selectedResolution;
        set => this.RaiseAndSetIfChanged(ref _selectedResolution, value);
    }

    public Resolution? GameRenderResolution { get; }

    public bool CanPatch => _canPatch?.Value ?? false;
    public bool PatchCmdIsExecuting => _patchCmdIsExecuting?.Value ?? false;
    public bool CanUninstall => _canUninstall?.Value ?? false;
    public bool UninstallCmdIsExecuting => _uninstallCmdIsExecuting?.Value ?? false;

    public bool ModalOpen
    {
        get => _modalOpen;
        set => this.RaiseAndSetIfChanged(ref _modalOpen, value);
    }

    public ReadOnlyObservableCollection<ResolutionProxy> Resolutions => _resolutions;

    public ReactiveCommand<Unit, Unit> AddResolutionCmd { get; }
    public ReactiveCommand<Unit, Unit> BrowseCmd { get; }
    public ReactiveCommand<Unit, Unit> PatchCmd { get; }
    public ReactiveCommand<Unit, Unit> UninstallCmd { get; }
    public Interaction<Unit, Resolution?> AddResolutionInteraction { get; }
    public Interaction<Unit, IStorageFile?> BrowseInteraction { get; }
    public string? UrlPathSegment { get; } = Guid.NewGuid().ToString()[..5];
    public IScreen HostScreen { get; set; }
}
