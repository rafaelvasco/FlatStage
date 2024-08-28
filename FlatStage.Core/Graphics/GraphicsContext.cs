using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BGFX;

namespace FlatStage;



public static unsafe partial class Graphics
{
    internal static Action<int, int>? BackBufferSizeChanged;

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

    static Graphics()
    {
        SampleFlags = new Bgfx.SamplerFlags[MaxTextureSamplers];
    }

    internal static void Init()
    {
        InitGraphicsContext();

        InitGraphicsState();

        Platform.WindowResized = OnPlatformWindowResized;
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

    public static void SetRenderTarget(int renderPass, RenderTarget? target = null)
    {
        Bgfx.set_view_frame_buffer((ushort)renderPass, target?.Handle ?? BgfxUtils.FrameBufferNone);
    }

    public static void SetViewScissor(byte view, int x, int y, int w, int h)
    {
        Bgfx.set_view_scissor(view, (ushort)x, (ushort)y, (ushort)w, (ushort)h);
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
        var nativeDisplayHandles = Platform.GetDisplayNativeHandles();

        var platformInfo = new Bgfx.PlatformData()
        {
            nwh = nativeDisplayHandles.WindowHandle.ToPointer(),
            ndt = nativeDisplayHandles.DisplayType.GetValueOrDefault().ToPointer(),
        };

        var init = (Bgfx.Init*)Marshal.AllocHGlobal(sizeof(Bgfx.Init)).ToPointer();

        Bgfx.init_ctor(init);

        init->vendorId = (ushort)Bgfx.PciIdFlags.None; // Auto Select

        init->type = Bgfx.RendererType.Count; // Auto Select

        var displaySize = Game.WindowSize;

        init->resolution.width = (uint)displaySize.Width;
        init->resolution.height = (uint)displaySize.Height;

        BackbufferWidth = displaySize.Width;
        BackbufferHeight = displaySize.Height;

        init->resolution.format = Bgfx.TextureFormat.BGRA8;
        init->resolution.reset = (uint)Bgfx.ResetFlags.Vsync;
        init->platformData = platformInfo;

        if (Platform.PlatformId == PlatformId.Mac)
        {
            Bgfx.render_frame(0);
        }

        if (!Bgfx.init(init))
        {
            throw new ApplicationException("Failed to initialize Graphics Context");
        }

        Marshal.FreeHGlobal((IntPtr)init);

        var caps = Bgfx.get_caps();

        Console.WriteLine(caps->rendererType);

        GraphicsBackend = GetGraphicsBackend(Bgfx.get_renderer_type());

        Console.WriteLine($"Graphics Backend: {GraphicsBackend}");
    }

    private static GraphicsBackend GetGraphicsBackend(Bgfx.RendererType rendererType)
    {
        switch (rendererType)
        {
            case Bgfx.RendererType.OpenGL: return GraphicsBackend.OpenGL;
            case Bgfx.RendererType.OpenGLES: return GraphicsBackend.OpenGLES;
            case Bgfx.RendererType.Direct3D11: return GraphicsBackend.Direct3D11;
            case Bgfx.RendererType.Direct3D12: return GraphicsBackend.Direct3D12;
            case Bgfx.RendererType.Vulkan: return GraphicsBackend.Vulkan;
            case Bgfx.RendererType.Metal: return GraphicsBackend.Metal;
        }

        FlatException.Throw($"Unsupported Renderer Backend: {rendererType}");

        return GraphicsBackend.Unknown;
    }

    private static void InitGraphicsState()
    {
        _baseState = Bgfx.StateFlags.WriteRgb | Bgfx.StateFlags.WriteA | Bgfx.StateFlags.WriteZ;

        SetViewClear(0, Color.Black);
    }

    private static void ApplyGraphicsChanges(GraphicsChanges changes)
    {
        var currentState = GetCurrentGraphicsChangesState();

        if (currentState.Equals(changes))
        {
            return;
        }

        VsyncEnabled = changes.VsyncEnabled;

        Bgfx.reset((uint)changes.BackbufferWidth, (uint)changes.BackbufferHeight, (uint)ResetFlags, Bgfx.TextureFormat.Count);

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

        BackBufferSizeChanged?.Invoke(size.Width, size.Height);
    }

    private static int _backbufferWidth;
    private static int _backbufferHeight;

    private static Bgfx.StateFlags _baseState;
    private static readonly Bgfx.SamplerFlags[] SampleFlags;
}
