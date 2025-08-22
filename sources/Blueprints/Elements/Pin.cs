using SkiaSharp;

namespace Blueprints;

public class Pin : Element
{
    private readonly HashSet<Connection> connections = [];

    public PinShape Shape { get; set => Set(ref field, value, false); }

    public Drawable? Content { get; set => Set(ref field, value, true); }

    public SKColor? Color { get; set => Set(ref field, value, false); }

    public PinDirection Direction { get; private set; }

    public SKPoint ConnectionPoint { get; private set; }

    public int MaxConnections { get; set; }

    public IReadOnlyCollection<Connection> Connections => connections;

    public bool AllowsMultipleConnections => MaxConnections is 0;

    public bool IsConnectionLimitReached => MaxConnections > 1 && connections.Count >= MaxConnections;

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

        if (IsConnectedTo(other))
        {
            return false;
        }

        if (IsConnectionLimitReached || other.IsConnectionLimitReached)
        {
            return false;
        }

        return true;
    }

    public bool IsConnectedTo(Pin other)
    {
        return connections.Any(item => item.Connects(this, other));
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
    }

    public void DisconnectFrom(Pin other)
    {
        Connection? connection = connections.FirstOrDefault(item => item.Connects(this, other));

        if (connection is not null)
        {
            connections.Remove(connection);
            other.connections.Remove(connection);
        }
    }

    public void DisconnectAll()
    {
        foreach (Connection connection in connections.ToArray())
        {
            connection.Disconnect();
        }
    }

    protected override Element[] SubElements()
    {
        return Content is not null ? [Content] : [];
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
        float left = Bounds.Left + Theme.PinPadding;
        float right = Bounds.Right - Theme.PinPadding - Theme.PinShapeSize;

        switch (Direction)
        {
            case PinDirection.Input:
                ConnectionPoint = new SKPoint(left + (Theme.PinShapeSize / 2), Bounds.MidY);
                Content?.Position = new SKPoint(left + Theme.PinShapeSize + Theme.PinPadding, Bounds.MidY - (Content.Size.Height / 2));
                break;
            case PinDirection.Output:
                ConnectionPoint = new SKPoint(right + (Theme.PinShapeSize / 2), Bounds.MidY);
                Content?.Position = new SKPoint(right - Theme.PinPadding - Content.Size.Width, Bounds.MidY - (Content.Size.Height / 2));
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
}
