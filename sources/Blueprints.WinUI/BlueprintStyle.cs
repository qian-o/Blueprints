using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using SkiaSharp.Views.Windows;
using Windows.UI;

namespace Blueprints.WinUI;

public class BlueprintStyle : IBlueprintStyle
{
    public string FontFamily { get; } = "Segoe UI";

    public SKColor Foreground { get; } = SKColors.Black;

    public GridLine MinorGridLine { get; } = new(SKColors.LightGray, 1.0f, 20.0f);

    public GridLine MajorGridLine { get; } = new(SKColors.Gray, 2.0f, 120.0f);

    public SKColor NodeStroke { get; } = ((Color)Application.Current.Resources["CardStrokeColorDefault"]).ToSKColor();

    public SKColor NodeBackground { get; } = ((SolidColorBrush)Application.Current.Resources["SolidBackgroundFillColorBaseBrush"]).Color.ToSKColor();

    public Thickness NodePadding { get; } = 16.0f;

    public float NodeStrokeWidth { get; } = 1.0f;

    public float NodeCornerRadius { get; } = 8.0f;

    public SKColor Hover { get; } = ((SolidColorBrush)Application.Current.Resources["SubtleFillColorSecondaryBrush"]).Color.ToSKColor();

    public SKColor Pressed { get; } = ((SolidColorBrush)Application.Current.Resources["SubtleFillColorTertiaryBrush"]).Color.ToSKColor();

    public SKColor Attention { get; } = ((SolidColorBrush)Application.Current.Resources["SystemFillColorAttentionBrush"]).Color.ToSKColor();
}
