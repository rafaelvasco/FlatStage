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
    private static readonly StringBuilder _log = new ();

    private static string GetLogLinePrefix(LogLevel level)
    {
        return level switch
        {
            LogLevel.Info => "[INFO] ",
            LogLevel.Warning => "[WARNING] ",
            LogLevel.Error => "[ERROR] ",
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }

    public static void Write(string message, LogLevel level = LogLevel.Info)
    {
        _log.AppendLine(GetLogLinePrefix(level));
        _log.AppendLine(message);
    }

    internal static void Flush()
    {
        File.WriteAllText("log.txt", _log.ToString());
    }
}
