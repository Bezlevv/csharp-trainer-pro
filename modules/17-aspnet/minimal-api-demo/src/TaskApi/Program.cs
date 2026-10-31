using TaskApi.Models;
using TaskApi.Services;
using TaskApi.Storage;

var builder = WebApplication.CreateBuilder(args);

// Регистрация зависимостей во встроенном контейнере DI.
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<TaskService>();

var app = builder.Build();

// Статические файлы (wwwroot/index.html) для интерактивного демо.
app.UseDefaultFiles();
app.UseStaticFiles();

// Группа маршрутов /api/tasks.
var tasks = app.MapGroup("/api/tasks");

// GET /api/tasks — список задач.
tasks.MapGet("/", async (TaskService service, CancellationToken token) =>
    Results.Ok(await service.GetAllAsync(token)));

// POST /api/tasks — создать задачу.
tasks.MapPost("/", async (CreateTaskRequest request, TaskService service, CancellationToken token) =>
{
    try
    {
        var created = await service.CreateAsync(request.Title, token);
        return Results.Created($"/api/tasks/{created.Id}", created);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

/// <summary>Точка входа для интеграционных тестов (WebApplicationFactory).</summary>
public partial class Program { }
