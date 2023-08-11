namespace FlatStage;

using System.Collections.Generic;
using Foundation.BGFX;

public static unsafe partial class Graphics
{
    internal static void RegisterRenderResource(Disposable resource)
    {
        RenderResources.Add(resource);
    }

    internal static Bgfx.TextureHandle GetFrameBufferTexture(Bgfx.FrameBufferHandle handle, byte attachment)
    {
        return Bgfx.get_texture(handle, attachment);
    }

    internal static Texture2D CreateTexture(string id, Texture2DProps props, bool mutable = false)
    {
        IDefinitionData.ThrowIfInValid(props, "Graphics::CreateTexture");

        Blitter.Begin(props.Data, props.Width, props.Height);
        Blitter.ConvertRgbaToBgra();
        Blitter.End();

        if (mutable)
        {
            var textureHandle = Bgfx.create_texture_2d(
                (ushort)props.Width,
                (ushort)props.Height,
                false,
                1,
                Texture2DProps.Format,
                (ulong)Texture2DProps.Flags,
                null
            );

            var texture = new Texture2D(id, textureHandle, props.Width, props.Height);

            texture.SetData(props.Data);

            return texture;
        }
        else
        {
            var textureHandle = Bgfx.create_texture_2d(
                (ushort)props.Width,
                (ushort)props.Height,
                false,
                1,
                Texture2DProps.Format,
                (ulong)Texture2DProps.Flags,
                Bgfx.make_ref(props.Data.Pin().Pointer, (uint)(props.Width * props.Height * props.BytesPerPixel))
            );

            var texture = new Texture2D(id, textureHandle, props.Width, props.Height);

            return texture;
        }
    }

    internal static ShaderProgram CreateShader(string id, ShaderProgramProps props)
    {
        IDefinitionData.ThrowIfInValid(props, "Graphics::CreateShader");

        var vsMem = BgfxUtils.MakeRef(props.VertexShader);
        var fsMem = BgfxUtils.MakeRef(props.FragmentShader);

        var vs = Bgfx.create_shader(vsMem);
        var fs = Bgfx.create_shader(fsMem);

        var handle = Bgfx.create_program(vs, fs, _destroyShaders: true);

        var shaderSamples = new ShaderSampler[props.Samplers.Length];

        var samplesSpan = props.Samplers.Span;

        for (int i = 0; i < props.Samplers.Length; ++i)
        {
            var samplerHandle = Bgfx.create_uniform(samplesSpan[i], Bgfx.UniformType.Sampler, 1);
            shaderSamples[i] = new ShaderSampler(samplerHandle);
        }

        var shaderParameters = new ShaderParameter[props.Parameters.Length];

        var parametersSpan = props.Parameters.Span;

        for (int i = 0; i < props.Parameters.Length; ++i)
        {
            var parameterHandle = Bgfx.create_uniform(parametersSpan[i], Bgfx.UniformType.Vec4, 4);
            shaderParameters[i] = new ShaderParameter(parameterHandle, parametersSpan[i]);
        }

        var shaderProgram = new ShaderProgram(id, handle, shaderSamples, shaderParameters);

        return shaderProgram;
    }

    internal static RenderTarget CreateRenderTarget(int width, int height)
    {
        var handle = Bgfx.create_frame_buffer((ushort)width, (ushort)height, Bgfx.TextureFormat.BGRA8,
            (ulong)Bgfx.SamplerFlags.Point);

        var renderTarget = new RenderTarget(handle, width, height);

        RegisterRenderResource(renderTarget);

        return renderTarget;
    }

    private static readonly List<Disposable> RenderResources = new();
}