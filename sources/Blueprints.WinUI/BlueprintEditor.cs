using Microsoft.UI.Xaml;
using SkiaSharp.Views.Windows;

namespace Blueprints.WinUI;

public sealed partial class BlueprintEditor : SKXamlCanvas, IBlueprintEditor
{
    public static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof(X),
                                                                                      typeof(double),
                                                                                      typeof(BlueprintEditor),
                                                                                      new PropertyMetadata(0.0));

    public static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof(Y),
                                                                                      typeof(double),
                                                                                      typeof(BlueprintEditor),
                                                                                      new PropertyMetadata(0.0));

    public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom),
                                                                                         typeof(double),
                                                                                         typeof(BlueprintEditor),
                                                                                         new PropertyMetadata(1.0));

    public BlueprintEditor()
    {
        PaintSurface += OnPaintSurface;
    }

    public double X
    {
        get => (double)GetValue(XProperty);
        set => SetValue(XProperty, value);
    }

    public double Y
    {
        get => (double)GetValue(YProperty);
        set => SetValue(YProperty, value);
    }

    public double Zoom
    {
        get => (double)GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    float IBlueprintEditor.Width => (float)(ActualWidth * Dpi);

    float IBlueprintEditor.Height => (float)(ActualHeight * Dpi);

    float IBlueprintEditor.X => (float)X;

    float IBlueprintEditor.Y => (float)Y;

    float IBlueprintEditor.Zoom => (float)Zoom;

    IBlueprintStyles IBlueprintEditor.Styles { get; } = new BlueprintStyles();

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        BlueprintRenderer.Render(this, e.Surface.Canvas);
    }
}
