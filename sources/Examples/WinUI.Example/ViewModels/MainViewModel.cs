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
            Inputs = [new() { Content = "Input 1" }, new() { Content = "Input 2" }],
            Outputs = [new() { Content = "Output 1" }, new() { Content = "Output 2" }],
            Content = "This is the content of Node 1."
        };

        Node node2 = new()
        {
            Header = "Node 2",
            Inputs = [new() { Content = "Input A" }],
            Outputs = [new() { Content = "Output A" }, new() { Content = "Output B" }],
            Content = "This is the content of Node 2."
        };

        Node node3 = new()
        {
            Header = "Node 3",
            Inputs = [new() { Content = "Input X" }],
            Outputs = [new() { Content = "Output X" }, new() { Content = "Output Y" }],
            Content = "This is the content of Node 3."
        };

        Elements = [node1, node2, node3];
    }

    [ObservableProperty]
    public partial Element[] Elements { get; set; }
}
