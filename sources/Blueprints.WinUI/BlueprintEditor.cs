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

        PointerEntered += (_, e) => controller.PointerEntered(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerExited += (_, e) => controller.PointerExited(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerPressed += (_, e) => controller.PointerPressed(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerMoved += (_, e) => controller.PointerMoved(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerReleased += (_, e) => controller.PointerReleased(PointerEventArgs(e.GetCurrentPoint(this)));

        PointerWheelChanged += (_, e) => controller.PointerWheelChanged(PointerWheelEventArgs(e.GetCurrentPoint(this)));

        static PointerEventArgs PointerEventArgs(PointerPoint pointerPoint)
        {
            Pointers pointers = Pointers.None;

            if (pointerPoint.Properties.IsLeftButtonPressed)
            {
                pointers |= Pointers.LeftButton;
            }

            if (pointerPoint.Properties.IsRightButtonPressed)
            {
                pointers |= Pointers.RightButton;
            }

            if (pointerPoint.Properties.IsMiddleButtonPressed)
            {
                pointers |= Pointers.MiddleButton;
            }

            if (pointerPoint.Properties.IsXButton1Pressed)
            {
                pointers |= Pointers.XButton1;
            }

            if (pointerPoint.Properties.IsXButton2Pressed)
            {
                pointers |= Pointers.XButton2;
            }

            return new(pointerPoint.Position.ToSKPoint(), pointers);
        }

        static PointerWheelEventArgs PointerWheelEventArgs(PointerPoint pointerPoint)
        {
            return new(pointerPoint.Position.ToSKPoint(), pointerPoint.Properties.MouseWheelDelta);
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
