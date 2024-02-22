using System;
using System.Reactive;
using System.Reactive.Linq;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Validations;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public class AddResolutionDialogViewModel : ViewModelBase
{
    private string? _width;
    private readonly ObservableAsPropertyHelper<string> _multiplier;

    public AddResolutionDialogViewModel()
    {
        var addCmdCanExecute = this.WhenAnyValue(
            x => x.Width,
            selector: widthStr =>
            {
                if (uint.TryParse(widthStr, out var width))
                {
                    return width is > 0 and < 65535;
                }

                return false;
            }
        );
        AddCmd = ReactiveCommand.Create(
            () => new Resolution(uint.Parse(Width!), 0),
            addCmdCanExecute
        );
        _multiplier = this.WhenAnyValue(x => x.Width)
            .Select(x =>
            {
                if (string.IsNullOrWhiteSpace(x))
                    return "";
                return uint.TryParse(x, out var parsed)
                    ? $"{Math.Round(parsed / (double)1280 * 100, 2)}%"
                    : "?.?%";
            })
            .ToProperty(this, x => x.Multiplier);
    }

    [ResolutionValidation]
    public string? Width
    {
        get => _width;
        set => this.RaiseAndSetIfChanged(ref _width, value);
    }

    public string Multiplier => _multiplier.Value;

    public ReactiveCommand<Unit, Resolution> AddCmd { get; }
}
