using System;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.ViewModels;
using ReactiveUI;

namespace DXHRDCHudScaler.Dialogs;

public partial class AddResolutionDialog : ReactiveWindow<AddResolutionDialogViewModel>
{
    public AddResolutionDialog()
    {
        InitializeComponent();
        this.WhenActivated(d =>
        {
            if (ViewModel is null)
            {
                return;
            }

            d(ViewModel.AddCmd.Subscribe(Close));
        });
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e) => Close();
}
