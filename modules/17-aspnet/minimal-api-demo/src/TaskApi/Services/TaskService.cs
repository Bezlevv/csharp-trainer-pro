using TaskApi.Models;

namespace TaskApi.Services;

/// <summary>
/// Бизнес-логика задач для веб-API.
/// </summary>
public sealed class TaskService
{
    private readonly Storage.ITaskRepository _repository;

    /// <summary>Создаёт сервис с хранилищем.</summary>
    public TaskService(Storage.ITaskRepository repository) => _repository = repository;

    /// <summary>Возвращает все задачи.</summary>
    public Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default)
        => _repository.GetAllAsync(token);

    /// <summary>
    /// Создаёт задачу с валидацией.
    /// </summary>
    /// <exception cref="ArgumentException">Название пустое.</exception>
    public async Task<TaskItem> CreateAsync(string title, CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название задачи не может быть пустым.", nameof(title));
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title.Trim()
        };

        await _repository.AddAsync(task, token);
        return task;
    }
}
