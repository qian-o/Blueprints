using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SkiaSharp;
using SkiaSharp.Views.Windows;

namespace Blueprints.WinUI;

public class BlueprintStyle : IBlueprintStyle
{
    public string FontFamily { get; } = "Segoe UI";

    public SKColor TextColor { get; } = ((SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"]).Color.ToSKColor();

    public SKColor HoverColor { get; } = ((SolidColorBrush)Application.Current.Resources["SubtleFillColorSecondaryBrush"]).Color.ToSKColor();

    public SKColor PressedColor { get; } = ((SolidColorBrush)Application.Current.Resources["SubtleFillColorTertiaryBrush"]).Color.ToSKColor();

    public SKColor AttentionColor { get; } = ((SolidColorBrush)Application.Current.Resources["SystemFillColorAttentionBrush"]).Color.ToSKColor();

    public SKColor MinorGridLineColor { get; } = ((SolidColorBrush)Application.Current.Resources["DividerStrokeColorDefaultBrush"]).Color.ToSKColor();

    public SKColor MajorGridLineColor { get; } = ((SolidColorBrush)Application.Current.Resources["ControlStrongStrokeColorDefaultBrush"]).Color.ToSKColor();
}
