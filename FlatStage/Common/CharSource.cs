using System.Text;

namespace FlatStage;
internal struct CharSource
{
    private readonly string? _string;
    private readonly StringBuilder? _builder;

    public CharSource(string s)
    {
        _string = s;
        _builder = null;
        Length = s.Length;
    }

    public CharSource(StringBuilder builder)
    {
        _builder = builder;
        _string = null;
        Length = _builder.Length;
    }

    public readonly int Length;
    public char this[int index]
    {
        get
        {
            if (_string != null)
                return _string[index];
            return _builder![index];
        }
    }
}