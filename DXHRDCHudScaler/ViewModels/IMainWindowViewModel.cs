namespace DXHRDCHudScaler.ViewModels;

public interface IMainWindowViewModel
{
    IMainTabViewModel MainTabViewModel { get; }
    IExtrasTabViewModel ExtrasTabViewModel { get; }
}