using System;
using DXHRDCHudScaler.Core.Services;
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
        services.AddTransient<IPatchService, PatchService>();
        services.AddTransient<IUninstallService, UninstallService>();
        services.AddTransient<IBackupDxhrdcService, BackupDxhrdcService>();
    }

    private static void RegisterPlatformSpecificServices(IServiceCollection services)
    {
        if (OperatingSystem.IsWindows())
        {
            services.AddTransient<IResolutionService, ResolutionService>();
            services.AddTransient<IFovService, FovService>();
            services.AddTransient<IGetGameRenderResolutionService, GetGameRenderResolutionServiceService>();
            services.AddTransient<IFindDxhrdcExeService, FindDxhrdcExeService>();
        }
        else if (OperatingSystem.IsMacOS()) { }
        else if (OperatingSystem.IsLinux()) { }
        else
        {
            throw new InvalidOperationException("Unknown platform");
        }
    }
}
