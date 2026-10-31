# Практика — Модуль 17

> Работаем в проекте `minimal-api-demo`. Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Добавить `GET /api/tasks/{id}`

**Сложность:** ★★☆
**Описание:** Верни задачу по id или 404, если её нет.

**Требования:**
- `TaskService.GetByIdAsync(Guid id)`.
- `Results.Ok` / `Results.NotFound`.

## Задача 2: `DELETE /api/tasks/{id}`

**Сложность:** ★★☆
**Описание:** Удали задачу; 204 при успехе, 404 если нет.

**Требования:**
- `Results.NoContent()` — это 204.
- Добавь `RemoveAsync` в `ITaskRepository`.

## Задача 3: Логирование через middleware

**Сложность:** ★★☆
**Описание:** Добавь middleware, логирующий метод и путь каждого запроса.

**Требования:**
- `app.Use(...)`, выводи через `app.Logger.LogInformation`.

## Задача 4: Валидация через guard

**Сложность:** ★★☆
**Описание:** В `TaskService` добавь проверку: id не пустой Guid → `ArgumentException`.

**Требования:**
- Покрой тестом: `GET /api/tasks/00000000-0000-0000-0000-000000000000` → `400`.

## Задача 5: Тест на 404

**Сложность:** ★★★
**Описание:** Напиши интеграционный тест: запрос к несуществующему id возвращает 404.

**Требования:**
- Используй `WebApplicationFactory<Program>` и случайный Guid.

## Задача 6: Средний балл задач

**Сложность:** ★★★
**Описание:** Добавь `GET /api/tasks/stats` — число задач и сколько завершено.

**Требования:**
- Верни объект `TaskStats { Total, Done }`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
// в TaskService:
public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken token = default)
{
    var tasks = await _repository.GetAllAsync(token);
    return tasks.FirstOrDefault(t => t.Id == id);
}

// в Program.cs:
tasks.MapGet("/{id:guid}", async (Guid id, TaskService service, CancellationToken token) =>
{
    var task = await service.GetByIdAsync(id, token);
    return task is null ? Results.NotFound() : Results.Ok(task);
});
```

**Пояснение:** Маршрут `{id:guid}` парсит Guid автоматически. `Results.NotFound()` — 404.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
// в ITaskRepository:
Task<bool> RemoveAsync(Guid id, CancellationToken token = default);

// в InMemoryTaskRepository:
public Task<bool> RemoveAsync(Guid id, CancellationToken token = default)
{
    lock (_lock)
    {
        return Task.FromResult(_items.RemoveAll(t => t.Id == id) > 0);
    }
}

// в Program.cs:
tasks.MapDelete("/{id:guid}", async (Guid id, Storage.ITaskRepository repo, CancellationToken token) =>
    await repo.RemoveAsync(id, token) ? Results.NoContent() : Results.NotFound());
```

**Пояснение:** `RemoveAll` возвращает число удалённых; если > 0 — 204, иначе 404.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
app.Use(async (context, next) =>
{
    app.Logger.LogInformation("Запрос: {Method} {Path}", context.Request.Method, context.Request.Path);
    await next();
});
```

**Пояснение:** Middleware логирует каждый запрос до передачи дальше по конвейеру.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken token = default)
{
    if (id == Guid.Empty)
    {
        throw new ArgumentException("id не может быть пустым.", nameof(id));
    }

    var tasks = await _repository.GetAllAsync(token);
    return tasks.FirstOrDefault(t => t.Id == id);
}

// обработчик:
tasks.MapGet("/{id:guid}", async (Guid id, TaskService service, CancellationToken token) =>
{
    try
    {
        var task = await service.GetByIdAsync(id, token);
        return task is null ? Results.NotFound() : Results.Ok(task);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
```

**Пояснение:** Guard-проверка бросает исключение, обработчик ловит и возвращает 400.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
[Fact]
public async Task Get_MissingId_ReturnsNotFound()
{
    using var client = new WebApplicationFactory<Program>().CreateClient();

    var response = await client.GetAsync($"/api/tasks/{Guid.NewGuid()}");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
}
```

**Пояснение:** Случайный Guid почти наверняка отсутствует в модели, поэтому получаем 404.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
// модель:
public record TaskStats(int Total, int Done);

// в TaskService:
public async Task<TaskStats> GetStatsAsync(CancellationToken token = default)
{
    var tasks = await _repository.GetAllAsync(token);
    return new TaskStats(tasks.Count, tasks.Count(t => t.Status == TaskStatus.Done));
}

// в Program.cs:
tasks.MapGet("/stats", async (TaskService service, CancellationToken token) =>
    Results.Ok(await service.GetStatsAsync(token)));
```

**Пояснение:** `record` удобен как DTO с автоматической сериализацией в JSON.

</details>
