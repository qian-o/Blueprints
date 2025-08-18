using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Node[] nodes = new Node[20];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new Node
            {
                Header = $"Node {i + 1}",
                Content = $"This is the content of Node {i + 1}.",
                Inputs = [new Pin { Shape = PinShape.Triangle, Content = $"Input {i + 1}A" }, new Pin { Content = $"Input {i + 1}B" }],
                Outputs = [new Pin { Shape = PinShape.Triangle, Content = $"Output {i + 1}A" }, new Pin { Content = $"Output {i + 1}B" }]
            };
        }

        Elements = nodes;
    }

    [ObservableProperty]
    public partial Element[] Elements { get; set; }
}
