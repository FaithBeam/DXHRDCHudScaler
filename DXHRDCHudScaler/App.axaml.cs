using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.DependencyInjection;
using DXHRDCHudScaler.ViewModels;
using DXHRDCHudScaler.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReactiveUI;
using Splat;
using Splat.Microsoft.Extensions.DependencyInjection;

namespace DXHRDCHudScaler;

public partial class App : Application
{
    public IServiceProvider? Container { get; private set; }

    public override void Initialize()
    {
        Init();
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = Locator.Current.GetService<IMainWindowViewModel>(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void Init()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.UseMicrosoftDependencyResolver();
                var resolver = Locator.CurrentMutable;
                resolver.InitializeSplat();
                resolver.InitializeReactiveUI();

                Bootstrapper.Register(services);

                services.AddSingleton<
                    IActivationForViewFetcher,
                    AvaloniaActivationForViewFetcher
                >();
                services.AddSingleton<IPropertyBindingHook, AutoDataTemplateBindingHook>();
            })
            .Build();
        Container = host.Services;
        Container.UseMicrosoftDependencyResolver();
        RxApp.MainThreadScheduler = AvaloniaScheduler.Instance;
    }
}
