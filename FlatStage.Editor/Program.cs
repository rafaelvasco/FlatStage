using FlatStage;
using FlatStage.Editor;


if (args.Length > 0)
{
    CliExecutor.Process(args);
}
else
{
    using var stage = new Stage();
    stage.Run(new Editor());
}