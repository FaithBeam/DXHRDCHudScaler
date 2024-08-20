using System;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Core.Services.BackupService;
// using DXHRDCHudScaler.Core.Services.SimplePatchService;
using DXHRDCHudScaler.Core.Services.UiScalePatchService;
using DXHRDCHudScaler.Core.Services.UninstallService;
using DXHRDCHudScaler.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace DXHRDCHudScaler.DependencyInjection;

public static class ServicesBootstrapper
{
    public static void RegisterServices(IServiceCollection services)
    {
        RegisterCommonServices(services);
        RegisterPlatformSpecificServices(services);
    }

    private static void RegisterCommonServices(IServiceCollection services)
    {
        services.AddSingleton<IAppState, AppState>();
        services.AddTransient<IUiScalePatchService, UiScalePatchService>();
        // services.AddTransient<ISimplePatchService, SimplePatchService>();
        services.AddTransient<IUninstallService, UninstallService>();
        services.AddTransient<IBackupDxhrdcService, BackupDxhrdcService>();
    }

    private static void RegisterPlatformSpecificServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindows() && OperatingSystem.IsWindowsVersionAtLeast(5))
        {
            services.AddTransient<IResolutionService, DXHRDCHudScaler.Windows.ResolutionService>();
            services.AddTransient<
                IGetGameRenderResolutionService,
                GetGameRenderResolutionServiceService
            >();
            services.AddTransient<IFindDxhrdcExeService, FindDxhrdcExeService>();
        }
        else if (OperatingSystem.IsMacOS()) { }
        else if (OperatingSystem.IsLinux())
        {
            services.AddTransient<IResolutionService, DXHRDXHudScaler.Linux.ResolutionService>();
            services.AddTransient<
                IGetGameRenderResolutionService,
                DXHRDXHudScaler.Linux.GetGameRenderResolutionService
            >();
            services.AddTransient<
                IFindDxhrdcExeService,
                DXHRDXHudScaler.Linux.FindDXHRDCExeService
            >();
        }
        else
        {
            throw new InvalidOperationException("Unknown platform");
        }
    }
}
