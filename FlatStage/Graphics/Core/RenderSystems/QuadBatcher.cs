using System;

namespace FlatStage.Graphics;

public class QuadBatcher
{
    public int VertexCount => _vertexIndex;
    public int MaxVertices { get; }

    public QuadBatcher(int maxQuads = 2048)
    {
        if (!MathUtils.IsPowerOfTwo(maxQuads))
        {
            maxQuads = MathUtils.NextPowerOfTwo(maxQuads);
        }

        MaxVertices = maxQuads * 4;

        _vertices = new Vertex2D[MaxVertices];

        BuildIndices(maxQuads);
    }

    public void Reset()
    {
        _indiceIndex = 0;
        _vertexIndex = 0;
    }

    public unsafe void PushQuad(ref Quad quad)
    {
        fixed (Vertex2D* p = &_vertices[0])
        {
            int index = _vertexIndex;

            *(p + index) = quad.TopLeft;
            *(p + index + 1) = quad.TopRight;
            *(p + index + 2) = quad.BottomRight;
            *(p + index + 3) = quad.BottomLeft;
        }

        unchecked
        {
            _vertexIndex += 4;
            _indiceIndex += 6;
        }
    }

    public unsafe void PushQuads(Span<Quad> quads)
    {
        fixed (Vertex2D* p = &_vertices[0])
        {
            for (int i = 0, index = _vertexIndex; i < quads.Length; ++i, index += 4)
            {
                ref var quad = ref quads[i];

                *(p + index) = quad.TopLeft;
                *(p + index + 1) = quad.TopRight;
                *(p + index + 2) = quad.BottomRight;
                *(p + index + 3) = quad.BottomLeft;
            }
        }

        unchecked
        {
            _vertexIndex += 4 * quads.Length;
            _indiceIndex += 6 * quads.Length;
        }
    }

    public void Submit()
    {
        Submit(0, _vertexIndex, 0, _indiceIndex);
    }

    public void Submit(int startingVertexIndex, int vertexCount, int startingIndiceIndex, int indexCount)
    {
        GraphicsContext.SetIndexBuffer(_indexBuffer, startingIndiceIndex, indexCount);

        var verticesSpan = new Span<Vertex2D>(_vertices, startingVertexIndex,
           vertexCount > 0 ? vertexCount : _vertexIndex);

        var transientVbo = new TransientVertexBuffer<Vertex2D>(verticesSpan, Vertex2D.Stride, Vertex2D.VertexLayout);

        GraphicsContext.SetTransientVertexBuffer(ref transientVbo, vertexCount);
    }

    private void BuildIndices(int maxQuads)
    {
        int countIndices = maxQuads * 6;

        var indices = new short[countIndices];

        var index = 0;

        for (int i = 0; i < maxQuads; i++, index += 6)
        {
            indices[index + 0] = (short)(i * 4 + 0);
            indices[index + 1] = (short)(i * 4 + 1);
            indices[index + 2] = (short)(i * 4 + 2);

            indices[index + 3] = (short)(i * 4 + 0);
            indices[index + 4] = (short)(i * 4 + 2);
            indices[index + 5] = (short)(i * 4 + 3);
        }

        _indexBuffer = new IndexBuffer<short>(indices);
    }

    private IndexBuffer<short> _indexBuffer = null!;

    private int _vertexIndex;
    private int _indiceIndex;

    private readonly Vertex2D[] _vertices;

}
