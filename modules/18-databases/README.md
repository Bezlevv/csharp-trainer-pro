# Модуль 18: Базы данных и EF Core

> **Цель:** работать с реляционными БД через ORM и понимать миграции.

## Зачем это нужно

Ни одно серьёзное приложение не обходится без хранения данных. Веб-API из модуля 17 использовало in-memory хранилище — в реальности данные живут в базе. **Entity Framework Core (EF Core)** — ORM от Microsoft: ты работаешь с C#-объектами, а EF Core сам превращает их в SQL-запросы. Это — стандарт индустрии для .NET.

## Теория

### 1. Что такое ORM и EF Core

**ORM** (Object-Relational Mapping) — прослойка между объектами C# и таблицами БД.

**EF Core** даёт:
- `DbContext` — модель базы как C#-класс;
- `DbSet<T>` — представление таблицы;
- **миграции** — версионирование схемы БД;
- **LINQ-запросы** — транслируются в SQL.

### 2. Провайдеры

EF Core требует провайдер под конкретную БД:

| Провайдер | БД |
|---|---|
| `Microsoft.EntityFrameworkCore.SqlServer` | SQL Server |
| `Microsoft.EntityFrameworkCore.Sqlite` | SQLite (файловая) |
| `Microsoft.EntityFrameworkCore.InMemory` | in-memory (тесты, демо) |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | PostgreSQL |

Устанавливается пакетом: `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`.

### 3. Контекст и сущности

```csharp
public class AppDbContext : DbContext
{
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}
```

Сущность:

```csharp
public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "";
    public TaskStatus Status { get; set; }
}
```

### 4. Запросы через LINQ

```csharp
var openTasks = await db.Tasks
    .Where(t => t.Status == TaskStatus.Todo)
    .OrderBy(t => t.Title)
    .ToListAsync();
```

EF Core переводит LINQ в SQL. **Важно:** `IQueryable<T>` — отложенный запрос; `ToListAsync`/`FirstOrDefaultAsync` материализуют.

### 5. `AsNoTracking`

Для чтения без отслеживания изменений:

```csharp
var snapshot = await db.Tasks.AsNoTracking().ToListAsync();
```

Ускоряет чистое чтение.

### 6. Миграции

Миграция — «слепок» схемы БД:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Команды генерируют классы-миграции, из которых EF Core строит таблицы.

### 7. Сохранение

```csharp
db.Tasks.Add(new TaskItem { Title = "Новая" });
await db.SaveChangesAsync();   // выполняет INSERT
```

Все изменения в контексте применяются одним `SaveChangesAsync`.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `ToList()` в цикле | выполнение запроса на каждой итерации | материализация один раз |
| Не `async`-методы EF | блокировка потока | `ToListAsync`, `SaveChangesAsync` |
| Хранение `decimal`/даты как строк | потери точности/сортировки | правильные типы столбцов |
| `AsNoTracking` забыт для чтения | лишние накладные на отслеживание | добавлять для чисто чтения |

## Контрольные вопросы

1. Что такое ORM?
2. Что делает `DbContext`?
3. Чем `IQueryable<T>` отличается от материализованного списка?
4. Что такое миграции?
5. Когда нужен `AsNoTracking`?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки
- Рабочий пример: [`efcore-demo`](efcore-demo)
