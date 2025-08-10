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

    public virtual void PointerEntered(PointerEventArgs args) { }

    public virtual void PointerExited(PointerEventArgs args) { }

    public virtual void PointerPressed(PointerEventArgs args) { }

    public virtual void PointerMoved(PointerEventArgs args) { }

    public virtual void PointerReleased(PointerEventArgs args) { }

    public virtual void PointerWheelChanged(PointerWheelEventArgs args) { }
}
