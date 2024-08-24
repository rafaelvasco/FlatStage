namespace FlatStage;

public abstract class Asset(string id) : Disposable
{
    public string Id { get; } = id;
}
