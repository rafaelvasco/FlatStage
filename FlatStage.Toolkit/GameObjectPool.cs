namespace FlatStage.Toolkit;

internal static class GameObjectPool
{
    private static readonly Dictionary<Type, Pool<GameObject>> _pools = new();

    // public static T Get<T>() where T : GameObject
    // {
    //     if (_pools.TryGetValue(typeof(T), out var pool))
    //     {
    //         return (pool.Get() as T)!;
    //     }
    //
    //     var newPool = new Pool<GameObject>(() => GameObjectCreator.Create<T>(""), 1024);
    //
    //     _pools.Add(typeof(T), newPool);
    //
    //     return (newPool.Get() as T)!;
    // }

    public static bool Return<T>(T item) where T : GameObject
    {
        return _pools.TryGetValue(typeof(T), out var pool) && pool.Return(item);
    }

    public static void Clear<T>()
    {
        if (_pools.TryGetValue(typeof(T), out var pool))
        {
            pool.Clear();
        }
    }
}
