using TaskTracker.Models;
using TaskTracker.Storage;
using TaskStatus = TaskTracker.Models.TaskStatus;
using Xunit;

namespace TaskTracker.Tests;

/// <summary>
/// Интеграционные тесты JSON-хранилища на временном файле.
/// </summary>
public class JsonTaskRepositoryTests : IDisposable
{
    private readonly string _tempFile;

    public JsonTaskRepositoryTests()
    {
        _tempFile = Path.Combine(Path.GetTempPath(), $"tasks_{Guid.NewGuid():N}.json");
    }

    [Fact]
    public async Task SaveThenLoad_RoundTrip_ReturnsSameData()
    {
        var repository = new JsonTaskRepository(_tempFile);

        var tasks = new List<TaskItem>
        {
            new() { Id = Guid.NewGuid(), Title = "Первая", Priority = TaskPriority.High },
            new() { Id = Guid.NewGuid(), Title = "Вторая", Status = TaskStatus.Done }
        };

        await repository.SaveAsync(tasks);
        var loaded = await repository.GetAllAsync();

        Assert.Equal(2, loaded.Count);
        Assert.Equal(tasks[0].Title, loaded[0].Title);
        Assert.Equal(tasks[0].Priority, loaded[0].Priority);
        Assert.Equal(TaskStatus.Done, loaded[1].Status);
    }

    [Fact]
    public async Task GetAll_MissingFile_ReturnsEmpty()
    {
        var repository = new JsonTaskRepository(_tempFile);

        var loaded = await repository.GetAllAsync();

        Assert.Empty(loaded);
    }

    [Fact]
    public async Task GetAll_CorruptFile_ReturnsEmptyWithoutThrow()
    {
        await File.WriteAllTextAsync(_tempFile, "это не json {{{");

        var repository = new JsonTaskRepository(_tempFile);

        var loaded = await repository.GetAllAsync();

        Assert.Empty(loaded);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile))
        {
            File.Delete(_tempFile);
        }
    }
}
