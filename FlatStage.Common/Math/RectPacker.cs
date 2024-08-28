using static Stb.StbRectPack;

namespace FlatStage;

public struct PackerRect(Rect rectangle, int data)
{
    public Rect Rectangle { get; private set; } = rectangle;

    public int X => Rectangle.X;
    public int Y => Rectangle.Y;

    public int Width => Rectangle.Width;

    public int Height => Rectangle.Height;

    public int Data { get; private set; } = data;
}

public unsafe class RectPacker : IDisposable
{
    private const int DEFAULT_WIDTH = 256;
    private const int DEFAULT_HEIGHT = 256;

    private readonly stbrp_context _context;
    private readonly List<PackerRect> _rects = new();

    public int Width => _context.width;
    public int Height => _context.height;

    public List<PackerRect> Rectangles => _rects;

    public RectPacker(int width = DEFAULT_WIDTH, int height = DEFAULT_HEIGHT)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        var numNodes = width;
        _context = new stbrp_context(numNodes);

        fixed (stbrp_context* contextPtr = &_context)
        {
            stbrp_init_target(contextPtr, width, height, _context.all_nodes, numNodes);
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public PackerRect? PackRect(int width, int height, int userData)
    {
        var rect = new stbrp_rect
        {
            id = _rects.Count,
            w = width,
            h = height
        };

        int result;

        fixed (stbrp_context* contextPtr = &_context)
        {
            result = stbrp_pack_rects(contextPtr, &rect, 1);
        }

        if (result == 0)
        {
            return null;
        }

        var packRect = new PackerRect(new Rect(rect.x, rect.y, rect.w, rect.h), userData);

        _rects.Add(packRect);

        return packRect;
    }
}
