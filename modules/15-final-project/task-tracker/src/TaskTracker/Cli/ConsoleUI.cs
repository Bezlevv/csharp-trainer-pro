using TaskTracker.Models;
using TaskTracker.Services;
using TaskStatus = TaskTracker.Models.TaskStatus;

namespace TaskTracker.Cli;

/// <summary>
/// Консольный интерфейс трекера задач.
/// </summary>
public sealed class ConsoleUI
{
    private readonly TaskService _service;

    /// <summary>
    /// Создаёт UI с указанным сервисом.
    /// </summary>
    public ConsoleUI(TaskService service)
    {
        _service = service;
    }

    /// <summary>
    /// Запускает цикл меню.
    /// </summary>
    public async Task RunAsync(CancellationToken token = default)
    {
        while (true)
        {
            token.ThrowIfCancellationRequested();
            PrintMenu();

            var choice = Console.ReadLine()?.Trim();
            switch (choice)
            {
                case "1": await ShowTasksAsync(token); break;
                case "2": await AddTaskAsync(token); break;
                case "3": await CompleteTaskAsync(token); break;
                case "4": await ChangePriorityAsync(token); break;
                case "5": await DeleteTaskAsync(token); break;
                case "6": await ShowFilteredAsync(token); break;
                case "0": return;
                default: Console.WriteLine("Неверная команда."); break;
            }
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== Трекер задач ===");
        Console.WriteLine("1. Показать задачи");
        Console.WriteLine("2. Добавить задачу");
        Console.WriteLine("3. Завершить задачу");
        Console.WriteLine("4. Изменить приоритет");
        Console.WriteLine("5. Удалить задачу");
        Console.WriteLine("6. Фильтр по статусу");
        Console.WriteLine("0. Выход");
        Console.Write("Выбор: ");
    }

    private async Task ShowTasksAsync(CancellationToken token)
    {
        var tasks = await _service.GetAsync(token: token);
        PrintTasks(tasks);
    }

    private static void PrintTasks(IReadOnlyList<TaskItem> tasks)
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("Задач нет.");
            return;
        }

        foreach (var task in tasks)
        {
            var statusIcon = task.Status switch
            {
                TaskStatus.Done => "[x]",
                TaskStatus.InProgress => "[→]",
                _ => "[ ]"
            };

            var deadline = task.Deadline is { } d ? d.ToString("dd.MM.yyyy") : "—";
            Console.WriteLine($"{statusIcon} {task.Title} | {task.Priority} | дедлайн: {deadline} | id: {task.Id}");
        }
    }

    private async Task AddTaskAsync(CancellationToken token)
    {
        Console.Write("Название: ");
        var title = Console.ReadLine() ?? "";

        Console.Write("Приоритет (low/medium/high, по умолчанию medium): ");
        var priorityInput = Console.ReadLine()?.Trim().ToLower();
        var priority = priorityInput switch
        {
            "low" => TaskPriority.Low,
            "high" => TaskPriority.High,
            _ => TaskPriority.Medium
        };

        Console.Write("Дедлайн (ГГГГ-ММ-ДД, пусто — нет): ");
        var deadlineInput = Console.ReadLine()?.Trim();
        DateTime? deadline = null;
        if (!string.IsNullOrWhiteSpace(deadlineInput) && DateTime.TryParse(deadlineInput, out var parsed))
        {
            deadline = parsed;
        }

        try
        {
            var task = await _service.CreateAsync(title, deadline, priority, token);
            Console.WriteLine($"Создана задача: {task.Title} (id: {task.Id})");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private async Task CompleteTaskAsync(CancellationToken token)
    {
        var id = ReadTaskId("id завершаемой задачи");
        if (id is null) return;

        try
        {
            await _service.CompleteAsync(id.Value, token);
            Console.WriteLine("Задача завершена.");
        }
        catch (InvalidTaskOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private async Task ChangePriorityAsync(CancellationToken token)
    {
        var id = ReadTaskId("id задачи");
        if (id is null) return;

        Console.Write("Новый приоритет (low/medium/high): ");
        var input = Console.ReadLine()?.Trim().ToLower();
        var priority = input switch
        {
            "low" => TaskPriority.Low,
            "high" => TaskPriority.High,
            _ => TaskPriority.Medium
        };

        try
        {
            await _service.ChangePriorityAsync(id.Value, priority, token);
            Console.WriteLine("Приоритет обновлён.");
        }
        catch (InvalidTaskOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private async Task DeleteTaskAsync(CancellationToken token)
    {
        var id = ReadTaskId("id удаляемой задачи");
        if (id is null) return;

        try
        {
            await _service.DeleteAsync(id.Value, token);
            Console.WriteLine("Задача удалена.");
        }
        catch (InvalidTaskOperationException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private async Task ShowFilteredAsync(CancellationToken token)
    {
        Console.Write("Статус (todo/inprogress/done): ");
        var input = Console.ReadLine()?.Trim().ToLower();
        TaskStatus? status = input switch
        {
            "todo" => TaskStatus.Todo,
            "inprogress" => TaskStatus.InProgress,
            "done" => TaskStatus.Done,
            _ => null
        };

        var tasks = await _service.GetAsync(status: status, token: token);
        PrintTasks(tasks);
    }

    private static Guid? ReadTaskId(string prompt)
    {
        Console.Write($"Введите {prompt}: ");
        var line = Console.ReadLine()?.Trim();
        if (Guid.TryParse(line, out var id))
        {
            return id;
        }

        Console.WriteLine("Некорректный id.");
        return null;
    }
}
