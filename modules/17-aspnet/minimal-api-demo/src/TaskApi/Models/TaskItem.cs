namespace TaskApi.Models;

/// <summary>Статус задачи.</summary>
public enum TaskStatus
{
    /// <summary>Создана.</summary>
    Todo,

    /// <summary>В работе.</summary>
    InProgress,

    /// <summary>Выполнена.</summary>
    Done
}

/// <summary>
/// Модель задачи для веб-API.
/// </summary>
public record TaskItem
{
    /// <summary>Уникальный идентификатор.</summary>
    public required Guid Id { get; init; }

    /// <summary>Название задачи.</summary>
    public required string Title { get; init; }

    /// <summary>Текущий статус.</summary>
    public TaskStatus Status { get; init; } = TaskStatus.Todo;

    /// <summary>Момент создания.</summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
