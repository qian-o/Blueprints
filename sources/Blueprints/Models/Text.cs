namespace Blueprints;

public class Text : Element
{
    public string Content { get; set; } = string.Empty;

    public override void Layout(IDrawingContext dc, float offsetX, float offsetY)
    {
        throw new NotImplementedException();
    }

    public override void Render(IDrawingContext dc)
    {
        throw new NotImplementedException();
    }

    public static implicit operator string(Text text)
    {
        return text.Content;
    }

    public static implicit operator Text(string content)
    {
        return new() { Content = content };
    }
}
