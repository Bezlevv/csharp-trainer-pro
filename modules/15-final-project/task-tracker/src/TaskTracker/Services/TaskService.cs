using TaskTracker.Models;
using TaskTracker.Storage;
using TaskStatus = TaskTracker.Models.TaskStatus;

namespace TaskTracker.Services;

/// <summary>
/// Бизнес-логика работы с задачами.
/// </summary>
public sealed class TaskService
{
    private readonly ITaskRepository _repository;

    /// <summary>
    /// Создаёт сервис с указанным хранилищем.
    /// </summary>
    /// <param name="repository">Хранилище задач.</param>
    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Создаёт задачу, проверяя корректность входных данных.
    /// </summary>
    /// <param name="title">Название задачи.</param>
    /// <param name="deadline">Дедлайн (по желанию).</param>
    /// <param name="priority">Приоритет.</param>
    /// <param name="token">Токен отмены.</param>
    /// <returns>Созданная задача.</returns>
    /// <exception cref="ArgumentException">Название пустое или дедлайн в прошлом.</exception>
    public async Task<TaskItem> CreateAsync(
        string title,
        DateTime? deadline = null,
        TaskPriority priority = TaskPriority.Medium,
        CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Название задачи не может быть пустым.", nameof(title));
        }

        if (deadline is not null && deadline <= DateTime.UtcNow)
        {
            throw new ArgumentException("Дедлайн должен быть в будущем.", nameof(deadline));
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = title.Trim(),
            Deadline = deadline,
            Priority = priority,
            CreatedAt = DateTime.UtcNow,
            Status = TaskStatus.Todo
        };

        var tasks = (await _repository.GetAllAsync(token)).ToList();
        tasks.Add(task);
        await _repository.SaveAsync(tasks, token);
        return task;
    }

    /// <summary>
    /// Возвращает задачи, опционально отфильтрованные по статусу и приоритету.
    /// </summary>
    public async Task<IReadOnlyList<TaskItem>> GetAsync(
        TaskStatus? status = null,
        TaskPriority? priority = null,
        CancellationToken token = default)
    {
        var tasks = await _repository.GetAllAsync(token);

        var query = tasks.AsEnumerable();

        if (status is not null)
        {
            query = query.Where(t => t.Status == status);
        }

        if (priority is not null)
        {
            query = query.Where(t => t.Priority == priority);
        }

        return query
            .OrderBy(t => t.Deadline ?? DateTime.MaxValue)
            .ThenBy(t => t.Title)
            .ToList();
    }

    /// <summary>
    /// Отмечает задачу как выполненную.
    /// </summary>
    /// <exception cref="InvalidTaskOperationException">Задача не найдена.</exception>
    public async Task CompleteAsync(Guid id, CancellationToken token = default)
    {
        var tasks = (await _repository.GetAllAsync(token)).ToList();
        var index = tasks.FindIndex(t => t.Id == id);
        if (index < 0)
        {
            throw new InvalidTaskOperationException("Задача не найдена.");
        }

        var current = tasks[index];
        tasks[index] = current with { Status = TaskStatus.Done };
        await _repository.SaveAsync(tasks, token);
    }

    /// <summary>
    /// Меняет приоритет задачи.
    /// </summary>
    /// <exception cref="InvalidTaskOperationException">Задача не найдена.</exception>
    public async Task ChangePriorityAsync(Guid id, TaskPriority priority, CancellationToken token = default)
    {
        var tasks = (await _repository.GetAllAsync(token)).ToList();
        var index = tasks.FindIndex(t => t.Id == id);
        if (index < 0)
        {
            throw new InvalidTaskOperationException("Задача не найдена.");
        }

        tasks[index] = tasks[index] with { Priority = priority };
        await _repository.SaveAsync(tasks, token);
    }

    /// <summary>
    /// Удаляет задачу.
    /// </summary>
    /// <exception cref="InvalidTaskOperationException">Задача не найдена.</exception>
    public async Task DeleteAsync(Guid id, CancellationToken token = default)
    {
        var tasks = (await _repository.GetAllAsync(token)).ToList();
        var index = tasks.FindIndex(t => t.Id == id);
        if (index < 0)
        {
            throw new InvalidTaskOperationException("Задача не найдена.");
        }

        tasks.RemoveAt(index);
        await _repository.SaveAsync(tasks, token);
    }
}
