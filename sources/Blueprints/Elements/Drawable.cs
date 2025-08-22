namespace Blueprints;

public abstract class Drawable : Element
{
    protected Drawable()
    {
        IsHitTestVisible = false;
    }

    protected sealed override Element[] SubElements(bool includeConnections = true)
    {
        return [];
    }

    protected sealed override void OnArrange()
    {
    }

    #region Implicit Operators
    public static implicit operator Drawable(string text)
    {
        return new Text(text);
    }
    #endregion
}
