using Microsoft.UI.Input;
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
        BlueprintEditorController controller = new(this);

        PointerEntered += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerEntered(BlueprintPointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerEntered(BlueprintPointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerEntered(BlueprintPointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerEntered(BlueprintPointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerEntered(BlueprintPointer.XButton2, pointerPoint.Position.ToSKPoint());
            }
        };

        PointerExited += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerExited(BlueprintPointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerExited(BlueprintPointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerExited(BlueprintPointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerExited(BlueprintPointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerExited(BlueprintPointer.XButton2, pointerPoint.Position.ToSKPoint());
            }
        };

        PointerPressed += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerPressed(BlueprintPointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerPressed(BlueprintPointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerPressed(BlueprintPointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerPressed(BlueprintPointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerPressed(BlueprintPointer.XButton2, pointerPoint.Position.ToSKPoint());
            }
        };

        PointerMoved += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerMoved(pointerPoint.Position.ToSKPoint());
        };

        PointerReleased += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerReleased(pointerPoint.Position.ToSKPoint());
        };

        PointerWheelChanged += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerWheelChanged(pointerPoint.Position.ToSKPoint(), pointerPoint.Properties.MouseWheelDelta);
        };

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

    float IBlueprintEditor.Width => (float)ActualWidth;

    float IBlueprintEditor.Height => (float)ActualHeight;

    IBlueprintStyle IBlueprintEditor.Style { get; } = new BlueprintStyle();

    float IBlueprintEditor.X { get => (float)X; set => X = value; }

    float IBlueprintEditor.Y { get => (float)Y; set => Y = value; }

    float IBlueprintEditor.Zoom { get => (float)Zoom; set => Zoom = value; }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        BlueprintRenderer.Render(this, e.Surface.Canvas, (float)Dpi);
    }
}
