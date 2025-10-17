using WinUI.Example.ViewModels;

namespace WinUI.Example.Views;

public sealed partial class MainView : Page
{
    public MainView()
    {
        InitializeComponent();

        ViewModel = new();
    }

    public MainViewModel ViewModel { get; }
}
