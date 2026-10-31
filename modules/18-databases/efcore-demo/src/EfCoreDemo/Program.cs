using EfCoreDemo.Data;
using EfCoreDemo.Models;
using Microsoft.EntityFrameworkCore;
using TaskStatus = EfCoreDemo.Models.TaskStatus;

// Демонстрация EF Core с in-memory провайдером (без реальной БД).
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase("demo")
    .Options;

await using var db = new AppDbContext(options);

db.Tasks.Add(new TaskItem { Title = "Изучить EF Core" });
db.Tasks.Add(new TaskItem { Title = "Настроить миграции", Status = TaskStatus.InProgress });
await db.SaveChangesAsync();

Console.WriteLine("Задачи из БД:");
var tasks = await db.Tasks.AsNoTracking().OrderBy(t => t.Title).ToListAsync();
foreach (var task in tasks)
{
    Console.WriteLine($"- {task.Title} [{task.Status}]");
}
