using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.ViewModels;
using ReactiveUI;

namespace DXHRDCHudScaler.Tabs;

public partial class ExtrasTabView : ReactiveUserControl<IExtrasTabViewModel>
{
    public ExtrasTabView()
    {
        this.WhenActivated(d =>
        {
            if (ViewModel == null)
            {
                return;
            }
        });
        InitializeComponent();
    }
}
