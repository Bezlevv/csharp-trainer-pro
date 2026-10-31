namespace TaskTracker.Services;

/// <summary>
/// Исключение, сигнализирующее о некорректном состоянии задачи
/// (например, попытка завершить несуществующую задачу).
/// </summary>
public class InvalidTaskOperationException : Exception
{
    /// <summary>Создаёт исключение с сообщением.</summary>
    public InvalidTaskOperationException(string message) : base(message)
    {
    }
}
