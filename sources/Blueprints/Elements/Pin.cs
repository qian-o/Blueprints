using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    public PinShape Shape { get; set; }

    public Drawable? Content { get; set; }

    public SKColor? Color { get; set; }

    public PinDirection Direction { get; private set; }

    protected override Element[] SubElements()
    {
        return [];
    }

    protected override Drawable[] SubDrawables()
    {
        return Content is null ? [] : [Content];
    }

    protected override void OnInitialize()
    {
        if (Parent is not Node node)
        {
            return;
        }

        Direction = node.Inputs.Contains(this) ? PinDirection.Input : PinDirection.Output;
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        float contentWidth = Theme.PinShapeSize;
        float contentHeight = Theme.PinShapeSize;

        if (Content is not null)
        {
            contentWidth += Content.Size.Width + Theme.PinPadding;
            contentHeight = Math.Max(contentHeight, Content.Size.Height);
        }

        return new((Theme.PinPadding * 2) + contentWidth, (Theme.PinPadding * 2) + contentHeight);
    }

    protected override void OnArrange()
    {
        if (Content is not null)
        {
            float left = Bounds.Location.X + Theme.PinPadding;
            float right = Bounds.Right - Theme.PinPadding - Theme.PinShapeSize;

            switch (Direction)
            {
                case PinDirection.Input:
                    Content.Position = new SKPoint(left + Theme.PinPadding + Theme.PinShapeSize, Bounds.MidY - (Content.Size.Height / 2));
                    break;
                case PinDirection.Output:
                    Content.Position = new SKPoint(right - Theme.PinPadding - Content.Size.Width, Bounds.MidY - (Content.Size.Height / 2));
                    break;
            }
        }
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (IsPointerOver)
        {
            dc.DrawRectangle(Bounds, 4.0f, Theme.PinHoverColor);
        }

        float left = Bounds.Location.X + Theme.PinPadding;
        float right = Bounds.Right - Theme.PinPadding - Theme.PinShapeSize;

        SKRect rect = SKRect.Empty;

        switch (Direction)
        {
            case PinDirection.Input:
                rect = SKRect.Create(left, Bounds.MidY - (Theme.PinShapeSize / 2), Theme.PinShapeSize, Theme.PinShapeSize);
                break;
            case PinDirection.Output:
                rect = SKRect.Create(right, Bounds.MidY - (Theme.PinShapeSize / 2), Theme.PinShapeSize, Theme.PinShapeSize);
                break;
        }

        switch (Shape)
        {
            case PinShape.Circle:
                dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinShapeSize / 2, Color ?? Theme.PinColor, 1.0f);
                break;
            case PinShape.FilledCircle:
                dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinShapeSize / 2, Color ?? Theme.PinColor);
                break;
            case PinShape.Triangle:
                {
                    SKPath path = new();
                    path.MoveTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left + MathF.Sqrt(MathF.Pow(rect.Width, 2) - MathF.Pow(rect.Width / 2, 2)), rect.MidY);
                    path.LineTo(rect.Left, rect.Bottom);
                    path.Close();

                    dc.DrawPath(path, Color ?? Theme.PinColor, 1.0f);
                }
                break;
            case PinShape.FilledTriangle:
                {
                    SKPath path = new();
                    path.MoveTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left + MathF.Sqrt(MathF.Pow(rect.Width, 2) - MathF.Pow(rect.Width / 2, 2)), rect.MidY);
                    path.LineTo(rect.Left, rect.Bottom);
                    path.Close();

                    dc.DrawPath(path, Color ?? Theme.PinColor);
                }
                break;
            case PinShape.Square:
                dc.DrawRectangle(rect, 0.0f, Color ?? Theme.PinColor, 1.0f);
                break;
            case PinShape.FilledSquare:
                dc.DrawRectangle(rect, 0.0f, Color ?? Theme.PinColor);
                break;
        }
    }

    protected override void OnDragStarted(DragEventArgs args)
    {
    }

    protected override void OnDragDelta(DragEventArgs args)
    {
    }

    protected override void OnDragCancelled(DragEventArgs args)
    {
    }
}
