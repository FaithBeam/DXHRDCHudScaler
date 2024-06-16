using ReactiveUI;

namespace DXHRDCHudScaler.ViewModels;

public interface IExtrasTabViewModel : IRoutableViewModel
{
    uint Fov { get; set; }
}
