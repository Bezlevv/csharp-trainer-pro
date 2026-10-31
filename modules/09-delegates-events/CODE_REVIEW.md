# Разбор кода — Модуль 09

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: Делегат-поле вместо `event`

### Исходный код

```csharp
class OrderService
{
    public Action<string>? OrderPlaced; // не event!
}
```

### Что не так

- Любой код может **присвоить**: `service.OrderPlaced = DoSomething;` — стирая всех остальных подписчиков. Отписка и множественная подписка работают некорректно.

### Как лучше

```csharp
class OrderService
{
    public event Action<string>? OrderPlaced;
}
```

### Вывод

`event` ограничивает: снаружи доступны только `+=` и `-=`, нельзя присвоить и затереть.

## Пример 2: Публикация без null-проверки

### Исходный код

```csharp
OrderPlaced?.Invoke(this, id); // ок
// а вот так — НЕЛЬЗЯ:
OrderPlaced.Invoke(this, id);  // NullReferenceException, если нет подписчиков
```

### Что не так

- Если подписчиков нет, `Invoke` падает. Нужно проверять на `null`.

### Как лучше

```csharp
OrderPlaced?.Invoke(this, id);
```

### Вывод

Всегда публикуй через `?.Invoke(...)`.

## Пример 3: Лямбда вместо именованного метода

### Исходный код

```csharp
service.Moved += (s, e) => EmailClient.Send(GetEmail());
service.Moved += (s, e) => EmailClient.Send(GetEmail());
```

### Что не так

- Одна и та же лямбда, подписанная дважды, выполнится дважды. Отписаться от неё невозможно (нет ссылки на метод).

### Как лучше

```csharp
void OnMoved(object? s, EventArgs e) => EmailClient.Send(GetEmail());

service.Moved += OnMoved;
// ... контроль подписки и отписка:
service.Moved -= OnMoved;
```

### Вывод

Переиспользуемые обработчики — отдельные методы, а не анонимные лямбды.

## Пример 4: Замыкание на переменную цикла

### Исходный код

```csharp
var actions = new List<Action>();
for (int i = 0; i < 3; i++)
{
    actions.Add(() => Console.WriteLine(i));
}

foreach (var a in actions) a(); // 3 3 3 — а НЕ 0 1 2
```

### Что не так

- Лямбда захватывает `i` по ссылке. К моменту вызова цикл уже закончился, `i == 3`.

### Как лучше

```csharp
for (int i = 0; i < 3; i++)
{
    int copy = i; // локальная копия на каждой итерации
    actions.Add(() => Console.WriteLine(copy));
}
// выведет 0 1 2
```

### Вывод

Если нужно зафиксировать значение в момент создания лямбды — скопируй его в локальную переменную.

## Пример 5: Подписчик живёт дольше источника

### Исходный код

```csharp
class Logger
{
    public Logger(Timer timer) => timer.Elapsed += OnElapsed;
    private void OnElapsed(object? s, ElapsedEventArgs e) => ...
}
```

### Что не так

- Подписчик `Logger` удерживается источником; если `Logger` больше не нужен, он не освободится (утечка памяти).

### Как лучше

```csharp
class Logger : IDisposable
{
    private readonly Timer _timer;
    public Logger(Timer timer)
    {
        _timer = timer;
        _timer.Elapsed += OnElapsed;
    }
    public void Dispose() => _timer.Elapsed -= OnElapsed;
}
```

### Вывод

Кто подписался, тот и отписывается (обычно в `Dispose`).
