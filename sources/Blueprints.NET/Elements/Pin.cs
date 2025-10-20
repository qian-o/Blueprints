using SkiaSharp;

namespace Blueprints.NET;

public class Pin : Element
{
    private readonly HashSet<Connection> connections = [];

    private PreviewConnection? previewConnection;

    public Pin()
    {
        Cursor = Cursor.Cross;
        CanDrag = true;
        CanDrop = true;
    }

    public Pin(EventHandler<ConnectionValidationEventArgs> connectionValidating) : this()
    {
        ConnectionValidating += connectionValidating;
    }

    public PinShape Shape { get; set => Set(ref field, value, false); }

    public Drawable? Content { get; set => Set(ref field, value, true); }

    public PinDirection Direction { get; private set; }

    public SKPoint ConnectionPoint { get; private set; }

    public int MaxConnections { get; set; }

    public IEnumerable<Connection> Connections => connections;

    public IEnumerable<Connection> OutgoingConnections => connections.Where(item => item.Source == this);

    public bool AllowsMultipleConnections => MaxConnections is 0;

    public bool IsConnectionLimitReached => MaxConnections > 1 && connections.Count >= MaxConnections;

    public event EventHandler<ConnectionValidationEventArgs>? ConnectionValidating;

    public bool CanConnectTo(Pin other)
    {
        if (other == this)
        {
            return false;
        }

        if (Parent == other.Parent)
        {
            return false;
        }

        if (Direction == other.Direction)
        {
            return false;
        }

        if (connections.Any(item => item.Connects(this, other)))
        {
            return false;
        }

        if (IsConnectionLimitReached || other.IsConnectionLimitReached)
        {
            return false;
        }

        ConnectionValidationEventArgs args = new(this, other);
        ConnectionValidating?.Invoke(this, args);

        return !args.Cancel;
    }

    public void ConnectTo(Pin other)
    {
        if (!CanConnectTo(other))
        {
            return;
        }

        if (MaxConnections == 1 && connections.Count > 0)
        {
            DisconnectAll();
        }

        if (other.MaxConnections == 1 && other.connections.Count > 0)
        {
            other.DisconnectAll();
        }

        Connection connection = new(this, other);

        connections.Add(connection);
        other.connections.Add(connection);

        Invalidate(true);
    }

    public void DisconnectFrom(Pin other)
    {
        Connection? connection = connections.FirstOrDefault(item => item.Connects(this, other));

        if (connection is not null)
        {
            connections.Remove(connection);
            other.connections.Remove(connection);
        }

        Invalidate(true);
    }

    public void DisconnectAll()
    {
        foreach (Connection connection in connections.ToArray())
        {
            connection.Disconnect();
        }

        Invalidate(true);
    }

    protected override IEnumerable<Element> SubElements(bool includeConnections = true)
    {
        if (Content is not null)
        {
            yield return Content;
        }

        if (includeConnections)
        {
            foreach (Connection connection in OutgoingConnections)
            {
                yield return connection;
            }
        }
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
        const float spacing = 8;

        float contentWidth = Theme.PinShapeSize;
        float contentHeight = Theme.PinShapeSize;

        if (Content is not null)
        {
            contentWidth += Content.Size.Width + spacing;
            contentHeight = Math.Max(contentHeight, Content.Size.Height);
        }

        return new((Theme.PinPadding * 2) + contentWidth, (Theme.PinPadding * 2) + contentHeight);
    }

    protected override void OnArrange()
    {
        const float spacing = 8;

        float left = Bounds.Left + Theme.PinPadding;
        float right = Bounds.Right - Theme.PinPadding - Theme.PinShapeSize;

        switch (Direction)
        {
            case PinDirection.Input:
                ConnectionPoint = new SKPoint(left + (Theme.PinShapeSize / 2), Bounds.MidY);
                Content?.Position = new SKPoint(left + Theme.PinShapeSize + spacing, Bounds.MidY - (Content.Size.Height / 2));
                break;

            case PinDirection.Output:
                ConnectionPoint = new SKPoint(right + (Theme.PinShapeSize / 2), Bounds.MidY);
                Content?.Position = new SKPoint(right - spacing - Content.Size.Width, Bounds.MidY - (Content.Size.Height / 2));
                break;
        }
    }

    protected override void OnRender(IDrawingContext dc)
    {
        if (IsPointerOver)
        {
            dc.DrawRectangle(Bounds, 4.0f, Theme.PinHoverColor);
        }

        float left = Bounds.Left + Theme.PinPadding;
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

        bool isFilled = Shape is PinShape.FilledCircle or PinShape.FilledSquare or PinShape.FilledTriangle
                        || connections.Any(static item => item is not PreviewConnection);

        switch (Shape)
        {
            case PinShape.Circle or PinShape.FilledCircle:
                {
                    dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinShapeSize / 2, Theme.PinColor, Theme.PinShapeStrokeWidth);

                    if (isFilled)
                    {
                        dc.DrawCircle(new(rect.MidX, rect.MidY), Theme.PinShapeSize / 2, Theme.PinColor);
                    }
                }
                break;

            case PinShape.Triangle or PinShape.FilledTriangle:
                {
                    using SKPath path = new();
                    path.MoveTo(rect.Left, rect.Top);
                    path.LineTo(rect.Left + MathF.Sqrt(MathF.Pow(rect.Width, 2) - MathF.Pow(rect.Width / 2, 2)), rect.MidY);
                    path.LineTo(rect.Left, rect.Bottom);
                    path.Close();

                    dc.DrawPath(path, Theme.PinColor, Theme.PinShapeStrokeWidth);

                    if (isFilled)
                    {
                        dc.DrawPath(path, Theme.PinColor);
                    }
                }
                break;

            case PinShape.Square or PinShape.FilledSquare:
                {
                    dc.DrawRectangle(rect, 0.0f, Theme.PinColor, Theme.PinShapeStrokeWidth);

                    if (isFilled)
                    {
                        dc.DrawRectangle(rect, 0.0f, Theme.PinColor);
                    }
                }
                break;
        }
    }

    protected override void OnPointerPressed(PointerEventArgs args)
    {
        if (args.Pointers is Pointers.LeftButton && args.Modifiers is Modifiers.Control or Modifiers.Menu)
        {
            if (args.Modifiers is Modifiers.Control)
            {
                Pin[] pins = [.. connections.Select(item => item.Source == this ? item.Target : item.Source)];

                if (pins.Length is 0)
                {
                    return;
                }

                previewConnection = new(this, pins, Direction)
                {
                    PreviewPoint = args.WorldPosition
                };
            }

            DisconnectAll();
        }
    }

    protected override void OnPointerReleased(PointerEventArgs args)
    {
        previewConnection = null;
    }

    protected override void OnDragStarted(DragEventArgs args)
    {
        previewConnection ??= new(this, [], Direction is PinDirection.Input ? PinDirection.Output : PinDirection.Input)
        {
            PreviewPoint = args.WorldPosition
        };

        connections.Add(previewConnection);

        args.Data = previewConnection;

        Invalidate(true);
    }

    protected override void OnDragDelta(DragEventArgs args)
    {
        (args.Data as PreviewConnection)?.PreviewPoint = args.WorldPosition;
    }

    protected override void OnDragOver(DragEventArgs args)
    {
        (args.Data as PreviewConnection)?.PreviewPoint = ConnectionPoint;
    }

    protected override void OnDrop(DragEventArgs args)
    {
        if (args.Data is not PreviewConnection connection)
        {
            return;
        }

        if (connection.SourcePins.Length is 0)
        {
            connection.Source.ConnectTo(this);
        }
        else
        {
            foreach (Pin pin in connection.SourcePins)
            {
                pin.ConnectTo(this);
            }
        }

        args.Handled = true;
    }

    protected override void OnDragCompleted(DragEventArgs args)
    {
        connections.Remove((PreviewConnection)args.Data!);

        previewConnection = null;
    }

    protected override void OnDragCancelled(DragEventArgs args)
    {
        connections.Remove((PreviewConnection)args.Data!);

        previewConnection = null;
    }
}