using TaskTracker.Models;
using TaskTracker.Storage;

namespace TaskTracker.Tests;

/// <summary>
/// Тестовое хранилище в памяти — заменяет JSON-файл в юнит-тестах.
/// </summary>
internal sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly List<TaskItem> _items = new();

    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default)
        => Task.FromResult<IReadOnlyList<TaskItem>>(_items.ToList());

    public Task SaveAsync(IReadOnlyList<TaskItem> tasks, CancellationToken token = default)
    {
        _items.Clear();
        _items.AddRange(tasks);
        return Task.CompletedTask;
    }
}
