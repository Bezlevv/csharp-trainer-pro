using EfCoreDemo.Data;
using EfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EfCoreDemo.Models.TaskStatus;
using Xunit;

namespace EfCoreDemo.Tests;

/// <summary>
/// Тесты EF Core-контекста на in-memory БД.
/// </summary>
public class AppDbContextTests
{
    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Add_ThenQuery_ReturnsTask()
    {
        await using var db = CreateContext();

        db.Tasks.Add(new TaskItem { Title = "Написать тест" });
        await db.SaveChangesAsync();

        var loaded = await db.Tasks.FirstOrDefaultAsync(t => t.Title == "Написать тест");

        Assert.NotNull(loaded);
        Assert.Equal(TaskStatus.Todo, loaded!.Status);
    }

    [Fact]
    public async Task AddTwo_ThenCount_ReturnsTwo()
    {
        await using var db = CreateContext();

        db.Tasks.Add(new TaskItem { Title = "Одна" });
        db.Tasks.Add(new TaskItem { Title = "Две" });
        await db.SaveChangesAsync();

        int count = await db.Tasks.CountAsync();

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task EmptyDb_Count_Zero()
    {
        await using var db = CreateContext();

        int count = await db.Tasks.CountAsync();

        Assert.Equal(0, count);
    }
}
