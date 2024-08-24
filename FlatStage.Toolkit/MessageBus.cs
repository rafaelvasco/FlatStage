namespace FlatStage.Toolkit;
public static class MessageBus
{
    private static readonly FastDictionary<int, List<Action>> _messageHandlers = new();

    public static void On(int messageCode, Action action)
    {
        if (!_messageHandlers.ContainsKey(messageCode))
        {
            _messageHandlers[messageCode] = new List<Action>();
        }

        _messageHandlers[messageCode].Add(action);
    }

    public static void Emit(int messageCode)
    {
        if (!_messageHandlers.TryGetValue(messageCode, out var handlers)) return;
        foreach (var handler in handlers)
        {
            handler.Invoke();
        }
    }
}
