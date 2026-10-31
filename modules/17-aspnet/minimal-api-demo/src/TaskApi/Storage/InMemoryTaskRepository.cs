using TaskApi.Models;

namespace TaskApi.Storage;

/// <summary>
/// Демонстрационное хранилище задач в памяти. В реальном проекте
/// его заменяют на репозиторий к БД (EF Core).
/// </summary>
public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _items = new();
    private readonly object _lock = new();

    /// <inheritdoc />
    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default)
    {
        lock (_lock)
        {
            return Task.FromResult<IReadOnlyList<TaskItem>>(_items.ToList());
        }
    }

    /// <inheritdoc />
    public Task AddAsync(TaskItem task, CancellationToken token = default)
    {
        lock (_lock)
        {
            _items.Add(task);
        }

        return Task.CompletedTask;
    }
}
