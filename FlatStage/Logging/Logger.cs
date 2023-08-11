using System.IO;
using System.Text;

namespace FlatStage;

public enum LogLevel
{
    Info,
    Warning,
    Error
}

public static class Logger
{
    private static TextWriter? _writer;
    private static StringBuilder _log;

    public static void Write(string message, LogLevel level = LogLevel.Info)
    {
    }
}