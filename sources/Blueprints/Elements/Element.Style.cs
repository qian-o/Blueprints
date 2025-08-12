using SkiaSharp;

namespace Blueprints;

public partial class Element
{
    private string? fontFamily;
    private SKColor? background;
    private SKColor? foreground;
    private SKColor? stroke;

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

    public SKColor Background { get => background ?? Style.Background; set => background = value; }

    public SKColor Foreground { get => foreground ?? Style.Foreground; set => foreground = value; }

    public SKColor Stroke { get => stroke ?? Style.Stroke; set => stroke = value; }

    public float FontWeight { get; set; } = 400;

    public float FontSize { get; set; } = 12.0f;

    public float StrokeWidth { get; set; } = 1.0f;

    public Thickness Padding { get; set; } = 0.0f;

    public CornerRadius CornerRadius { get; set; } = 0.0f;

    public void UseGlobalStyle()
    {
        fontFamily = null;
        background = null;
        foreground = null;
        stroke = null;
    }
}
