namespace DXHRDCHudScaler.ViewModels;

public class MainWindowViewModel(IMainTabViewModel mainTabViewModel, IExtrasTabViewModel extrasTabViewModel)
    : ViewModelBase,
        IMainWindowViewModel
{
    public IMainTabViewModel MainTabViewModel { get; } = mainTabViewModel;
    public IExtrasTabViewModel ExtrasTabViewModel { get; } = extrasTabViewModel;
}
