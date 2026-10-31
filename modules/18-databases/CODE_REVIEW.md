# Разбор кода — Модуль 18

> Формат: плохой код с EF Core → находим проблемы → рефакторим.

## Пример 1: `ToList()` в цикле

### Исходный код

```csharp
foreach (var title in titles)
{
    var tasks = await db.Tasks.Where(t => t.Title == title).ToListAsync();
    // обработка...
}
```

### Что не так

- Запрос к БД на каждой итерации — N запросов вместо одного (N+1).

### Как лучше

```csharp
var tasks = await db.Tasks.Where(t => titles.Contains(t.Title)).ToListAsync();
```

### Вывод

Один запрос `WHERE IN (...)` вместо N отдельных.

## Пример 2: Синхронные методы EF

### Исходный код

```csharp
var tasks = db.Tasks.ToList(); // блокирует поток
```

### Что не так

- Блокировка потока под нагрузкой. Все методы EF имеют async-версии.

### Как лучше

```csharp
var tasks = await db.Tasks.ToListAsync();
```

### Вывод

`async`/`await` для всех операций с БД.

## Пример 3: Сравнение enum со строкой

### Исходный код

```csharp
db.Tasks.Where(t => t.Status.ToString() == "Done")
```

### Что не так

- `ToString()` не транслируется в SQL корректно; зависит от культуры/регистра.

### Как лучше

```csharp
db.Tasks.Where(t => t.Status == TaskStatus.Done)
```

### Вывод

Используй типизированные enum-сравнения.

## Пример 4: Загрузка всех сущностей для одной

### Исходный код

```csharp
var all = await db.Tasks.ToListAsync();
var one = all.FirstOrDefault(t => t.Id == id);
```

### Что не так

- Тянем всю таблицу в память ради одной строки.

### Как лучше

```csharp
var one = await db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
```

### Вывод

Пусть запрос сужает выборку на стороне БД.

## Пример 5: Забытый `SaveChangesAsync`

### Исходный код

```csharp
db.Tasks.Add(new TaskItem { Title = "Без сохранения" });
// нет SaveChangesAsync() — добавление потеряется
```

### Что не так

- Изменения не применяются к БД, пока не вызван `SaveChangesAsync`.

### Как лучше

```csharp
db.Tasks.Add(new TaskItem { Title = "С сохранением" });
await db.SaveChangesAsync();
```

### Вывод

`SaveChangesAsync` — обязательный шаг после изменений.
