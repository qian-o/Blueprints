#if WINDOWS
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;
using SkiaSharp;

namespace Blueprints.WinUI;

public unsafe partial class SKView : Canvas
{
    private static readonly DXGI dxgi = DXGI.GetApi(null);
    private static readonly D3D12 d3d12 = D3D12.GetApi();

    private static ComPtr<IDXGIFactory6> factory;
    private static ComPtr<IDXGIAdapter> adapter;
    private static ComPtr<ID3D12Device> device;
    private static ComPtr<ID3D12CommandQueue> queue;

    private static readonly GRContext context;

    static SKView()
    {
        dxgi.CreateDXGIFactory1(out factory);
        factory.EnumAdapterByGpuPreference(0, GpuPreference.HighPerformance, out adapter);
        d3d12.CreateDevice(adapter, D3DFeatureLevel.Level120, out device);

        CommandQueueDesc commandQueueDesc = new()
        {
            Type = CommandListType.Direct,
            Priority = (int)CommandQueuePriority.Normal,
            Flags = CommandQueueFlags.None,
            NodeMask = 0
        };

        device.CreateCommandQueue(ref commandQueueDesc, out queue);

        context = GRContext.CreateDirect3D(new()
        {
            Adapter = (nint)adapter.Handle,
            Device = (nint)device.Handle,
            Queue = (nint)queue.Handle,
            ProtectedContext = false
        });
    }

    private WriteableBitmap? bitmap;
    private SKSurface? surface;

    public void Invalidate()
    {
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.Normal, OnRender);
    }

    private void OnRender()
    {
        int width = Math.Max(1, (int)ActualWidth);
        int height = Math.Max(1, (int)ActualHeight);

        if (bitmap is null || surface is null || bitmap.PixelWidth != width || bitmap.PixelHeight != height)
        {
            surface?.Dispose();

            bitmap = new(width, height);
            surface = SKSurface.Create(context, true, new(width, height, SKColorType.Bgra8888, SKAlphaType.Premul));

            Background = new ImageBrush
            {
                ImageSource = bitmap
            };
        }

        Paint?.Invoke(this, surface!.Canvas);
    }
}
#endif
