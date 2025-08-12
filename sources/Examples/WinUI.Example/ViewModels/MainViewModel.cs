using Blueprints;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WinUI.Example.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    public partial Node[] Nodes { get; set; } = [new() { Title = (Text)"Test Node" }];
}
