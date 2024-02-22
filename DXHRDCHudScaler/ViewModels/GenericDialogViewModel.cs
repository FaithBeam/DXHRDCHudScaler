namespace DXHRDCHudScaler.ViewModels;

public class GenericDialogViewModel(string windowTitle, string title, string message)
{
    public string? WindowTitle { get; } = windowTitle;
    public string? Title { get; } = title;
    public string? Message { get; } = message;
}
