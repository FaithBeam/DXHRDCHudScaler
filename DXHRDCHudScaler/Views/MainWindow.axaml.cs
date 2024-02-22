using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using DXHRDCHudScaler.Core.Models;
using DXHRDCHudScaler.Dialogs;
using DXHRDCHudScaler.ViewModels;
using ReactiveUI;

namespace DXHRDCHudScaler.Views;

public partial class MainWindow : ReactiveWindow<IMainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
    }
}
