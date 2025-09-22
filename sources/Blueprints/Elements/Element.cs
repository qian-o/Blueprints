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

    public Cursor Cursor { get; set; } = Cursor.Arrow;

    public bool CanDrag { get; set; }

    public bool CanDrop { get; set; }

    public bool IsDragging { get; private set => Set(ref field, value, false); }

    public IBlueprintTheme Theme => Editor?.Theme ?? DefaultBlueprintTheme.Instance;

    public object? Tag { get; set; }

    public event EventHandler<PointerEventArgs>? PointerEntered;

    public event EventHandler<PointerEventArgs>? PointerExited;

    public event EventHandler<PointerEventArgs>? PointerMoved;

    public event EventHandler<PointerEventArgs>? PointerPressed;

    public event EventHandler<PointerEventArgs>? PointerReleased;

    public event EventHandler<PointerWheelEventArgs>? PointerWheelChanged;

    public event EventHandler<DragEventArgs>? DragStarted;

    public event EventHandler<DragEventArgs>? DragDelta;

    public event EventHandler<DragEventArgs>? DragOver;

    public event EventHandler<DragEventArgs>? Drop;

    public event EventHandler<DragEventArgs>? DragCompleted;

    public event EventHandler<DragEventArgs>? DragCancelled;

    public void Invalidate(bool updateLayout)
    {
        if (updateLayout)
        {
            UpdateLayout();
        }

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

    public virtual bool HitTest(SKPoint position)
    {
        return IsHitTestVisible && Bounds.Contains(position);
    }

    internal void Layout(IBlueprintEditor editor, IDrawingContext dc)
    {
        Bind(editor, null);

        if (!isLayouted)
        {
            Measure(dc);
            Arrange();

            isLayouted = true;
        }
    }

    internal void Render(IDrawingContext dc)
    {
        if (Bounds.IsEmpty)
        {
            return;
        }

        OnRender(dc);

        foreach (Element element in SubElements(false))
        {
            element.Render(dc);
        }
    }

    internal Element? FindPointerOverElement()
    {
        foreach (Element element in SubElements())
        {
            if (element.FindPointerOverElement() is Element pointerOverElement)
            {
                return pointerOverElement;
            }
        }

        return IsPointerOver ? this : null;
    }

    protected void Set<T>(ref T field, T value, bool updateLayout)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;

            Invalidate(updateLayout);
        }
    }

    protected abstract Element[] SubElements(bool includeConnections = true);

    protected abstract void OnInitialize();

    protected abstract SKSize OnMeasure(IDrawingContext dc);

    protected abstract void OnArrange();

    protected abstract void OnRender(IDrawingContext dc);

    #region InputController event handlers
    protected virtual void OnPointerEntered(PointerEventArgs args) { }

    protected virtual void OnPointerExited(PointerEventArgs args) { }

    protected virtual void OnPointerMoved(PointerEventArgs args) { }

    protected virtual void OnPointerPressed(PointerEventArgs args) { }

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
        foreach (Element element in SubElements())
        {
            element.Bind(editor, this);
        }

        if (Editor == editor && Parent == parent)
        {
            return;
        }

        Editor = editor;
        Parent = parent;

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

    private void UpdateLayout()
    {
        isLayouted = false;

        Parent?.UpdateLayout();
    }

    #region IInputController implementation
    void IInputController.PointerMoved(PointerEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IInputController)element).PointerMoved(args);
        }

        if (args.Handled)
        {
            return;
        }

        bool isHit = HitTest(args.WorldPosition);

        if (isHit && !IsPointerOver)
        {
            IsPointerOver = true;

            OnPointerEntered(args);

            PointerEntered?.Invoke(this, args);
        }
        else if (!isHit && IsPointerOver)
        {
            IsPointerOver = false;

            OnPointerExited(args);

            PointerExited?.Invoke(this, args);
        }

        if (isHit)
        {
            args.HoveredElement ??= this;

            OnPointerMoved(args);

            PointerMoved?.Invoke(this, args);
        }
    }

    void IInputController.PointerPressed(PointerEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IInputController)element).PointerPressed(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (IsPointerOver)
        {
            OnPointerPressed(args);

            PointerPressed?.Invoke(this, args);
        }
    }

    void IInputController.PointerReleased(PointerEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IInputController)element).PointerReleased(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (IsPointerOver)
        {
            OnPointerReleased(args);

            PointerReleased?.Invoke(this, args);
        }
    }

    void IInputController.PointerWheelChanged(PointerWheelEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IInputController)element).PointerWheelChanged(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (IsPointerOver)
        {
            OnPointerWheelChanged(args);

            PointerWheelChanged?.Invoke(this, args);
        }
    }
    #endregion

    #region IDragDropController implementation
    void IDragDropController.DragStarted(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IDragDropController)element).DragStarted(args);
        }

        if (args.Element is not null)
        {
            return;
        }

        if (CanDrag && (IsDragging = IsPointerOver))
        {
            args.Element = this;

            OnDragStarted(args);

            DragStarted?.Invoke(this, args);
        }
    }

    void IDragDropController.DragDelta(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        OnDragDelta(args);

        DragDelta?.Invoke(this, args);
    }

    void IDragDropController.DragOver(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IDragDropController)element).DragOver(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (IsPointerOver && CanDrop && !IsDragging)
        {
            OnDragOver(args);

            DragOver?.Invoke(this, args);
        }
    }

    void IDragDropController.Drop(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        foreach (Element element in SubElements())
        {
            ((IDragDropController)element).Drop(args);
        }

        if (args.Handled)
        {
            return;
        }

        if (IsPointerOver && CanDrop && !IsDragging)
        {
            OnDrop(args);

            Drop?.Invoke(this, args);
        }
    }

    void IDragDropController.DragCompleted(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        IsDragging = false;

        OnDragCompleted(args);

        DragCompleted?.Invoke(this, args);
    }

    void IDragDropController.DragCancelled(DragEventArgs args)
    {
        if (Editor == null)
        {
            return;
        }

        IsDragging = false;

        OnDragCancelled(args);

        DragCancelled?.Invoke(this, args);
    }
    #endregion
}
