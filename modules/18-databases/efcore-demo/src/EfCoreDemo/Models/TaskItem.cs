namespace EfCoreDemo.Models;

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
/// Сущность задачи для EF Core.
/// </summary>
public class TaskItem
{
    /// <summary>Первичный ключ.</summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Название задачи.</summary>
    public string Title { get; set; } = "";

    /// <summary>Статус.</summary>
    public TaskStatus Status { get; set; } = TaskStatus.Todo;

    /// <summary>Момент создания.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
