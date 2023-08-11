using System;

namespace FlatStage;

public abstract class Disposable : IDisposable
{
    ~Disposable()
    {
        Dispose();
        Console.WriteLine("Not properly disposed");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        Free();
    }

    protected abstract void Free();
}