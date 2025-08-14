using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Node node1 = new()
        {
            Header = (Text)"Node 1",
            Content = (Text)"This is the content of Node 1."
        };

        Node node2 = new()
        {
            Header = (Text)"Node 2",
            Content = (Text)"This is the content of Node 2."
        };

        Node node3 = new()
        {
            Header = (Text)"Node 3",
            Content = (Text)"This is the content of Node 3."
        };

        Elements = [node1, node2, node3];
    }

    [ObservableProperty]
    public partial Element[] Elements { get; set; }
}
