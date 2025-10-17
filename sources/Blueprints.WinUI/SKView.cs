using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using Windows.Foundation;
using WinRT;
using Buffer = Windows.Storage.Streams.Buffer;

namespace Blueprints.WinUI;

public partial class SKView : Canvas
{
#if WINDOWS
    [ComImport]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("905a0fef-bc53-11df-8c49-001e4fc686da")]
    private interface IServiceProviderInterop
    {
        nint Buffer { get; }
    }
#else
    [UnsafeAccessor(UnsafeAccessorKind.Method)]
    private static extern void ApplyActionOnRawBufferPtr(Buffer buffer, Action<nint> action);
#endif

    private WriteableBitmap? bitmap;

    public SKView()
    {
        SizeChanged += (_, _) => Invalidate();
    }

    public double Dpi => XamlRoot?.RasterizationScale ?? 1.0;


    public event EventHandler<SKCanvas>? Paint;

    public void Invalidate()
    {
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.High, OnInvalidate);
    }

    protected static SKPoint SKPoint(Point point)
    {
        return new((float)point.X, (float)point.Y);
    }

    private unsafe void OnInvalidate()
    {
        int width = (int)(ActualWidth * Dpi);
        int height = (int)(ActualHeight * Dpi);

        if (width is 0 || height is 0)
        {
            return;
        }

        if (bitmap is null || bitmap.PixelWidth != width || bitmap.PixelHeight != height)
        {
            Background = new ImageBrush() { ImageSource = bitmap = new(width, height) };
        }

#if WINDOWS
        using SKSurface surface = SKSurface.Create(new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), bitmap.PixelBuffer.As<IServiceProviderInterop>().Buffer);

        Paint?.Invoke(this, surface.Canvas);
#else
        ApplyActionOnRawBufferPtr((Buffer)bitmap.PixelBuffer, pixels =>
        {
            using SKSurface surface = SKSurface.Create(new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul), pixels);

            Paint?.Invoke(this, surface.Canvas);
        });
#endif

        bitmap.Invalidate();
    }
}
