using System.Text.Json;
using TaskTracker.Models;

namespace TaskTracker.Storage;

/// <summary>
/// Хранилище задач в JSON-файле.
/// </summary>
public sealed class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Создаёт репозиторий с указанным путём к файлу.
    /// </summary>
    /// <param name="filePath">Путь к файлу JSON.</param>
    public JsonTaskRepository(string filePath)
    {
        _filePath = filePath;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken token = default)
    {
        if (!File.Exists(_filePath))
        {
            return Array.Empty<TaskItem>();
        }

        try
        {
            await using var stream = File.OpenRead(_filePath);
            var tasks = await JsonSerializer.DeserializeAsync<List<TaskItem>>(stream, SerializerOptions, token);
            return tasks ?? new List<TaskItem>();
        }
        catch (JsonException)
        {
            // Битый файл не должен ронять приложение — возвращаем пустой список.
            return Array.Empty<TaskItem>();
        }
    }

    /// <inheritdoc />
    public async Task SaveAsync(IReadOnlyList<TaskItem> tasks, CancellationToken token = default)
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, tasks.ToList(), SerializerOptions, token);
    }
}
