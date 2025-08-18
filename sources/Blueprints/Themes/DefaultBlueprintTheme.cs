using SkiaSharp;

namespace Blueprints;

public class DefaultBlueprintTheme : IBlueprintTheme
{
    public DefaultBlueprintTheme()
    {
        ApplyTheme();
    }

    public ThemeMode Mode { get; set { field = value; ApplyTheme(); } }

    public SKColor TextColor { get; private set; }

    public SKColor BackgroundColor { get; private set; }

    public SKColor MinorGridLineColor { get; private set; }

    public SKColor MajorGridLineColor { get; private set; }

    public SKColor CardBackgroundColor { get; private set; }

    public SKColor CardBorderColor { get; private set; }

    public float CardBorderWidth { get; private set; }

    public float CardCornerRadius { get; } = 8.0f;

    public float CardPadding { get; } = 10.0f;

    public SKColor PinColor { get; private set; }

    public SKColor PinHoverColor { get; private set; }

    public float PinShapeSize { get; } = 8.0f;

    public float PinPadding { get; } = 4.0f;

    private void ApplyTheme()
    {
        switch (Mode)
        {
            case ThemeMode.Light:
                TextColor = new SKColor(32, 32, 32);
                BackgroundColor = new SKColor(245, 245, 245);
                MinorGridLineColor = new SKColor(225, 225, 225);
                MajorGridLineColor = new SKColor(200, 200, 200);
                CardBackgroundColor = SKColors.White;
                CardBorderColor = new SKColor(180, 180, 180);
                CardBorderWidth = 1.0f;
                PinColor = new SKColor(100, 100, 100);
                PinHoverColor = new SKColor(220, 220, 220);
                break;

            case ThemeMode.Dark:
                TextColor = new SKColor(230, 230, 230);
                BackgroundColor = new SKColor(30, 30, 30);
                MinorGridLineColor = new SKColor(50, 50, 50);
                MajorGridLineColor = new SKColor(70, 70, 70);
                CardBackgroundColor = new SKColor(45, 45, 48);
                CardBorderColor = new SKColor(100, 100, 100);
                CardBorderWidth = 1.0f;
                PinColor = new SKColor(150, 150, 150);
                PinHoverColor = new SKColor(50, 50, 50);
                break;

            case ThemeMode.HighContrast:
                TextColor = SKColors.White;
                BackgroundColor = SKColors.Black;
                MinorGridLineColor = new SKColor(64, 64, 64);
                MajorGridLineColor = new SKColor(128, 128, 128);
                CardBackgroundColor = new SKColor(20, 20, 20);
                CardBorderColor = SKColors.White;
                CardBorderWidth = 2.0f;
                PinColor = new SKColor(200, 200, 200);
                PinHoverColor = new SKColor(80, 80, 80);
                break;
        }
    }
}
