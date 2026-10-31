# Разбор кода — Модуль 17

> Формат: плохой веб-код → находим проблемы → рефакторим.

## Пример 1: Вся логика в `Program.cs`

### Исходный код

```csharp
app.MapPost("/api/orders", async (OrderRequest r) =>
{
    if (string.IsNullOrWhiteSpace(r.Title)) return Results.BadRequest();
    var order = new Order { Id = Guid.NewGuid(), Title = r.Title };
    // ... валидация, БД, email, логирование прямо здесь
    return Results.Created($"/api/orders/{order.Id}", order);
});
```

### Что не так

- Обработчик превращается в God-метод: и валидация, и бизнес-логика, и доступ к данным. Не тестируется, не переиспользуется.

### Как лучше

```csharp
app.MapPost("/api/orders", async (OrderRequest r, OrderService service, CancellationToken token) =>
{
    try
    {
        var order = await service.CreateAsync(r.Title, token);
        return Results.Created($"/api/orders/{order.Id}", order);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
```

### Вывод

Обработчик — тонкий; логика — в сервисе.

## Пример 2: `new` сервиса внутри обработчика

### Исходный код

```csharp
app.MapGet("/api/tasks", async () =>
{
    var service = new TaskService(new InMemoryTaskRepository());
    return Results.Ok(await service.GetAllAsync());
});
```

### Что не так

- Жёсткая привязка: нельзя подменить репозиторий для тестов. DI не используется.

### Как лучше

```csharp
app.MapGet("/api/tasks", async (TaskService service, CancellationToken token) =>
    Results.Ok(await service.GetAllAsync(token)));
```

### Вывод

Параметры-зависимости DI входят в сигнатуру обработчика.

## Пример 3: Всегда `OK` вместо корректных кодов

### Исходный код

```csharp
app.MapGet("/api/tasks/{id}", async (Guid id, TaskService service) =>
{
    var task = await service.GetByIdAsync(id);
    return Results.Ok(task); // даже если task == null
});
```

### Что не так

- Клиент не может понять, существует ли задача: получит 200 с null в теле.

### Как лучше

```csharp
return task is null ? Results.NotFound() : Results.Ok(task);
```

### Вывод

Используй семантически верные коды: 404, 400, 201.

## Пример 4: Синхронный вызов в async-цепочке

### Исходный код

```csharp
app.MapGet("/api/report", () =>
{
    var data = _repo.GetAll().Result; // блокирует поток
    return Results.Ok(data);
});
```

### Что не так

- Блокировка потока; под нагрузкой — исчерпание пула потоков и дедлоки.

### Как лучше

```csharp
app.MapGet("/api/report", async (ITaskRepository repo, CancellationToken token) =>
    Results.Ok(await repo.GetAllAsync(token)));
```

### Вывод

`async`/`await` до конца цепочки.

## Пример 5: Проглатывание исключений

### Исходный код

```csharp
try { return await service.GetAllAsync(); }
catch (Exception) { return Results.Ok(Array.Empty<TaskItem>()); }
```

### Что не так

- Баг маскируется пустым списком. Клиент не видит, что произошла ошибка.

### Как лучше

Пусть глобальный обработчик исключений вернёт 500, а в коде ловим только ожидаемые:

```csharp
try { ... }
catch (ArgumentException ex) { return Results.BadRequest(ex.Message); }
```

### Вывод

Лови ожидаемое; непредвиденное — 500 с логом.
