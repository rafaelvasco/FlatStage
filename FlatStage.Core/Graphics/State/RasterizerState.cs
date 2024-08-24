namespace FlatStage;

public class RasterizerState()
{
    public CullMode CullMode { get; set; } = CullMode.CullCounterClockwiseFace;

    public bool ScissorTestEnable { get; set; } = false;

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
