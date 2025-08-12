using SkiaSharp;

namespace Blueprints;

public partial class Element
{
    private string? fontFamily;
    private SKColor? foreground;

    public IBlueprintStyle Style
    {
        get
        {
            if (Editor is null)
            {
                throw new InvalidOperationException("Editor is not bound to this element.");
            }

            return Editor.Style;
        }
    }

    public string FontFamily { get => fontFamily ?? Style.FontFamily; set => fontFamily = value; }

    public SKColor Foreground { get => foreground ?? Style.Foreground; set => foreground = value; }

    public SKColor Stroke { get; set; }

    public SKColor Background { get; set; }

    public Thickness Margin { get; set; }

    public Thickness Padding { get; set; }

    public float StrokeWidth { get; set; }

    public float CornerRadius { get; set; }

    public float FontWeight { get; set; } = 400.0f;

    public float FontSize { get; set; } = 16.0f;

    public virtual void UseGlobalStyle()
    {
        fontFamily = null;
        foreground = null;
    }
}
