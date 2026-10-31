using TaskTracker.Models;

namespace TaskTracker.Storage;

/// <summary>
/// Контракт для хранилища задач. Абстракция позволяет подменять
/// реализацию (JSON, БД, в памяти) без изменения бизнес-логики.
/// </summary>
public interface ITaskRepository
{
    /// <summary>Возвращает все сохранённые задачи.</summary>
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default);

    /// <summary>Сохраняет полный список задач.</summary>
    Task SaveAsync(IReadOnlyList<TaskItem> tasks, CancellationToken token = default);
}
