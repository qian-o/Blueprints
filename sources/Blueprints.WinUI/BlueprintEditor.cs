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
                                                                                          typeof(IEnumerable<Node>),
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

            controller.PointerEntered(Pointers(pointerPoint), pointerPoint.Position.ToSKPoint());
        };

        PointerExited += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerExited(Pointers(pointerPoint), pointerPoint.Position.ToSKPoint());
        };

        PointerPressed += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerPressed(Pointers(pointerPoint), pointerPoint.Position.ToSKPoint());
        };

        PointerMoved += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerMoved(Pointers(pointerPoint), pointerPoint.Position.ToSKPoint());
        };

        PointerReleased += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerReleased(Pointers(pointerPoint), pointerPoint.Position.ToSKPoint());
        };

        PointerWheelChanged += (_, e) =>
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this);

            controller.PointerWheelChanged(pointerPoint.Properties.MouseWheelDelta, pointerPoint.Position.ToSKPoint());
        };

        static PointerFlags Pointers(PointerPoint pointerPoint)
        {
            PointerFlags pointers = PointerFlags.None;

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                pointers |= PointerFlags.LeftButton;
            }

            if (pointerPoint.Properties.IsRightButtonPressed)
            {
                pointers |= PointerFlags.RightButton;
            }

            if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                pointers |= PointerFlags.MiddleButton;
            }

            if (pointerPoint.Properties.IsXButton1Pressed)
            {
                pointers |= PointerFlags.XButton1;
            }

            if (pointerPoint.Properties.IsXButton2Pressed)
            {
                pointers |= PointerFlags.XButton2;
            }

            return pointers;
        }
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

    public IEnumerable<Node>? Nodes
    {
        get => (IEnumerable<Node>)GetValue(NodesProperty);
        set => SetValue(NodesProperty, value);
    }

    float IBlueprintEditor.Width => (float)ActualWidth;

    float IBlueprintEditor.Height => (float)ActualHeight;

    IBlueprintStyle IBlueprintEditor.Style { get; } = new BlueprintStyle();

    float IBlueprintEditor.X { get => (float)X; set => X = value; }

    float IBlueprintEditor.Y { get => (float)Y; set => Y = value; }

    float IBlueprintEditor.Zoom { get => (float)Zoom; set => Zoom = value; }

    IEnumerable<Node> IBlueprintEditor.Nodes { get => Nodes ?? []; set => Nodes = value; }
}
