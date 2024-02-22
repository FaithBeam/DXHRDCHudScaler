using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Platform.Storage;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Models;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public interface IMainTabViewModel
{
    string? ModalText { get; set; }
    string? BrowseTextBox { get; set; }
    string ResolutionComboToolTip { get; }
    ResolutionProxy? SelectedResolution { get; set; }
    Resolution? GameRenderResolution { get; }
    bool CanPatch { get; }
    bool PatchCmdIsExecuting { get; }
    bool CanUninstall { get; }
    bool UninstallCmdIsExecuting { get; }
    bool ModalOpen { get; set; }
    ReadOnlyObservableCollection<ResolutionProxy> Resolutions { get; }
    ReactiveCommand<Unit, Unit> AddResolutionCmd { get; }
    ReactiveCommand<Unit, Unit> BrowseCmd { get; }
    ReactiveCommand<Unit, Unit> PatchCmd { get; }
    ReactiveCommand<Unit, Unit> UninstallCmd { get; }
    Interaction<Unit, Resolution?> AddResolutionInteraction { get; }
    Interaction<Unit, IStorageFile?> BrowseInteraction { get; }
    Interaction<GenericDialogViewModel, Unit> GenericDialogInteraction { get; }
}