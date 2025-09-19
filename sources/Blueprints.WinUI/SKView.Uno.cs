#if __UNO__
using SkiaSharp;
using Uno.WinUI.Graphics2DSK;
using Windows.Foundation;

namespace Blueprints.WinUI;

public partial class SKView : SKCanvasElement
{
    protected SKPoint SKPoint(Point point)
    {
        return new((float)(point.X / Dpi), (float)(point.Y / Dpi));
    }

    protected override void RenderOverride(SKCanvas canvas, Size area)
    {
        Paint?.Invoke(this, canvas);
    }
}
#endif
