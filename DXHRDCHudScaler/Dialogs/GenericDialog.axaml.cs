using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.ViewModels;

namespace DXHRDCHudScaler.Dialogs;

public partial class GenericDialog : ReactiveWindow<GenericDialogViewModel>
{
    public GenericDialog()
    {
        InitializeComponent();
    }

    private void OkBtn_OnClick(object? sender, RoutedEventArgs e) => Close();
}
