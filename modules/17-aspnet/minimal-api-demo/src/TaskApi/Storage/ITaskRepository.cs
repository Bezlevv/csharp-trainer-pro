using TaskApi.Models;

namespace TaskApi.Storage;

/// <summary>
/// Контракт хранилища задач.
/// </summary>
public interface ITaskRepository
{
    /// <summary>Возвращает все задачи.</summary>
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default);

    /// <summary>Добавляет задачу.</summary>
    Task AddAsync(TaskItem task, CancellationToken token = default);
}
