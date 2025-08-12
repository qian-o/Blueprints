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

    public SKColor Background { get; set; } = SKColors.Transparent;

    public SKColor Stroke { get; set; } = SKColors.Transparent;

    public Thickness Margin { get; set; } = 0.0f;

    public Thickness Padding { get; set; } = 0.0f;

    public Thickness StrokeThickness { get; set; } = 0.0f;

    public CornerRadius CornerRadius { get; set; } = 0.0f;

    public float FontWeight { get; set; } = 400.0f;

    public float FontSize { get; set; } = 12.0f;

    public void UseGlobalStyle()
    {
        fontFamily = null;
        foreground = null;
    }
}
