# Практика — Модуль 18

> Работаем в проекте `efcore-demo`. Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Фильтрация по статусу

**Сложность:** ★☆☆
**Описание:** Выведи только задачи со статусом `InProgress`.

**Требования:**
- `Where(t => t.Status == TaskStatus.InProgress)`.
- `ToListAsync`.

## Задача 2: Обновление задачи

**Сложность:** ★★☆
**Описание:** Найди задачу по названию и смени ей статус.

**Требования:**
- `FirstOrDefaultAsync`, измени свойство, `SaveChangesAsync`.

## Задача 3: Удаление

**Сложность:** ★★☆
**Описание:** Удали задачу с конкретным названием.

**Требования:**
- `Remove(entity)` + `SaveChangesAsync`.

## Задача 4: Счётчик

**Сложность:** ★☆☆
**Описание:** Посчитай общее число задач.

**Требования:**
- `CountAsync`.

## Задача 5: Пагинация

**Сложность:** ★★★
**Описание:** Выведи вторую «страницу» по 2 задачи.

**Требования:**
- `OrderBy` → `Skip(2)` → `Take(2)`.

## Задача 6: Тест на обновление

**Сложность:** ★★★
**Описание:** Напиши тест: добавить → сменить статус → проверить.

**Требования:**
- In-memory БД с уникальным именем.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
var inProgress = await db.Tasks
    .Where(t => t.Status == TaskStatus.InProgress)
    .ToListAsync();

foreach (var t in inProgress) Console.WriteLine(t.Title);
```

**Пояснение:** `Where` транслируется в SQL `WHERE`.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
var task = await db.Tasks.FirstOrDefaultAsync(t => t.Title == "Изучить EF Core");
if (task is not null)
{
    task.Status = TaskStatus.Done;
    await db.SaveChangesAsync();
}
```

**Пояснение:** Изменения в отслеживаемой сущности фиксируются `SaveChangesAsync`.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
var task = await db.Tasks.FirstOrDefaultAsync(t => t.Title == "Старая");
if (task is not null)
{
    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
}
```

**Пояснение:** `Remove` помечает на удаление; `SaveChangesAsync` выполняет DELETE.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
int count = await db.Tasks.CountAsync();
Console.WriteLine(count);
```

**Пояснение:** `CountAsync` не «грузит» сущности — генерирует `SELECT COUNT(*)`.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
var page2 = await db.Tasks
    .OrderBy(t => t.Title)
    .Skip(2)
    .Take(2)
    .ToListAsync();
```

**Пояснение:** `Skip`/`Take` транслируются в SQL `OFFSET`/`LIMIT`.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
[Fact]
public async Task Update_Status_IsPersisted()
{
    var options = new DbContextOptionsBuilder<AppDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    await using (var db = new AppDbContext(options))
    {
        db.Tasks.Add(new TaskItem { Title = "Тест" });
        await db.SaveChangesAsync();
    }

    await using (var db = new AppDbContext(options))
    {
        var task = await db.Tasks.FirstAsync();
        task.Status = TaskStatus.Done;
        await db.SaveChangesAsync();
    }

    await using (var db = new AppDbContext(options))
    {
        var loaded = await db.Tasks.FirstAsync();
        Assert.Equal(TaskStatus.Done, loaded.Status);
    }
}
```

**Пояснение:** Один InMemory-контекст на имя БД сохраняет данные между экземплярами контекста.

</details>
