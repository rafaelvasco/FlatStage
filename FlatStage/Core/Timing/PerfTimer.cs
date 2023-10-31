using System.Diagnostics;

namespace FlatStage.Utils;
public static class PerfTimer
{
    private static readonly Stopwatch _timer = new();

    public static void Begin()
    {
        _timer.Restart();
    }

    public static long End()
    {
        _timer.Stop();

        return _timer.ElapsedMilliseconds;
    }
}
