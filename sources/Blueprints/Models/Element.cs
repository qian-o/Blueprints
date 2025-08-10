using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;

namespace Blueprints;

public abstract partial class Element : ObservableObject, IController
{
    [ObservableProperty]
    public partial float Width { get; set; } = float.NaN;

    [ObservableProperty]
    public partial float Height { get; set; } = float.NaN;

    [ObservableProperty]
    public partial SKRect Bounds { get; protected set; }

    [ObservableProperty]
    public partial IBlueprintEditor? Editor { get; private set; }

    public SKRect ScreenBounds
    {
        get
        {
            if (Editor is null)
            {
                return SKRect.Empty;
            }

            return new SKRect(Editor.X + (Bounds.Left * Editor.Zoom),
                              Editor.Y + (Bounds.Top * Editor.Zoom),
                              Editor.X + (Bounds.Right * Editor.Zoom),
                              Editor.Y + (Bounds.Bottom * Editor.Zoom));
        }
    }

    public void Bind(IBlueprintEditor? editor)
    {
        Editor = editor;
    }

    public abstract void Layout(IDrawingContext dc, float offsetX, float offsetY);

    public abstract void Render(IDrawingContext dc);

    public void PointerEntered(PointerEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerEntered(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerEntered(args);
    }

    public void PointerExited(PointerEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerExited(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerExited(args);
    }

    public void PointerPressed(PointerEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerPressed(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerPressed(args);
    }

    public void PointerMoved(PointerEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerMoved(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerMoved(args);
    }

    public void PointerReleased(PointerEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerReleased(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerReleased(args);
    }

    public void PointerWheelChanged(PointerWheelEventArgs args)
    {
        foreach (var item in Children())
        {
            if (item.ScreenBounds.Contains(args.Position))
            {
                item.PointerWheelChanged(args);
            }
        }

        if (args.Handled)
        {
            return;
        }

        OnPointerWheelChanged(args);
    }

    protected virtual Element[] Children()
    {
        return [];
    }

    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }
}
