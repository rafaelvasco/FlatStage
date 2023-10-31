using FlatStage.Foundation.BGFX;

namespace FlatStage.Graphics;

public class SamplerState
{
    public TextureAddressMode AddressU
    {
        get => _addressU;
        set
        {
            if (_addressU != value)
            {
                _addressU = value;
                UpdateUnionState();
            }
        }
    }

    public TextureAddressMode AddressV
    {
        get => _addressV;
        set
        {
            if (_addressV != value)
            {
                _addressV = value;
                UpdateUnionState();
            }
        }
    }

    public TextureFilter TextureFilter
    {
        get => _filter;
        set
        {
            if (value != _filter)
            {
                _filter = value;
                UpdateUnionState();
            }
        }
    }

    public static readonly SamplerState LinearClamp = new(
        TextureFilter.Linear,
        TextureAddressMode.Clamp,
        TextureAddressMode.Clamp
    );

    public static readonly SamplerState LinearWrap = new(
        TextureFilter.Linear,
        TextureAddressMode.Wrap,
        TextureAddressMode.Wrap
    );

    public static readonly SamplerState PointClamp = new(
        TextureFilter.Point,
        TextureAddressMode.Clamp,
        TextureAddressMode.Clamp
    );

    public static readonly SamplerState PointWrap = new(
        TextureFilter.Point,
        TextureAddressMode.Wrap,
        TextureAddressMode.Wrap
    );

    private void UpdateUnionState()
    {
        State =
            (Bgfx.SamplerFlags)_addressU |
            (Bgfx.SamplerFlags)_addressV |
            (Bgfx.SamplerFlags)_filter;
    }

    public SamplerState()
    {
        TextureFilter = TextureFilter.Point;
        AddressU = TextureAddressMode.Wrap;
        AddressV = TextureAddressMode.Wrap;
    }

    private SamplerState(
        TextureFilter filter,
        TextureAddressMode addressU,
        TextureAddressMode addressV
    )
    {
        TextureFilter = filter;
        AddressU = addressU;
        AddressV = addressV;
    }

    private TextureAddressMode _addressU;
    private TextureAddressMode _addressV;
    private TextureFilter _filter;

    internal Bgfx.SamplerFlags State { get; private set; }
}