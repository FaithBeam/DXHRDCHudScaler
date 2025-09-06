namespace DXHRDCHudScaler.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel(MainTabViewModel mainTabViewModel)
    {
        MainTabViewModel = mainTabViewModel;
    }

    public MainTabViewModel MainTabViewModel { get; }
}
