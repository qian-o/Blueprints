using SkiaSharp;

namespace Blueprints;

public abstract class Drawable : Element
{
    public new SKPoint Position { get => base.Position; internal set => base.Position = value; }

    protected override Element[] SubElements()
    {
        return [];
    }

    protected override sealed void OnArrange()
    {
    }

    #region Implicit Operators
    public static implicit operator Drawable(string text)
    {
        return new Text(text);
    }
    #endregion
}
