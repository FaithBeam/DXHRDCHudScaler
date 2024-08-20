using DXHRDCHudScaler.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace DXHRDCHudScaler.DependencyInjection;

public static class ViewModelBootstrapper
{
    public static void RegisterViewModels(IServiceCollection services)
    {
        services.AddSingleton<IMainTabViewModel, MainTabViewModel>();
        // services.AddSingleton<IExtrasTabViewModel, ExtrasTabViewModel>();
        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
    }
}
