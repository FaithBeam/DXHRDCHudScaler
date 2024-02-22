using System.Reactive;
using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public interface IMainWindowViewModel : IScreen
{
    ReactiveCommand<Unit, IRoutableViewModel> GoToExtras { get; }
    ReactiveCommand<Unit, IRoutableViewModel> GoToMain { get; }
}