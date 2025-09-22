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

[GeneratedComInterface]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Guid("63aad0b8-7c24-40ff-85a8-640d944cc325")]
internal unsafe partial interface ISwapChainPanelNative
{
    void SetSwapChain(IDXGISwapChain3* swapChain);

    ulong Release();
}

internal static unsafe class GPU
{
    public static ComPtr<IDXGIFactory6> Factory;

    public static ComPtr<IDXGIAdapter> Adapter;

    public static ComPtr<ID3D12Device> Device;

    public static ComPtr<ID3D12CommandQueue> Queue;

    static GPU()
    {
        DXGI = DXGI.GetApi(null);
        D3D12 = D3D12.GetApi();

        DXGI.CreateDXGIFactory1(out Factory);
        Factory.EnumAdapterByGpuPreference(0, GpuPreference.HighPerformance, out Adapter);
        D3D12.CreateDevice(Adapter, D3DFeatureLevel.Level120, out Device);

        CommandQueueDesc desc = new()
        {
            Type = CommandListType.Direct,
            Priority = (int)CommandQueuePriority.Normal,
            Flags = CommandQueueFlags.None,
            NodeMask = 0
        };

        Device.CreateCommandQueue(ref desc, out Queue);

        GRContext = GRContext.CreateDirect3D(new()
        {
            Adapter = (nint)Adapter.Handle,
            Device = (nint)Device.Handle,
            Queue = (nint)Queue.Handle
        });
    }

    public static DXGI DXGI { get; }

    public static D3D12 D3D12 { get; }

    public static GRContext GRContext { get; }
}

internal unsafe partial class SwapChain : IDisposable
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

        GPU.Factory.CreateSwapChainForComposition(GPU.Queue, &desc, (ComPtr<IDXGIOutput>)null, ref swapChain);

        Matrix3X2F matrix = new() { DXGI11 = scale, DXGI22 = scale };
        swapChain.SetMatrixTransform(&matrix);

        CreateFrameBuffers(width, height);

        Width = width;
        Height = height;
        Scale = scale;

        swapChainPanelNative.SetSwapChain(swapChain.Handle);
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

        swapChainPanelNative.SetSwapChain(null);

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
            surfaces[i] = SKSurface.Create(GPU.GRContext, textures[i], GRSurfaceOrigin.TopLeft, SKColorType.Bgra8888);
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

public partial class SKView : SwapChainPanel
{
    private SwapChain? swapChain;

    public void Invalidate()
    {
        DispatcherQueue?.TryEnqueue(DispatcherQueuePriority.Normal, OnInvalidate);
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

        GPU.GRContext.Flush();
        GPU.GRContext.PurgeUnusedResources(2000);

        swapChain.Present();
    }

    partial void UnloadedPartial()
    {
        swapChain?.Dispose();
        swapChain = null;
    }
}
#endif
