using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        Nodes = new Node[200];

        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i] = new()
            {
                Header = $"Node {i + 1}",
                Content = $"This is the content of Node {i + 1}.",
                Inputs = [new() { Shape = PinShape.Triangle, Content = $"Input {i + 1}A" }, new() { Content = $"Input {i + 1}B" }],
                Outputs = [new() { Shape = PinShape.Triangle, Content = $"Output {i + 1}A" }, new() { Content = $"Output {i + 1}B" }]
            };
        }
    }

    [ObservableProperty]
    public partial Node[] Nodes { get; set; }

    [RelayCommand]
    private void ConnectNodes()
    {
        for (int i = 0; i < Nodes.Length; i++)
        {
            Nodes[i].Outputs[0].ConnectTo(Nodes[(i + 1) % Nodes.Length].Inputs[0]);
        }
    }
}
