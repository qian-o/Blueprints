#if __UNO__
using SkiaSharp;
using Uno.WinUI.Graphics2DSK;
using Windows.Foundation;

namespace Blueprints.WinUI;

public partial class SKView : SKCanvasElement
{
    protected override void RenderOverride(SKCanvas canvas, Size area)
    {
        canvas.Save();
        canvas.Scale((float)(1.0 / Dpi));

        Paint?.Invoke(this, canvas);

        canvas.Restore();
    }
}
#endif
