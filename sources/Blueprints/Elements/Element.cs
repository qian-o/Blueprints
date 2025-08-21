using SkiaSharp;

namespace Blueprints;

public abstract class Element : IInputController, IDragDropController
{
    private readonly HashSet<Behavior> behaviors = [];

    private bool isLayouted;

    ~Element()
    {
        ClearBehaviors();
    }

    public IBlueprintEditor? Editor { get; private set; }

    public Element? Parent { get; private set; }

    public SKPoint Position { get; set => Set(ref field, value, true); } = SKPoint.Empty;

    public SKSize Size { get; private set; } = SKSize.Empty;

    public SKRect Bounds { get; private set; } = SKRect.Empty;

    public bool IsHitTestVisible { get; set; } = true;

    public bool IsPointerOver { get; private set => Set(ref field, value, false); }

    public bool IsDragged { get; private set => Set(ref field, value, false); }

    public IBlueprintTheme Theme => Editor?.Theme ?? throw new InvalidOperationException("Editor is not bound to this element.");

    public event EventHandler<PointerEventArgs>? PointerEntered;

    public event EventHandler<PointerEventArgs>? PointerExited;

    public event EventHandler<PointerEventArgs>? PointerPressed;

    public event EventHandler<PointerEventArgs>? PointerMoved;

    public event EventHandler<PointerEventArgs>? PointerReleased;

    public event EventHandler<PointerWheelEventArgs>? PointerWheelChanged;

    public event EventHandler<DragEventArgs>? DragStarted;

    public event EventHandler<DragEventArgs>? DragDelta;

    public event EventHandler<DragEventArgs>? DragOver;

    public event EventHandler<DragEventArgs>? Drop;

    public event EventHandler<DragEventArgs>? DragCompleted;

    public event EventHandler<DragEventArgs>? DragCancelled;

    public void UpdateLayout()
    {
        isLayouted = false;

        Parent?.UpdateLayout();
    }

    public void Invalidate()
    {
        Editor?.Invalidate();
    }

    public void AddBehavior(Behavior behavior)
    {
        if (behaviors.Add(behavior))
        {
            behavior.Attach(this);
        }
    }

    public void RemoveBehavior(Behavior behavior)
    {
        if (behaviors.Remove(behavior))
        {
            behavior.Detach(this);
        }
    }

    public void ClearBehaviors()
    {
        foreach (Behavior behavior in behaviors)
        {
            behavior.Detach(this);
        }

        behaviors.Clear();
    }

    public bool HitTest(SKPoint position)
    {
        return IsHitTestVisible && Bounds.Contains(position);
    }

    internal void Layout(IBlueprintEditor editor, IDrawingContext dc)
    {
        Bind(editor, null);

        if (isLayouted)
        {
            return;
        }

        Measure(dc);
        Arrange();

        isLayouted = true;
    }

    internal void Render(IDrawingContext dc)
    {
        if (Bounds.IsEmpty)
        {
            return;
        }

        OnRender(dc);

        foreach (Element element in SubElements())
        {
            element.Render(dc);
        }
    }

    protected void Set<T>(ref T field, T value, bool updateLayout)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;

            if (updateLayout)
            {
                UpdateLayout();
            }

            Invalidate();
        }
    }

    protected abstract Element[] SubElements();

    protected abstract void OnInitialize();

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnArrange();

    protected abstract void OnRender(IDrawingContext dc);

    #region InputController event handlers
    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerReleased(PointerEventArgs args) { }

    protected virtual void OnPointerWheelChanged(PointerWheelEventArgs args) { }
    #endregion

    #region DragDropController event handlers
    protected virtual void OnDragStarted(DragEventArgs args) { }

    protected virtual void OnDragDelta(DragEventArgs args) { }

    protected virtual void OnDragOver(DragEventArgs args) { }

    protected virtual void OnDrop(DragEventArgs args) { }

    protected virtual void OnDragCompleted(DragEventArgs args) { }

    protected virtual void OnDragCancelled(DragEventArgs args) { }
    #endregion

    private void Bind(IBlueprintEditor editor, Element? parent)
    {
        if (Editor == editor && Parent == parent)
        {
            return;
        }

        Editor = editor;
        Parent = parent;

        foreach (Element element in SubElements())
        {
            element.Bind(editor, this);
        }

        OnInitialize();
    }

    private void Measure(IDrawingContext dc)
    {
        foreach (Element element in SubElements())
        {
            element.Measure(dc);
        }

        Size = OnMeasure(dc);
    }

    private void Arrange()
    {
        Bounds = SKRect.Create(Position, Size);

        OnArrange();

        foreach (Element element in SubElements())
        {
            element.Arrange();
        }
    }

    #region IInputController implementation
    void IInputController.PointerEntered(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerEntered(args);
            }
        }

        IsPointerOver = true;

        OnPointerEntered(args);

        PointerEntered?.Invoke(this, args);
    }

    void IInputController.PointerExited(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.IsPointerOver)
            {
                ((IInputController)element).PointerExited(args);
            }
        }

        IsPointerOver = false;

        OnPointerExited(args);

        PointerExited?.Invoke(this, args);
    }

    void IInputController.PointerPressed(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerPressed(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerPressed(args);

        PointerPressed?.Invoke(this, args);
    }

    void IInputController.PointerMoved(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                if (element.IsPointerOver)
                {
                    ((IInputController)element).PointerMoved(args);

                    if (args.Handled)
                    {
                        return;
                    }

                    break;
                }
                else
                {
                    ((IInputController)element).PointerEntered(args);
                }
            }
            else if (element.IsPointerOver)
            {
                ((IInputController)element).PointerExited(args);
            }
        }

        OnPointerMoved(args);

        PointerMoved?.Invoke(this, args);
    }

    void IInputController.PointerReleased(PointerEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerReleased(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerReleased(args);

        PointerReleased?.Invoke(this, args);
    }

    void IInputController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IInputController)element).PointerWheelChanged(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnPointerWheelChanged(args);

        PointerWheelChanged?.Invoke(this, args);
    }
    #endregion

    #region IDragDropController implementation
    void IDragDropController.DragStarted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).DragStarted(args);

                return;
            }
        }

        IsDragged = true;

        OnDragStarted(args);

        DragStarted?.Invoke(this, args);
    }

    void IDragDropController.DragDelta(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.IsDragged)
            {
                ((IDragDropController)element).DragDelta(args);

                return;
            }
        }

        OnDragDelta(args);

        DragDelta?.Invoke(this, args);
    }

    void IDragDropController.DragOver(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).DragOver(args);

                break;
            }
        }

        OnDragOver(args);

        DragOver?.Invoke(this, args);
    }

    void IDragDropController.Drop(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.HitTest(args.WorldPosition))
            {
                ((IDragDropController)element).Drop(args);

                if (args.Handled)
                {
                    return;
                }

                break;
            }
        }

        OnDrop(args);

        Drop?.Invoke(this, args);
    }

    void IDragDropController.DragCompleted(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.IsDragged)
            {
                ((IDragDropController)element).DragCompleted(args);

                return;
            }
        }

        IsDragged = false;

        OnDragCompleted(args);

        DragCompleted?.Invoke(this, args);
    }

    void IDragDropController.DragCancelled(DragEventArgs args)
    {
        if (Editor == null)
        {
            throw new InvalidOperationException("Editor is not bound to this element.");
        }

        foreach (Element element in SubElements())
        {
            if (element.IsDragged)
            {
                ((IDragDropController)element).DragCancelled(args);

                return;
            }
        }

        IsDragged = false;

        OnDragCancelled(args);

        DragCancelled?.Invoke(this, args);
    }
    #endregion
}
