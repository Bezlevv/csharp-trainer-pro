# Разбор кода — Модуль 15

> Формат: частые ошибки в подобных проектах → как избежать.

## Пример 1: Вся логика в `Program.cs`

### Плохо

```csharp
// Program.cs на 400 строк: и меню, и расчёты, и JsonSerializer, и Console
```

### Почему

- Входная точка превращается в God-класс: трудно читать, тестировать, расширять.

### Как надо

Тонкий `Program.cs` с DI-сборкой + отдельные слои. Так и сделано в проекте.

## Пример 2: Бизнес-логика знает про JSON

### Плохо

```csharp
class TaskService
{
    public void Save() => File.WriteAllText("tasks.json", JsonSerializer.Serialize(...));
}
```

### Почему

- Сервис привязан к конкретному формату. Смена хранилища (например, на SQLite) — переписывание всего сервиса.

### Как надо

`ITaskRepository` + `JsonTaskRepository`. Сервис зависит от абстракции (DIP).

## Пример 3: `DateTime.Now` в сервисе

### Плохо

```csharp
if (deadline <= DateTime.Now) throw ...;
```

### Почему

- Непредсказуемые тесты и поведение. Локальное время не для логики.

### Как надо

`DateTime.UtcNow` — или, лучше, `IClock` (задача 5 практики) для полной детерминированности.

## Пример 4: Битый JSON — молча

### Плохо

```csharp
catch (Exception)
{
    // ничего — тихо теряем данные
}
```

### Почему

- Пользователь не узнает о проблеме; приложение продолжит работу с «пустыми» данными.

### Как надо

- Ловить `JsonException` (не `Exception` всё подряд).
- Хотя бы диагностировать (`Console.Error.WriteLine`), не падая.

## Пример 5: Возврат `List<T>` наружу

### Плохо

```csharp
public List<TaskItem> GetAll() => _items;
```

### Почему

- Потребитель может изменить хранилище напрямую, минуя бизнес-логику.

### Как надо

```csharp
public IReadOnlyList<TaskItem> GetAll() => _items;
```

## Пример 6: Всё `public` без XML-доков

### Плохо

```csharp
public class TaskService
{
    public Task<TaskItem> CreateAsync(...) // без <summary>
}
```

### Почему

- С `TreatWarningsAsErrors` + `GenerateDocumentationFile` проект просто не соберётся (CS1591). Документация — не роскошь, а требование.

### Как надо

Публичные члены — с XML-доками. Так и сделано в проекте.
