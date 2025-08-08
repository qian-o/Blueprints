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

    public static readonly DependencyProperty NodesProperty = DependencyProperty.Register(nameof(Nodes),
                                                                                          typeof(IEnumerable<BlueprintNode>),
                                                                                          typeof(BlueprintEditor),
                                                                                          new PropertyMetadata(null));

    public BlueprintEditor()
    {
        BlueprintRenderer renderer = new(this);
        BlueprintController controller = new(this);

        PaintSurface += (_, e) => renderer.Render(e.Surface.Canvas, (float)Dpi);

        PointerEntered += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerEntered(Pointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerEntered(Pointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerEntered(Pointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerEntered(Pointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerEntered(Pointer.XButton2, pointerPoint.Position.ToSKPoint());
            }
        };

        PointerExited += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerExited(Pointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerExited(Pointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerExited(Pointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerExited(Pointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerExited(Pointer.XButton2, pointerPoint.Position.ToSKPoint());
            }
        };

        PointerPressed += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                controller.PointerPressed(Pointer.LeftButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsRightButtonPressed)
            {
                controller.PointerPressed(Pointer.RightButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                controller.PointerPressed(Pointer.MiddleButton, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton1Pressed)
            {
                controller.PointerPressed(Pointer.XButton1, pointerPoint.Position.ToSKPoint());
            }
            else if (pointerPoint.Properties.IsXButton2Pressed)
            {
                controller.PointerPressed(Pointer.XButton2, pointerPoint.Position.ToSKPoint());
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

    public IEnumerable<BlueprintNode>? Nodes
    {
        get => (IEnumerable<BlueprintNode>)GetValue(NodesProperty);
        set => SetValue(NodesProperty, value);
    }

    float IBlueprintEditor.Width => (float)ActualWidth;

    float IBlueprintEditor.Height => (float)ActualHeight;

    IBlueprintStyle IBlueprintEditor.Style { get; } = new BlueprintStyle();

    float IBlueprintEditor.X { get => (float)X; set => X = value; }

    float IBlueprintEditor.Y { get => (float)Y; set => Y = value; }

    float IBlueprintEditor.Zoom { get => (float)Zoom; set => Zoom = value; }

    IEnumerable<BlueprintNode> IBlueprintEditor.Nodes { get => Nodes ?? []; set => Nodes = value; }
}
