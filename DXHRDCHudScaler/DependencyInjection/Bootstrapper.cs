using Avalonia.ReactiveUI;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;

namespace DXHRDCHudScaler.DependencyInjection;

public static class Bootstrapper
{
    public static void Register(IServiceCollection services)
    {
        ServicesBootstrapper.RegisterServices(services);
        ViewModelBootstrapper.RegisterViewModels(services);
    }
}
