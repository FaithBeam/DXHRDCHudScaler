using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Models;
using DXHRDCHudScaler.Windows;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public class ExtrasTabViewModel : ViewModelBase, IExtrasTabViewModel
{
    private bool _applyBtnEnabled;
    private bool _fovSliderEnabled;
    private readonly IFovService _fovService;
    private uint _fov;
    private SourceCache<Job, string> _jobCache = new(x => x.Name);

    public ExtrasTabViewModel(IScreen screen, IFovService fovService)
    {
        HostScreen = screen;
        _fovService = fovService;

        uint initialFov;
        if (_fovService.TryGetCurrentFov(out var foundFov))
        {
            initialFov = foundFov;
            FovSliderEnabled = true;
        }
        else
        {
            initialFov = 75;
        }
        Fov = initialFov;
        ResetFovCmd = ReactiveCommand.Create(() =>
        {
            Fov = 75;
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

            initialFov = Fov;
            _jobCache.Clear();
        });
        this.WhenAnyValue(x => x.Fov)
            .Throttle(TimeSpan.FromSeconds(0.25))
            .Subscribe(x =>
            {
                if (x == initialFov)
                {
                    _jobCache.Remove("SetFov");
                }
                else
                {
                    _jobCache.AddOrUpdate(new Job("SetFov", () => fovService.SetFov(x)));
                }
            });
    }

    public bool ApplyBtnEnabled
    {
        get => _applyBtnEnabled;
        set => this.RaiseAndSetIfChanged(ref _applyBtnEnabled, value);
    }

    public bool FovSliderEnabled
    {
        get => _fovSliderEnabled;
        set => this.RaiseAndSetIfChanged(ref _fovSliderEnabled, value);
    }

    public uint Fov
    {
        get => _fov;
        set => this.RaiseAndSetIfChanged(ref _fov, value);
    }

    public IEnumerable<int> Ticks => Enumerable.Range(1, 120);

    public ReactiveCommand<Unit, Unit> ResetFovCmd { get; }
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
