namespace FlatStage.CLI;

using FlatStage;

public abstract class Executor
{
    public abstract void Execute(Span<string> parameters);

    protected static bool ParamExists(string key, Span<string> parameters)
    {
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (parameters[i] == key)
            {
                return true;
            }
        }

        return false;
    }

    protected static string GetParamValue(string key, Span<string> parameters)
    {
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (parameters[i] == key)
            {
                if (i < parameters.Length - 1)
                {
                    if (!parameters[i + 1].StartsWith("-"))
                    {
                        return parameters[i + 1];
                    }

                    break;
                }
            }
        }

        throw new Exception($"Could not get value for parameter {key}");
    }
}

public class BuildExecutor : Executor
{
    public override void Execute(Span<string> parameters)
    {
        if (parameters.Length != 2)
        {
            throw new ArgumentException("Invalid Build Usage: Example: Build -p [GameProjectPath]");
        }

        var path = GetParamValue("-p", parameters);

        if (!Directory.Exists(path))
        {
            throw new ApplicationException("Informed Path doesn't exist!");
        }

        AssetBuilder.BuildAssets(path);
    }
}

public class BuildSingleAssetExecutor : Executor
{
    public override void Execute(Span<string> parameters)
    {
        if (parameters.Length != 4)
        {
            throw new ArgumentException("Invalid Build Usage: Example: BuildAsset -p [GameProjectPath] -id [AssetId]");
        }

        var path = GetParamValue("-p", parameters);

        if (!Directory.Exists(path))
        {
            throw new ApplicationException("Informed Path doesn't exist!");
        }

        var assetId = GetParamValue("-id", parameters);

        AssetBuilder.BuildAsset(path, assetId);
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
