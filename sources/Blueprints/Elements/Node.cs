using SkiaSharp;

namespace Blueprints;

public class Node : Element
{
    private SKPoint? pointerPressPosition;

    public float X { get; set; }

    public float Y { get; set; }

    public Element? Title { get; set; }

    public Element? Content { get; set; }

    public Pin[] Inputs { get; set; } = [];

    public Pin[] Outputs { get; set; } = [];

    public override void UseGlobalStyle()
    {
        base.UseGlobalStyle();

        MinWidth = 200;
        MinHeight = 100;
        Stroke = Style.NodeStroke;
        Background = Style.NodeBackground;
        Padding = Style.NodePadding;
        StrokeWidth = Style.NodeStrokeWidth;
        CornerRadius = Style.NodeCornerRadius;
    }

    protected override Element[] GetSubElements()
    {
        List<Element> children = [];

        if (Title is not null)
        {
            children.Add(Title);
        }

        if (Content is not null)
        {
            children.Add(Content);
        }

        children.AddRange(Inputs);
        children.AddRange(Outputs);

        return [.. children];
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        float width = Padding.Horizontal;
        float height = Padding.Vertical;

        if (Title is not null)
        {
            width += Title.Margin.Horizontal + Title.DesiredSize.Width;
            height += Title.Margin.Vertical + Title.DesiredSize.Height + Padding.Vertical;
        }

        return new(width, height);
    }

    protected override void OnArrange()
    {
        if (Title is not null)
        {
            float x = ContentBounds.Left + Padding.Left + Title.Margin.Left;
            float y = ContentBounds.Top + Padding.Top + Title.Margin.Top;

            Title.Arrange(SKRect.Create(x, y, Title.DesiredSize.Width, Title.DesiredSize.Height));
        }
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (Title is not null)
        {
            SKPoint start = new(ContentBounds.Left, ContentBounds.Top + Padding.Top + Title.DesiredSize.Height + Title.Margin.Vertical + Padding.Top);
            SKPoint end = new(ContentBounds.Right, ContentBounds.Top + Padding.Top + Title.DesiredSize.Height + Title.Margin.Vertical + Padding.Top);

            dc.DrawLine(start, end, Style.NodeStroke, Style.NodeStrokeWidth);
        }
    }

    protected override void OnPointerPressed(PointerEventArgs args)
    {
        if (args.Pointers.HasFlag(PointerFlags.LeftButton))
        {
            pointerPressPosition = args.Position - ScreenBounds.Location;
        }
    }

    protected override void OnPointerMoved(PointerEventArgs args)
    {
        if (pointerPressPosition is not null)
        {
            X = args.Position.X - Editor.X - pointerPressPosition.Value.X;
            Y = args.Position.Y - Editor.Y - pointerPressPosition.Value.Y;
            X /= Editor.Zoom;
            Y /= Editor.Zoom;

            Invalidate();
        }
    }

    protected override void OnPointerReleased(PointerEventArgs args)
    {
        pointerPressPosition = null;
    }
}
