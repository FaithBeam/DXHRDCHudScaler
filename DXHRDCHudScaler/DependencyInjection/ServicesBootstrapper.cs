using System;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Core.Services;
using DXHRDCHudScaler.Core.Services.BackupService;
using DXHRDCHudScaler.Core.Services.FindDXHRDCExeService;
using DXHRDCHudScaler.Core.Services.GetGameRenderResolutionServiceService;
using DXHRDCHudScaler.Core.Services.ResolutionService;
// using DXHRDCHudScaler.Core.Services.SimplePatchService;
using DXHRDCHudScaler.Core.Services.UiScalePatchService;
using DXHRDCHudScaler.Core.Services.UninstallService;
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
        services.AddScoped<IUiScalePatchService, UiScalePatchService>();
        // services.AddScoped<ISimplePatchService, SimplePatchService>();
        services.AddScoped<IUninstallService, UninstallService>();
        services.AddScoped<IBackupDxhrdcService, BackupDxhrdcService>();
    }

    private static void RegisterPlatformSpecificServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindows() && OperatingSystem.IsWindowsVersionAtLeast(5))
        {
            services.AddScoped<IResolutionService, ResolutionServiceWin>();
            services.AddScoped<
                IGetGameRenderResolutionService,
                GetGameRenderResolutionServiceServiceWin
            >();
            services.AddScoped<IFindDxhrdcExeService, FindDxhrdcExeServiceWin>();
        }
        else if (OperatingSystem.IsMacOS()) { }
        else if (OperatingSystem.IsLinux())
        {
            services.AddScoped<IResolutionService, ResolutionServiceLinux>();
            services.AddScoped<
                IGetGameRenderResolutionService,
                GetGameRenderResolutionServiceLinux
            >();
            services.AddScoped<IFindDxhrdcExeService, FindDXHRDCExeServiceLinux>();
        }
        else
        {
            throw new InvalidOperationException("Unknown platform");
        }
    }
}
