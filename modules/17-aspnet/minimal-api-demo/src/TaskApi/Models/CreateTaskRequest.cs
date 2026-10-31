namespace TaskApi.Models;

/// <summary>
/// Тело запроса на создание задачи.
/// </summary>
public record CreateTaskRequest
{
    /// <summary>Название новой задачи.</summary>
    public required string Title { get; init; }
}
