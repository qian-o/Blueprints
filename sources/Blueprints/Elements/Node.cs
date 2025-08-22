using SkiaSharp;

namespace Blueprints;

public class Node : Element
{
    public Node()
    {
        AddBehavior(MoveBehavior.Instance);
    }

    public Drawable? Header { get; set => Set(ref field, value, true); }

    public Drawable? Content { get; set => Set(ref field, value, true); }

    public Pin[] Inputs { get; set => Set(ref field, value, true); } = [];

    public Pin[] Outputs { get; set => Set(ref field, value, true); } = [];

    public Connection[] Connections()
    {
        return [.. Inputs.Concat(Outputs).SelectMany(pin => pin.OutgoingConnections)];
    }

    protected override Element[] SubElements(bool includeConnections)
    {
        List<Element> elements = [];

        if (Header is not null)
        {
            elements.Add(Header);
        }

        if (Content is not null)
        {
            elements.Add(Content);
        }

        return [.. elements, .. Inputs, .. Outputs];
    }

    protected override void OnInitialize()
    {
    }

    protected override SKSize OnMeasure(IDrawingContext dc)
    {
        float contentWidth = 0;
        float contentHeight = 0;

        // Header
        if (Header is not null)
        {
            contentWidth = Header.Size.Width;
            contentHeight = Header.Size.Height + Theme.CardPadding;
        }

        // Pins
        {
            const float pinSpacing = 4;

            int rows = Math.Max(Inputs.Length, Outputs.Length);

            for (int i = 0; i < rows; i++)
            {
                float rowWidth = 0;
                float rowHeight = 0;

                if (i < Inputs.Length)
                {
                    Pin input = Inputs[i];

                    rowWidth = input.Size.Width;
                    rowHeight = input.Size.Height;
                }

                if (i < Outputs.Length)
                {
                    Pin output = Outputs[i];

                    rowWidth += output.Size.Width + pinSpacing;
                    rowHeight = Math.Max(rowHeight, output.Size.Height);
                }

                contentWidth = Math.Max(contentWidth, rowWidth);
                contentHeight += rowHeight + pinSpacing;
            }
        }

        // Content
        if (Content is not null)
        {
            contentWidth = Math.Max(contentWidth, Content.Size.Width);
            contentHeight += Content.Size.Height;
        }

        return new((Theme.CardBorderWidth * 2) + (Theme.CardPadding * 2) + contentWidth, (Theme.CardBorderWidth * 2) + (Theme.CardPadding * 2) + contentHeight);
    }

    protected override void OnArrange()
    {
        float left = Bounds.Left + Theme.CardBorderWidth + Theme.CardPadding;
        float top = Bounds.Top + Theme.CardBorderWidth + Theme.CardPadding;
        float right = Bounds.Right - Theme.CardBorderWidth - Theme.CardPadding;

        // Header
        if (Header is not null)
        {
            Header.Position = new(left, top);
            top += Header.Size.Height + Theme.CardPadding;
        }

        // Pins
        {
            const float pinSpacing = 4;

            int rows = Math.Max(Inputs.Length, Outputs.Length);

            for (int i = 0; i < rows; i++)
            {
                float rowHeight = 0;

                if (i < Inputs.Length)
                {
                    Pin input = Inputs[i];
                    input.Position = new(left, top);

                    rowHeight = input.Size.Height;
                }

                if (i < Outputs.Length)
                {
                    Pin output = Outputs[i];
                    output.Position = new(right - output.Size.Width, top);

                    rowHeight = Math.Max(rowHeight, output.Size.Height);
                }

                top += rowHeight + pinSpacing;
            }
        }

        // Content
        Content?.Position = new(left, top);
    }

    protected override void OnRender(IDrawingContext dc)
    {
        dc.DrawRectangle(Bounds, Theme.CardCornerRadius, Theme.CardBackgroundColor);
        dc.DrawRectangle(Bounds, Theme.CardCornerRadius, Theme.CardBorderColor, Theme.CardBorderWidth);

        if (Header is not null)
        {
            float lineY = Header.Bounds.Bottom + (Theme.CardPadding / 2);

            dc.DrawLine(new(Bounds.Left, lineY), new(Bounds.Right, lineY), Theme.CardBorderColor, Theme.CardBorderWidth);
        }
    }
}
