using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using SkiaSharp.Views.Windows;
using Windows.Foundation;

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

    private Point lastPointerPosition;

    public BlueprintEditor()
    {
        PointerPressed += OnPointerPressed;
        PointerMoved += OnPointerMoved;
        PointerWheelChanged += OnPointerWheelChanged;

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

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint pointerPoint = e.GetCurrentPoint(this);

        if (pointerPoint.Properties.IsRightButtonPressed)
        {
            lastPointerPosition = pointerPoint.Position;
        }
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint pointerPoint = e.GetCurrentPoint(this);

        if (pointerPoint.Properties.IsRightButtonPressed)
        {
            Point currentPosition = pointerPoint.Position;

            double deltaX = currentPosition.X - lastPointerPosition.X;
            double deltaY = currentPosition.Y - lastPointerPosition.Y;

            X += deltaX / Dpi;
            Y += deltaY / Dpi;

            lastPointerPosition = currentPosition;

            Invalidate();
        }
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        PointerPoint pointerPoint = e.GetCurrentPoint(this);

        double delta = pointerPoint.Properties.MouseWheelDelta > 0 ? 0.1 : -0.1;

        Zoom = Math.Max(0.1, Zoom + delta);

        Invalidate();
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        BlueprintRenderer.Render(this, e.Surface.Canvas);
    }
}
