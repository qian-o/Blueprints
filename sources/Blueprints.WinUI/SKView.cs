using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using Windows.Foundation;

namespace Blueprints.WinUI;

public partial class SKView : Canvas
{
    private WriteableBitmap? bitmap;
    private nint pixels;
    private SKSurface? surface;

    public SKView()
    {
        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi => XamlRoot?.RasterizationScale ?? 1.0;


    public event EventHandler<SKCanvas>? Paint;

    public void Invalidate()
    {
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.High, DoInvalidate);
    }

    protected static SKPoint SKPoint(Point point)
    {
        return new((float)point.X, (float)point.Y);
    }

    private unsafe void DoInvalidate()
    {
        int width = (int)(ActualWidth * Dpi);
        int height = (int)(ActualHeight * Dpi);

        if (width is 0 || height is 0)
        {
            return;
        }

        if (bitmap is null || bitmap.PixelWidth != width || bitmap.PixelHeight != height || pixels is 0 || surface is null)
        {
            Background = new ImageBrush() { ImageSource = bitmap = new(width, height) };

            NativeMemory.Free((void*)pixels);
            pixels = (nint)NativeMemory.Alloc((nuint)(width * height * 4));

            surface?.Dispose();
            surface = SKSurface.Create(new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), pixels);
        }

        Paint?.Invoke(this, surface.Canvas);

        bitmap.PixelBuffer.AsStream().Write(new ReadOnlySpan<byte>((void*)pixels, width * height * 4));

        bitmap.Invalidate();
    }
}
