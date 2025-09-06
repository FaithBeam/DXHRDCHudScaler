using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Dialogs;
using DXHRDCHudScaler.ViewModels;
using ReactiveUI;

namespace DXHRDCHudScaler.Tabs;

public partial class MainTabView : ReactiveUserControl<MainTabViewModel>
{
    private TopLevel? _topLevel;
    private Window? _window;

    public MainTabView()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            if (ViewModel == null)
            {
                return;
            }
            _topLevel = TopLevel.GetTopLevel(this);
            _window = (Window)_topLevel! ?? throw new Exception("Unable to get window for MainTab");
            d(ViewModel.BrowseInteraction.RegisterHandler(ShowOpenFileDialogAsync));
            d(
                ViewModel.AddResolutionInteraction.RegisterHandler(
                    ShowAddCustomResolutionDialogAsync
                )
            );
        });
    }

    private async Task ShowAddCustomResolutionDialogAsync(
        IInteractionContext<Unit, Resolution?> arg
    )
    {
        var dialog = new AddResolutionDialog { DataContext = new AddResolutionDialogViewModel() };
        var result = await dialog.ShowDialog<Resolution?>(_window!);
        arg.SetOutput(result);
    }

    private static readonly string[] Options = ["DXHRDC.exe"];

    private async Task ShowOpenFileDialogAsync(IInteractionContext<Unit, IStorageFile?> arg)
    {
        if (_topLevel is not null)
        {
            var fileNames = await _topLevel.StorageProvider.OpenFilePickerAsync(
                new FilePickerOpenOptions
                {
                    Title = "Select DXHRDC.exe",
                    AllowMultiple = false,
                    FileTypeFilter =
                    [
                        new FilePickerFileType("DXHRDC.exe") { Patterns = Options },
                        new FilePickerFileType("dxhr.exe") { Patterns = ["dxhr.exe"] },
                    ],
                }
            );
            arg.SetOutput(fileNames.Any() ? fileNames[0] : null);
        }
    }
}
