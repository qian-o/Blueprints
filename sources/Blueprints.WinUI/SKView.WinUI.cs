#if WINDOWS
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Silk.NET.Core.Native;
using Silk.NET.Direct3D12;
using Silk.NET.DXGI;
using SkiaSharp;
using Windows.Foundation;
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
        private readonly GRBackendTexture[] textures = new GRBackendTexture[BufferCount];
        private readonly SKSurface[] surfaces = new SKSurface[BufferCount];

        private ComPtr<IDXGISwapChain3> swapChain;

        public SwapChain(SwapChainPanel swapChainPanel, uint width, uint height, float scale)
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

            Matrix3X2F matrix = new() { DXGI11 = scale, DXGI22 = scale };
            swapChain.SetMatrixTransform(&matrix);

            CreateFrameBuffers(width, height);

            Width = width;
            Height = height;
            Scale = scale;

            swapChainPanelNative.SetSwapChain((nint)swapChain.Handle);
        }

        public uint Width { get; private set; }

        public uint Height { get; private set; }

        public float Scale { get; private set; }

        public SKSurface CurrentSurface => surfaces[swapChain.GetCurrentBackBufferIndex()];

        public void Resize(uint width, uint height, float scale)
        {
            if (width == Width && height == Height && scale == Scale)
            {
                return;
            }

            swapChain.ResizeBuffers(BufferCount, width, height, Format.FormatB8G8R8A8Unorm, (uint)SwapChainFlag.None);

            Matrix3X2F matrix = new() { DXGI11 = scale, DXGI22 = scale };
            swapChain.SetMatrixTransform(&matrix);

            CreateFrameBuffers(width, height);

            Width = width;
            Height = height;
            Scale = scale;
        }

        public void Present()
        {
            swapChain.Present(1, 0);
        }

        public void Dispose()
        {
            DestroyFrameBuffers();

            swapChainPanelNative.SetSwapChain(0);

            swapChain.Dispose();
        }

        private void CreateFrameBuffers(uint width, uint height)
        {
            DestroyFrameBuffers();

            for (uint i = 0; i < BufferCount; i++)
            {
                swapChain.GetBuffer(i, out ComPtr<ID3D12Resource> resource);

                GRD3DTextureResourceInfo info = new()
                {
                    Resource = (nint)resource.Handle,
                    ResourceState = (uint)ResourceStates.Common,
                    SampleCount = 1,
                    LevelCount = 1,
                    Format = (uint)Format.FormatB8G8R8A8Unorm,
                };

                textures[i] = new((int)width, (int)height, info);
                surfaces[i] = SKSurface.Create(context, textures[i], GRSurfaceOrigin.TopLeft, SKColorType.Bgra8888);
            }
        }

        private void DestroyFrameBuffers()
        {
            for (int i = 0; i < BufferCount; i++)
            {
                textures[i]?.Dispose();
                surfaces[i]?.Dispose();
            }
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
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.Normal, OnInvalidate);
    }

    protected static SKPoint SKPoint(Point point)
    {
        return new((float)point.X, (float)point.Y);
    }

    private void OnInvalidate()
    {
        uint width = (uint)(ActualWidth * Dpi);
        uint height = (uint)(ActualHeight * Dpi);

        if (width is 0 || height is 0)
        {
            return;
        }

        swapChain ??= new(this, width, height, (float)(1.0 / Dpi));
        swapChain.Resize(width, height, (float)(1.0 / Dpi));

        Paint?.Invoke(this, swapChain.CurrentSurface.Canvas);

        context.Flush();
        context.PurgeUnusedResources(2000);

        swapChain.Present();
    }

    partial void UnloadedPartial()
    {
        swapChain?.Dispose();
        swapChain = null;
    }
}
#endif
