using System;
using System.Collections.Generic;
using FlatStage.Foundation.BGFX;

namespace FlatStage;

public class ShaderParameter
{
    internal Bgfx.UniformHandle Handle { get; }

    public string Name { get; }

    public bool Constant { get; set; } = false;

    internal bool SubmitedOnce;

    public Vec4 Value => _value;

    private Vec4 _value;


    internal ShaderParameter(Bgfx.UniformHandle handle, string name)
    {
        Name = name;
        Handle = handle;
    }

    public void SetValue(float v)
    {
        _value.X = v;
    }

    public void SetValue(Vec2 v)
    {
        _value.X = v.X;
        _value.Y = v.Y;
    }

    public void SetValue(Vec3 v)
    {
        _value.X = v.X;
        _value.Y = v.Y;
        _value.Z = v.Z;
    }

    public void SetValue(Vec4 v)
    {
        _value = v;
    }

    public void SetValue(Color color)
    {
        _value.X = color.R / 255f;
        _value.Y = color.G / 255f;
        _value.Z = color.B / 255f;
        _value.W = color.A / 255f;
    }
}

public class ShaderSampler
{
    internal Bgfx.UniformHandle Handle { get; }

    public Texture2D? Texture { get; internal set; }

    internal ShaderSampler(Bgfx.UniformHandle handle)
    {
        Handle = handle;
        Texture = null;
    }
}

internal struct ShaderProgramProps : IDefinitionData
{
    public Memory<byte> VertexShader = Memory<byte>.Empty;
    public Memory<byte> FragmentShader = Memory<byte>.Empty;
    public Memory<string> Samplers = Memory<string>.Empty;
    public Memory<string> Parameters = Memory<string>.Empty;

    public readonly override string ToString()
    {
        return
            $"VS: {VertexShader.Length}, FS: {FragmentShader.Length}, Samplers: {Samplers.Length}, Parameters: {Parameters.Length}";
    }

    public ShaderProgramProps()
    {
    }

    public readonly bool IsValid()
    {
        return !VertexShader.IsEmpty && !FragmentShader.IsEmpty;
    }
}

public sealed class ShaderProgram : Asset
{
    internal ShaderSampler[] Samplers { get; }

    internal ShaderParameter[] Parameters { get; }

    internal ShaderProgram(string id, Bgfx.ProgramHandle handle, ShaderSampler[] samples, ShaderParameter[] parameters)
        : base(id)
    {
        Handle = handle;
        Samplers = samples;
        Parameters = parameters;

        _paramsMap = new Dictionary<string, int>();

        for (int i = 0; i < parameters.Length; ++i)
        {
            _paramsMap.Add(parameters[i].Name, i);
        }
    }

    public void SetTexture(int slot, Texture2D texture)
    {
        slot = Calc.Max(slot, 0);

        Samplers[slot].Texture = texture;

        TextureSlotIndex = Calc.Max(TextureSlotIndex, slot);
    }

    public ShaderParameter? GetParam(string name)
    {
        return _paramsMap.TryGetValue(name, out var index) ? Parameters[index] : null;
    }

    protected override void Free()
    {
        for (int i = 0; i < Samplers.Length; i++)
        {
            Bgfx.destroy_uniform(Samplers[i].Handle);
        }

        for (int i = 0; i < Parameters.Length; i++)
        {
            Bgfx.destroy_uniform(Parameters[i].Handle);
        }

        Bgfx.destroy_program(Handle);
    }

    internal readonly Bgfx.ProgramHandle Handle;

    internal int TextureSlotIndex { get; private set; }

    private readonly Dictionary<string, int> _paramsMap;
}