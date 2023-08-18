namespace FlatStage.Graphics;

public struct Quad
{
    public Vertex2D TopLeft;
    public Vertex2D TopRight;
    public Vertex2D BottomRight;
    public Vertex2D BottomLeft;

    public Quad(
       float x1, float y1, float z1, Color color1, float t1X, float t1Y,
       float x2, float y2, float z2, Color color2, float t2X, float t2Y,
       float x3, float y3, float z3, Color color3, float t3X, float t3Y,
       float x4, float y4, float z4, Color color4, float t4X, float t4Y
   )
    {
        TopLeft = new Vertex2D(color1, x1, y1, z1, t1X, t1Y);
        TopRight = new Vertex2D(color2, x2, y2, z2, t2X, t2Y);
        BottomRight = new Vertex2D(color3, x3, y3, z3, t3X, t3Y);
        BottomLeft = new Vertex2D(color4, x4, y4, z4, t4X, t4Y);
    }
}