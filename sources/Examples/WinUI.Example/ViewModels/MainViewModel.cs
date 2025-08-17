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
            Content = "This is the content of Node 1.",
            Inputs = [new Pin { Header = "Input 1" }, new Pin { Header = "Input 2" }],
            Outputs = [new Pin { Header = "Output 1" }, new Pin { Header = "Output 2" }]
        };

        Node node2 = new()
        {
            Header = "Node 2",
            Content = "This is the content of Node 2.",
            Inputs = [new Pin { Header = "Input A" }],
            Outputs = [new Pin { Header = "Output A" }, new Pin { Header = "Output B" }]
        };

        Node node3 = new()
        {
            Header = "Node 3",
            Content = "This is the content of Node 3.",
            Inputs = [new Pin { Header = "Input X" }],
            Outputs = [new Pin { Header = "Output X" }, new Pin { Header = "Output Y" }]
        };

        Elements = [node1, node2, node3];
    }

    [ObservableProperty]
    public partial Element[] Elements { get; set; }
}
