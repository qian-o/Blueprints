using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Node node1 = new()
        {
            Header = "Node 1",
            Content = "This is the content of Node 1."
        };

        Node node2 = new()
        {
            Header = "Node 2",
            Content = "This is the content of Node 2."
        };

        Node node3 = new()
        {
            Header = "Node 3",
            Content = "This is the content of Node 3."
        };

        Elements = [node1, node2, node3];
    }

    [ObservableProperty]
    public partial Element[] Elements { get; set; }
}
