using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Node[] nodes = new Node[200];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = new()
            {
                Header = $"Node {i + 1}",
                Content = $"This is the content of Node {i + 1}.",
                Inputs = [new() { Shape = PinShape.Triangle, Content = $"Input {i + 1}A" }, new() { Content = $"Input {i + 1}B" }],
                Outputs = [new() { Shape = PinShape.Triangle, Content = $"Output {i + 1}A" }, new() { Content = $"Output {i + 1}B" }]
            };
        }

        Nodes = nodes;
    }

    [ObservableProperty]
    public partial IEnumerable<Element> Nodes { get; set; }

    [RelayCommand]
    private void ConnectNodes()
    {
        Node[] nodes = [.. Nodes.OfType<Node>()];

        for (int i = 0; i < nodes.Length; i++)
        {
            if (i + 1 == nodes.Length)
            {
                return;
            }

            nodes[i].Outputs[0].ConnectTo(nodes[i + 1].Inputs[0]);
        }
    }
}
