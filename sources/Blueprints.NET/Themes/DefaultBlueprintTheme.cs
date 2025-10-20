using SkiaSharp;

namespace Blueprints;

public class DefaultBlueprintTheme : IBlueprintTheme
{
    public static DefaultBlueprintTheme Instance { get; } = new();

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

    public float CardPadding { get; } = 12.0f;

    public SKColor PinColor { get; private set; }

    public SKColor PinHoverColor { get; private set; }

    public float PinShapeSize { get; } = 10.0f;

    public float PinShapeStrokeWidth { get; } = 2.0f;

    public float PinPadding { get; } = 6.0f;

    public SKColor ConnectionColor { get; private set; }

    public SKColor ConnectionHoverColor { get; private set; }

    public float ConnectionWidth { get; } = 3.0f;

    public float ConnectionHoverWidth { get; } = 4.0f;

    private void ApplyTheme()
    {
        switch (Mode)
        {
            case ThemeMode.Light:
                TextColor = new(32, 32, 32);
                BackgroundColor = new(245, 245, 245);
                MinorGridLineColor = new(225, 225, 225);
                MajorGridLineColor = new(200, 200, 200);
                CardBackgroundColor = new(255, 255, 255, 240);
                CardBorderColor = new(180, 180, 180);
                CardBorderWidth = 1.0f;
                PinColor = new(100, 100, 100);
                PinHoverColor = new(220, 220, 220);
                ConnectionColor = new(100, 100, 100);
                ConnectionHoverColor = new(50, 50, 50);
                break;

            case ThemeMode.Dark:
                TextColor = new(230, 230, 230);
                BackgroundColor = new(30, 30, 30);
                MinorGridLineColor = new(50, 50, 50);
                MajorGridLineColor = new(70, 70, 70);
                CardBackgroundColor = new(45, 45, 48, 240);
                CardBorderColor = new(100, 100, 100);
                CardBorderWidth = 1.0f;
                PinColor = new(150, 150, 150);
                PinHoverColor = new(50, 50, 50);
                ConnectionColor = new(150, 150, 150);
                ConnectionHoverColor = new(200, 200, 200);
                break;

            case ThemeMode.HighContrast:
                TextColor = SKColors.White;
                BackgroundColor = SKColors.Black;
                MinorGridLineColor = new(64, 64, 64);
                MajorGridLineColor = new(128, 128, 128);
                CardBackgroundColor = new(20, 20, 20, 240);
                CardBorderColor = SKColors.White;
                CardBorderWidth = 2.0f;
                PinColor = new(200, 200, 200);
                PinHoverColor = new(80, 80, 80);
                ConnectionColor = new(200, 200, 200);
                ConnectionHoverColor = SKColors.White;
                break;
        }
    }
}
