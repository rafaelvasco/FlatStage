using FlatStage.Platform;
using FlatStage.Foundation.BGFX;
using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace FlatStage.Graphics;

public enum GraphicsBackend
{
    Direct3D11,
    Direct3D12,
    Vulkan,
    Metal,
    OpenGl
}

public static unsafe partial class GraphicsContext
{
    internal static Action<int, int> BackBufferSizeChanged = null!;

    public static int BackbufferWidth
    {
        get => _backbufferWidth;
        private set => _backbufferWidth = value;
    }

    public static int BackbufferHeight
    {
        get => _backbufferHeight;
        private set => _backbufferHeight = value;
    }

    public static bool VsyncEnabled
    {
        get => (ResetFlags & Bgfx.ResetFlags.Vsync) == Bgfx.ResetFlags.Vsync;
        private set => ResetFlags |= value ? Bgfx.ResetFlags.Vsync : Bgfx.ResetFlags.None;
    }

    public static GraphicsBackend GraphicsBackend { get; private set; }

    private static Bgfx.ResetFlags ResetFlags { get; set; } = Bgfx.ResetFlags.Vsync;

    private const int MaxTextureSamplers = 16;

    static GraphicsContext()
    {
        SampleFlags = new Bgfx.SamplerFlags[MaxTextureSamplers];
    }

    internal static void Init()
    {
        InitGraphicsContext();

        InitGraphicsState();

        PlatformContext.WindowResized = OnPlatformWindowResized;
    }

    public static void Present()
    {
        _ = Bgfx.frame(false);
    }

    internal static void Touch(int renderPass)
    {
        Bgfx.touch((ushort)renderPass);
    }

    public static void SetViewRect(int renderPass, int x, int y, int w, int h)
    {
        Bgfx.set_view_rect((ushort)renderPass, (ushort)x, (ushort)y, (ushort)w, (ushort)h);
    }

    public static void SetViewRect(int renderPass)
    {
        Bgfx.set_view_rect((ushort)renderPass, 0, 0, (ushort)_backbufferWidth, (ushort)_backbufferHeight);
    }

    public static void SetViewClear(int renderPass, Color color)
    {
        Bgfx.set_view_clear((ushort)renderPass, (ushort)(Bgfx.ClearFlags.Color | Bgfx.ClearFlags.Depth), color.Rgba,
            1.0f, 0);
    }

    public static void SetViewModeSequential(int renderPass)
    {
        Bgfx.set_view_mode((ushort)renderPass, Bgfx.ViewMode.Sequential);
    }

    public static void SetState(
        BlendState blendState,
        RasterizerState rasterizerState
    )
    {
        var combinedState =
            _baseState |
            Bgfx.StateFlags.DepthTestLequal |
            blendState.State |
            (Bgfx.StateFlags)rasterizerState.CullMode;

        Bgfx.set_state((ulong)combinedState, blendState.BlendColor.Rgba);
    }

    public static void SetSampleState(int slot, SamplerState samplerState)
    {
        SampleFlags[slot] = samplerState.State;
    }

    public static void SetViewTransform(int renderPass, Matrix view, Matrix proj)
    {
        Bgfx.set_view_transform((ushort)renderPass, Unsafe.AsPointer(ref view.M11), Unsafe.AsPointer(ref proj.M11));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetVertexBuffer<T>(VertexBuffer<T> vbo) where T : struct
        => SetVertexBuffer(vbo, 0, vbo.VertexCount);

    public static void SetVertexBuffer<T>(VertexBuffer<T> vbo, int startVertex, int vertexCount) where T : struct
    {
        Bgfx.set_vertex_buffer(
            0, vbo.Handle,
            (uint)startVertex, (uint)vertexCount
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDynamicVertexBuffer<T>(DynamicVertexBuffer<T> vbo) where T : struct
        => SetDynamicVertexBuffer(vbo, 0, vbo.VertexCount);

    public static void SetDynamicVertexBuffer<T>(DynamicVertexBuffer<T> vbo, int startVertex,
        int vertexCount) where T : struct
    {
        Bgfx.set_dynamic_vertex_buffer(
            0, vbo.Handle,
            (uint)startVertex, (uint)vertexCount
        );
    }

    public static void SetTransientVertexBuffer<T>(ref TransientVertexBuffer<T> tvb, int numVertices) where T : struct
    {
        fixed (Bgfx.TransientVertexBuffer* ptr = &tvb.Handle)
        {
            Bgfx.set_transient_vertex_buffer(0, ptr, 0, (uint)numVertices);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetIndexBuffer<T>(IndexBuffer<T> ibo) where T : struct
        => SetIndexBuffer(ibo, 0, ibo.IndexCount);

    public static void SetIndexBuffer<T>(IndexBuffer<T> ibo, int startIndex, int indexCount) where T : struct
    {
        Bgfx.set_index_buffer(ibo.Handle, (uint)startIndex, (uint)indexCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDynamicIndexBuffer<T>(DynamicIndexBuffer<T> ibo) where T : struct
        => SetDynamicIndexBuffer(ibo, 0, ibo.IndexCount);

    public static void SetDynamicIndexBuffer<T>(DynamicIndexBuffer<T> ibo, int startIndex, int indexCount)
        where T : struct
    {
        Bgfx.set_dynamic_index_buffer(ibo.Handle, (uint)startIndex, (uint)indexCount);
    }

    /// <summary>
    /// Enables debugging features.
    /// </summary>
    /// <param name="features">The set of debug features to enable.</param>
    public static void SetDebugFeatures(DebugFeatures features)
    {
        Bgfx.set_debug((uint)features);
    }

    /// <summary>
    /// Clears the debug text buffer.
    /// </summary>
    /// <param name="color">The color with which to clear the background.</param>
    /// <param name="smallText"><c>true</c> to use a small font for debug output; <c>false</c> to use normal sized text.</param>
    public static void ClearDebugText(DebugColor color = DebugColor.Black, bool smallText = false)
    {
        var attr = (byte)((byte)color << 4);
        Bgfx.dbg_text_clear(attr, smallText);
    }

    /// <summary>
    /// Writes debug text to the screen.
    /// </summary>
    /// <param name="x">The X position, in cells.</param>
    /// <param name="y">The Y position, in cells.</param>
    /// <param name="foreColor">The foreground color of the text.</param>
    /// <param name="backColor">The background color of the text.</param>
    /// <param name="format">The format of the message.</param>
    /// <param name="args">The arguments with which to format the message.</param>
    public static void DebugTextWrite(int x, int y, DebugColor foreColor, DebugColor backColor, string format,
        params object[] args)
    {
        DebugTextWrite(x, y, foreColor, backColor, string.Format(CultureInfo.CurrentCulture, format, args));
    }

    /// <summary>
    /// Writes debug text to the screen.
    /// </summary>
    /// <param name="x">The X position, in cells.</param>
    /// <param name="y">The Y position, in cells.</param>
    /// <param name="foreColor">The foreground color of the text.</param>
    /// <param name="backColor">The background color of the text.</param>
    /// <param name="message">The message to write.</param>
    public static void DebugTextWrite(int x, int y, DebugColor foreColor, DebugColor backColor, string message)
    {
        var attr = (byte)(((byte)backColor << 4) | (byte)foreColor);
        Bgfx.dbg_text_printf((ushort)x, (ushort)y, attr, "%s", message);
    }

    /// <summary>
    /// Draws data directly into the debug text buffer.
    /// </summary>
    /// <param name="x">The X position, in cells.</param>
    /// <param name="y">The Y position, in cells.</param>
    /// <param name="width">The width of the image to draw.</param>
    /// <param name="height">The height of the image to draw.</param>
    /// <param name="data">The image data bytes.</param>
    /// <param name="pitch">The pitch of each line in the image data.</param>
    public static void DebugTextImage(int x, int y, int width, int height, byte[] data, int pitch)
    {
        fixed (byte* ptr = data)
            Bgfx.dbg_text_image((ushort)x, (ushort)y, (ushort)width, (ushort)height, ptr, (ushort)pitch);
    }

    public static void SetRenderTarget(int renderPass, RenderTarget? target = null)
    {
        Bgfx.set_view_frame_buffer((ushort)renderPass, target?.Handle ?? BgfxUtils.FrameBufferNone);
    }

    public static void SetViewScissor(byte view, int x, int y, int w, int h)
    {
        Bgfx.set_view_scissor((ushort)view, (ushort)x, (ushort)y, (ushort)w, (ushort)h);
    }

    public static void Render(int renderPass, ShaderProgram shader)
    {
        SubmitShaderProgram(shader);

        Bgfx.submit((ushort)renderPass, shader.Handle, 0, (byte)Bgfx.DiscardFlags.All);
    }

    public static void TakeScreenshot(string filePath)
    {
        Bgfx.request_screen_shot(Bgfx.FrameBufferHandle.Invalid, filePath);
    }

    private static void SubmitShaderProgram(ShaderProgram shader)
    {
        static void SetTexture(int slot, ShaderSampler sampler)
        {
            if (sampler.Texture != null)
            {
                Bgfx.set_texture((byte)slot, sampler.Handle, sampler.Texture.Handle, (uint)SampleFlags[slot]);
            }
            else
            {
                throw new ApplicationException("Shader Sampler with Null Texture.");
            }
        }

        if (shader.TextureSlotIndex == 0)
        {
            var sampler = shader.Samplers[0];

            SetTexture(0, sampler);
        }
        else
        {
            for (int i = 0; i < shader.TextureSlotIndex; ++i)
            {
                SetTexture(i, shader.Samplers[i]);
            }
        }

        if (shader.Parameters.Length == 0)
        {
            return;
        }

        for (int i = 0; i < shader.Parameters.Length; ++i)
        {
            var p = shader.Parameters[i];

            if (p.Constant)
            {
                if (p.SubmitedOnce)
                {
                    continue;
                }

                p.SubmitedOnce = true;
            }

            var val = p.Value;

            Bgfx.set_uniform(shader.Parameters[i].Handle, &val, 4);
        }
    }

    internal static void Shutdown()
    {
        Console.WriteLine($"Disposing Render Resources: ({RenderResources.Count})");

        foreach (var resource in RenderResources)
        {
            resource.Dispose();
        }

        Bgfx.shutdown();
    }

    private static void InitGraphicsContext()
    {
        var nativeDisplayHandles = PlatformContext.GetDisplayNativeHandles();

        var platformInfo = new Bgfx.PlatformData()
        {
            nwh = nativeDisplayHandles.WindowHandle.ToPointer(),
            ndt = nativeDisplayHandles.DisplayType.GetValueOrDefault().ToPointer(),
        };

        var init = (Bgfx.Init*)Marshal.AllocHGlobal(sizeof(Bgfx.Init)).ToPointer();

        Bgfx.init_ctor(init);

        var rendererType = GetRendererType();

        switch (rendererType)
        {
            case Bgfx.RendererType.Direct3D11:
                GraphicsBackend = GraphicsBackend.Direct3D11;
                break;
            case Bgfx.RendererType.Direct3D12:
                GraphicsBackend = GraphicsBackend.Direct3D12;
                break;
            case Bgfx.RendererType.Metal:
                GraphicsBackend = GraphicsBackend.Metal;
                break;
            case Bgfx.RendererType.OpenGL:
                GraphicsBackend = GraphicsBackend.OpenGl;
                break;
            case Bgfx.RendererType.Vulkan:
                GraphicsBackend = GraphicsBackend.Vulkan;
                break;
        }

        init->vendorId = (ushort)Bgfx.PciIdFlags.None;
        init->type = rendererType;

        var displaySize = Game.WindowSize;

        init->resolution.width = (uint)displaySize.Width;
        init->resolution.height = (uint)displaySize.Height;

        BackbufferWidth = displaySize.Width;
        BackbufferHeight = displaySize.Height;

        init->resolution.format = Bgfx.TextureFormat.BGRA8;
        init->resolution.reset = (uint)Bgfx.ResetFlags.Vsync;
        init->platformData = platformInfo;

        if (!Bgfx.init(init))
        {
            throw new ApplicationException("Failed to initialize Graphics Context");
        }

        Marshal.FreeHGlobal((IntPtr)init);

        Console.WriteLine($"Graphics Backend: {Bgfx.get_renderer_type()}");
    }

    private static void InitGraphicsState()
    {
        _baseState = Bgfx.StateFlags.WriteRgb | Bgfx.StateFlags.WriteA | Bgfx.StateFlags.WriteZ;

        SetViewClear(0, Color.Black);

#if DEBUG
        Bgfx.set_debug((uint)Bgfx.DebugFlags.Text);
#endif
    }

    private static Bgfx.RendererType GetRendererType()
    {
        return PlatformContext.PlatformId switch
        {
            PlatformId.Windows => Bgfx.RendererType.Direct3D11,
            PlatformId.Mac => Bgfx.RendererType.Metal,
            PlatformId.Linux => Bgfx.RendererType.Vulkan,
            _ => throw new ApplicationException("Can't determine renderer backend. Platform unknown"),
        };
    }

    private static void ApplyGraphicsChanges(GraphicsChanges changes)
    {
        var currentState = GetCurrentGraphicsChangesState();

        if (currentState.Equals(changes))
        {
            return;
        }

        VsyncEnabled = changes.VsyncEnabled;

        Bgfx.reset((uint)changes.BackbufferWidth, (uint)changes.BackbufferHeight, (uint)ResetFlags);

        BackbufferWidth = changes.BackbufferWidth;
        BackbufferHeight = changes.BackbufferHeight;
    }

    private static GraphicsChanges GetCurrentGraphicsChangesState()
    {
        return new GraphicsChanges
        {
            BackbufferWidth = BackbufferWidth,
            BackbufferHeight = BackbufferHeight,
            VsyncEnabled = VsyncEnabled
        };
    }

    private static void OnPlatformWindowResized(Size size)
    {
        Game.GameLoop.SuppressDraw();

        var graphicsChanges = new GraphicsChanges
        {
            BackbufferWidth = size.Width,
            BackbufferHeight = size.Height,
            VsyncEnabled = VsyncEnabled
        };

        ApplyGraphicsChanges(graphicsChanges);

        BackBufferSizeChanged(size.Width, size.Height);
    }

    private static int _backbufferWidth;
    private static int _backbufferHeight;

    private static Bgfx.StateFlags _baseState;
    private static readonly Bgfx.SamplerFlags[] SampleFlags;
}
