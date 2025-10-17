using Example.ViewModels;

namespace Example.Views;

public sealed partial class MainView : Page
{
    public MainView()
    {
        InitializeComponent();

        ViewModel = new();
    }

    public MainViewModel ViewModel { get; }
}
