using TaskTracker.Models;
using TaskTracker.Services;
using TaskStatus = TaskTracker.Models.TaskStatus;
using Xunit;

namespace TaskTracker.Tests;

/// <summary>
/// Тесты бизнес-логики TaskService.
/// </summary>
public class TaskServiceTests
{
    private readonly InMemoryTaskRepository _repository = new();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(_repository);
    }

    [Fact]
    public async Task CreateAsync_ValidInput_AddsTask()
    {
        var task = await _service.CreateAsync("Сделать отчёт", priority: TaskPriority.High);

        Assert.Equal("Сделать отчёт", task.Title);
        Assert.Equal(TaskStatus.Todo, task.Status);
        Assert.Equal(TaskPriority.High, task.Priority);

        var all = await _service.GetAsync();
        Assert.Single(all);
    }

    [Fact]
    public async Task CreateAsync_EmptyTitle_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync("   "));
    }

    [Fact]
    public async Task CreateAsync_DeadlineInPast_ThrowsArgumentException()
    {
        var past = DateTime.UtcNow.AddDays(-1);
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync("Задача", past));
    }

    [Fact]
    public async Task CompleteAsync_ExistingTask_MarksDone()
    {
        var task = await _service.CreateAsync("Задача");

        await _service.CompleteAsync(task.Id);

        var all = await _service.GetAsync();
        Assert.Equal(TaskStatus.Done, all.Single().Status);
    }

    [Fact]
    public async Task CompleteAsync_MissingTask_Throws()
    {
        await Assert.ThrowsAsync<InvalidTaskOperationException>(
            () => _service.CompleteAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task ChangePriorityAsync_ExistingTask_UpdatesPriority()
    {
        var task = await _service.CreateAsync("Задача");

        await _service.ChangePriorityAsync(task.Id, TaskPriority.Low);

        var all = await _service.GetAsync();
        Assert.Equal(TaskPriority.Low, all.Single().Priority);
    }

    [Fact]
    public async Task DeleteAsync_ExistingTask_Removes()
    {
        var task = await _service.CreateAsync("Задача");

        await _service.DeleteAsync(task.Id);

        var all = await _service.GetAsync();
        Assert.Empty(all);
    }

    [Fact]
    public async Task GetAsync_FilterByStatus_ReturnsMatchingOnly()
    {
        var todoTask = await _service.CreateAsync("Сделать");
        await _service.CreateAsync("В работе", priority: TaskPriority.High);
        await _service.CompleteAsync(todoTask.Id);

        var done = await _service.GetAsync(status: TaskStatus.Done);

        Assert.Single(done);
        Assert.Equal("Сделать", done.Single().Title);
    }

    [Fact]
    public async Task GetAsync_SortsByDeadlineFirst()
    {
        var now = DateTime.UtcNow;
        var later = await _service.CreateAsync("На потом", now.AddMonths(6));
        var sooner = await _service.CreateAsync("Срочно", now.AddDays(1));

        var tasks = await _service.GetAsync();

        Assert.Equal(sooner.Id, tasks[0].Id);
        Assert.Equal(later.Id, tasks[1].Id);
    }
}
