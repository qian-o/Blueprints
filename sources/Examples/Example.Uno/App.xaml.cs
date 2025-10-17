using Example.Uno.Views;

namespace Example.Uno;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        new Window() { Content = new MainView() }.Activate();
    }
}
