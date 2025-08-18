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
        float width = Theme.PinSize;
        float height = Theme.PinSize;

        if (Content is not null)
        {
            width = Theme.PinSize + Content.Size.Width + 6;
            height = Math.Max(Theme.PinSize, Content.Size.Height);
        }

        return new(width, height);
    }

    protected override void OnArrange()
    {
        if (Content is not null)
        {
            switch (Direction)
            {
                case PinDirection.Input:
                    Content.Position = new SKPoint(Bounds.Left + Theme.PinSize + 6, Bounds.MidY - (Content.Size.Height / 2));
                    break;
                case PinDirection.Output:
                    Content.Position = new SKPoint(Bounds.Right - Theme.PinSize - 6 - Content.Size.Width, Bounds.MidY - (Content.Size.Height / 2));
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

        SKRect rect = SKRect.Empty;

        switch (Direction)
        {
            case PinDirection.Input:
                rect = SKRect.Create(Bounds.Left, Bounds.MidY - (Theme.PinSize / 2), Theme.PinSize, Theme.PinSize);
                break;
            case PinDirection.Output:
                rect = SKRect.Create(Bounds.Right - Theme.PinSize, Bounds.MidY - (Theme.PinSize / 2), Theme.PinSize, Theme.PinSize);
                break;
        }

        switch (Shape)
        {
            case PinShape.Circle:
                dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinSize / 2, Color ?? Theme.PinColor, 1.0f);
                break;
            case PinShape.FilledCircle:
                dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinSize / 2, Color ?? Theme.PinColor);
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
