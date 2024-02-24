using System.Reactive;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Windows;
using ReactiveUI;
using Splat;

namespace DXHRDCHudScaler.ViewModels;

public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    public MainWindowViewModel()
    {
        var cur = Locator.Current;
        var mainTabViewModel = new MainTabViewModel(
            this,
            cur.GetService<IResolutionService>()!,
            cur.GetService<IGetGameRenderResolutionService>()!,
            cur.GetService<IPatchService>()!,
            cur.GetService<IBackupDxhrdcService>()!,
            cur.GetService<IUninstallService>()!,
            cur.GetService<IFindDxhrdcExeService>()!
        );
        var extrasTabViewModel = new ExtrasTabViewModel(this, cur.GetService<IFovService>()!);
        Router.Navigate.Execute(mainTabViewModel);
        GoToMain = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(mainTabViewModel)
        );
        GoToExtras = ReactiveCommand.CreateFromObservable(
            () => Router.Navigate.Execute(extrasTabViewModel)
        );
    }

    public RoutingState Router { get; } = new();

    // The command that navigates a user to first view model.
    public ReactiveCommand<Unit, IRoutableViewModel> GoToExtras { get; }

    // The command that navigates a user back.
    public ReactiveCommand<Unit, IRoutableViewModel> GoToMain { get; }
}
