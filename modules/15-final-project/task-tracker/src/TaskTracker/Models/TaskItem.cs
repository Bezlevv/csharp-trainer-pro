namespace TaskTracker.Models;

/// <summary>Статус задачи.</summary>
public enum TaskStatus
{
    /// <summary>Задача создана, работа не начата.</summary>
    Todo,

    /// <summary>Задача в работе.</summary>
    InProgress,

    /// <summary>Задача выполнена.</summary>
    Done
}

/// <summary>Приоритет задачи.</summary>
public enum TaskPriority
{
    /// <summary>Низкий приоритет.</summary>
    Low,

    /// <summary>Средний приоритет.</summary>
    Medium,

    /// <summary>Высокий приоритет.</summary>
    High
}

/// <summary>
/// Модель задачи трекера.
/// </summary>
public record TaskItem
{
    /// <summary>Уникальный идентификатор.</summary>
    public required Guid Id { get; init; }

    /// <summary>Краткое название задачи.</summary>
    public required string Title { get; init; }

    /// <summary>Подробное описание.</summary>
    public string Description { get; init; } = "";

    /// <summary>Текущий статус.</summary>
    public TaskStatus Status { get; init; } = TaskStatus.Todo;

    /// <summary>Приоритет.</summary>
    public TaskPriority Priority { get; init; } = TaskPriority.Medium;

    /// <summary>Момент создания.</summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>Плановый дедлайн (если задан).</summary>
    public DateTime? Deadline { get; init; }
}
