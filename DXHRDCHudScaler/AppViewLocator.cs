using System;
using DXHRDCHudScaler.Tabs;
using DXHRDCHudScaler.ViewModels;
using ReactiveUI;

namespace DXHRDCHudScaler;

public class AppViewLocator : IViewLocator
{
    public IViewFor ResolveView<T>(T? viewModel, string? contract = null) =>
        viewModel switch
        {
            MainTabViewModel context => new MainTabView { DataContext = context },
            // ExtrasTabViewModel context => new ExtrasTabView { DataContext = context },
            _ => throw new ArgumentOutOfRangeException(nameof(viewModel))
        };
}
