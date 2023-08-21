using System;
using System.Collections.Generic;
using System.IO;

using FlatStage.ContentPipeline;

namespace FlatStage.Platform;

public abstract class Executor
{
    public abstract void Execute(Span<string> parameters);
}

public class BuildExecutor : Executor
{
    public override void Execute(Span<string> parameters)
    {
        if (parameters.Length != 1)
        {
            throw new ArgumentException("Invalid Build Usage: Example: Build [GameProjectPath]");
        }

        var gameFolderPath = parameters[0];

        if (!Directory.Exists(gameFolderPath))
        {
            throw new ApplicationException("Informed Path doesn't exist!");
        }

        AssetBuilder.BuildAssets(gameFolderPath);
    }
}

public class BuildSingleAssetExecutor : Executor
{
    public override void Execute(Span<string> parameters)
    {
        if (parameters.Length != 2)
        {
            throw new ArgumentException("Invalid Build Usage: Example: BuildAsset [GameProjectPath] [AssetId]");
        }

        var gameFolderPath = parameters[0];

        if (!Directory.Exists(gameFolderPath))
        {
            throw new ApplicationException("Informed Path doesn't exist!");
        }

        var assetId = parameters[1];

        AssetBuilder.BuildAsset(gameFolderPath, assetId);

    }
}

public static class CliExecutor
{
    private static readonly Dictionary<string, Executor> Executors = new();

    static CliExecutor()
    {
        Executors.Add("BUILD", new BuildExecutor());
        Executors.Add("BUILDASSET", new BuildSingleAssetExecutor());
    }

    public static void Process(string[] args)
    {
        try
        {
            var command = args[0];
            var parameters = new Span<string>(args, 1, args.Length - 1);

            if (Executors.TryGetValue(command.ToUpper(), out var executor))
            {
                executor.Execute(parameters);
            }
        }
        catch (Exception e)
        {

            Console.WriteLine(e.Message);
        }
    }
}