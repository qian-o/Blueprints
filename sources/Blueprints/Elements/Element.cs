using SkiaSharp;

namespace Blueprints;

public abstract class Element
{
    public IBlueprintEditor? Editor { get; private set; }

    public SKPoint Position { get; set; } = SKPoint.Empty;

    public SKSize Size { get; set; } = SKSize.Empty;

    public SKRect Bounds { get; set; } = SKRect.Empty;

    public IBlueprintStyle Style => Editor?.Style ?? throw new InvalidOperationException("Editor is not bound to this element.");

    internal void Bind(IBlueprintEditor editor)
    {
        Editor = editor;

        foreach (Element element in Children())
        {
            element.Bind(editor);
        }
    }

    internal void Layout(IDrawingContext dc)
    {
        foreach (Element element in Children())
        {
            element.Layout(dc);
        }

        Size = OnLayout(dc);
    }

    internal void Render(IDrawingContext dc)
    {
        Bounds = SKRect.Create(Position, Size);

        if (Bounds.IsEmpty)
        {
            return;
        }

        OnRender(dc);

        foreach (Element element in Children())
        {
            element.Render(dc);
        }
    }

    protected abstract Element[] Children();

    protected abstract SKSize OnLayout(IDrawingContext dc);

    protected abstract void OnRender(IDrawingContext dc);
}
