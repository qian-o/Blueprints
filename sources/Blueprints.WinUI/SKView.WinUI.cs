#if WINDOWS
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;
using SkiaSharp;
using WinRT;

namespace Blueprints.WinUI;

public unsafe partial class SKView : SwapChainPanel
{
    [GeneratedComInterface]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("63aad0b8-7c24-40ff-85a8-640d944cc325")]
    internal partial interface ISwapChainPanelNative
    {
        [PreserveSig]
        int SetSwapChain(nint swapChain);

        ulong Release();
    }

    private partial class SwapChain : IDisposable
    {
        private const int BufferCount = 3;

        private readonly ISwapChainPanelNative swapChainPanelNative;

        private ComPtr<IDXGISwapChain3> swapChain;

        public SwapChain(SwapChainPanel swapChainPanel, uint width, uint height)
        {
            swapChainPanelNative = swapChainPanel.As<ISwapChainPanelNative>();

            SwapChainDesc1 desc = new()
            {
                Width = width,
                Height = height,
                Format = Format.FormatB8G8R8A8Unorm,
                SampleDesc = new() { Count = 1, Quality = 0 },
                BufferUsage = DXGI.UsageRenderTargetOutput,
                BufferCount = BufferCount,
                Scaling = Scaling.Stretch,
                SwapEffect = SwapEffect.FlipDiscard,
                Flags = (uint)SwapChainFlag.AllowTearing
            };

            factory.CreateSwapChainForComposition(queue, &desc, (ComPtr<IDXGIOutput>)null, ref swapChain);

            swapChainPanelNative.SetSwapChain((nint)swapChain.Handle);

            Width = width;
            Height = height;
        }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public GRBackendTexture ToGRBackendTexture()
        {
            swapChain.GetBuffer(swapChain.GetCurrentBackBufferIndex(), out ComPtr<ID3D12Resource> resource);

            GRD3DTextureResourceInfo info = new()
            {
                Resource = (nint)resource.Handle,
                ResourceState = (uint)ResourceStates.Common,
                SampleCount = 1,
                LevelCount = 1,
                Format = (uint)Format.FormatB8G8R8A8Unorm,
            };

            return new((int)Width, (int)Height, info);
        }

        public void Resize(uint width, uint height)
        {
            if (width == Width && height == Height)
            {
                return;
            }

            swapChain.ResizeBuffers(BufferCount, width, height, Format.FormatB8G8R8A8Unorm, (uint)SwapChainFlag.None);

            Width = width;
            Height = height;
        }

        public void Present()
        {
            swapChain.Present(1, 0);
        }

        public void Dispose()
        {
            swapChainPanelNative.SetSwapChain(0);

            swapChain.Dispose();
        }
    }

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

        CommandQueueDesc desc = new()
        {
            Type = CommandListType.Direct,
            Priority = (int)CommandQueuePriority.Normal,
            Flags = CommandQueueFlags.None,
            NodeMask = 0
        };

        device.CreateCommandQueue(ref desc, out queue);

        context = GRContext.CreateDirect3D(new()
        {
            Adapter = (nint)adapter.Handle,
            Device = (nint)device.Handle,
            Queue = (nint)queue.Handle,
            ProtectedContext = false
        });
    }

    private SwapChain? swapChain;

    public void Invalidate()
    {
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.Normal, OnRender);
    }

    private void OnRender()
    {
        uint width = (uint)(ActualWidth * Dpi);
        uint height = (uint)(ActualHeight * Dpi);

        if (width is 0 || height is 0)
        {
            return;
        }

        swapChain ??= new(this, width, height);
        swapChain.Resize(width, height);

        using GRBackendTexture texture = swapChain.ToGRBackendTexture();
        using SKSurface surface = SKSurface.Create(context, texture, GRSurfaceOrigin.TopLeft, SKColorType.Bgra8888);

        Paint?.Invoke(this, surface.Canvas);

        context.Flush();

        swapChain.Present();
    }
}
#endif
