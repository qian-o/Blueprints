using Example.Uno.ViewModels;
using Microsoft.UI.Xaml.Controls;

namespace Example.Uno.Views;

public sealed partial class MainView : Page
{
    public MainView()
    {
        InitializeComponent();

        ViewModel = new();
    }

    public MainViewModel ViewModel { get; }
}
