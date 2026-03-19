using Microsoft.UI.Xaml;

namespace HCG.FondoRevolvente.Client;

public sealed partial class App : Application
{
    private Window? _window;

    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new Window();
        _window.Content = new Views.Module04_SolicitudList.SolicitudListPage();
        _window.Activate();
    }
}