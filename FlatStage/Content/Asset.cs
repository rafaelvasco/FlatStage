namespace FlatStage;

public abstract class Asset : Disposable
{
    public string Id { get; }

    protected Asset(string id)
    {
        Id = id;
    }
}