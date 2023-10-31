using FlatStage.Editor;
using FlatStage.Platform;

if (args.Length > 0)
{
    CliExecutor.Process(args);
}
else
{
    using var editor = new Editor();
    editor.Run();
}
