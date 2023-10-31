namespace FlatStage.Graphics;

public class RasterizerState
{
    public CullMode CullMode { get; set; }

    public bool ScissorTestEnable { get; set; }

    public RasterizerState()
    {
        CullMode = CullMode.CullCounterClockwiseFace;
        ScissorTestEnable = false;
    }

    public static readonly RasterizerState CullClockWise = new(CullMode.CullClockwiseFace);

    public static readonly RasterizerState CullCounterClockWise = new(CullMode.CullCounterClockwiseFace);

    public static readonly RasterizerState CullNone = new(CullMode.None);

    private RasterizerState(
        CullMode cullMode
    ) : this()
    {
        CullMode = cullMode;
    }
}