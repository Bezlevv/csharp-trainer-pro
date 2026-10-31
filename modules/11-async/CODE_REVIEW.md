# Разбор кода — Модуль 11

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: `.Result` внутри async-метода

### Исходный код

```csharp
async Task<string> GetAsync()
{
    var client = new HttpClient();
    return client.GetStringAsync("https://example.com").Result; // ПЛОХО
}
```

### Что не так

- Блокирует поток и рискует получить **deadlock** в UI/ASP.NET, т.к. контекст синхронизации удерживается.

### Как лучше

```csharp
async Task<string> GetAsync()
{
    using var client = new HttpClient();
    return await client.GetStringAsync("https://example.com");
}
```

### Вывод

Внутри async-кода — только `await`.

## Пример 2: `async void`

### Исходный код

```csharp
async void SaveAsync() // только для обработчиков событий!
{
    await Task.Delay(100);
}
```

### Что не так

- `async void` нельзя дождаться, а исключения теряются — их никто не поймает.

### Как лучше

```csharp
async Task SaveAsync()
{
    await Task.Delay(100);
}
```

### Вывод

Используй `async Task`. `async void` — лишь для event-обработчиков.

## Пример 3: Fire-and-forget

### Исходный код

```csharp
Task fire = SomeAsync();
// не дожидаемся: исключения из SomeAsync потеряются
```

### Что не так

- Если `SomeAsync` бросит исключение, оно окажется «в никуда». Программа продолжит работать с потенциально неконсистентным состоянием.

### Как лучше

```csharp
try
{
    await SomeAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
```

### Вывод

Любую задачу либо дожидайся через `await`, либо осознанно обрабатывай ошибки.

## Пример 4: `async`-метод без `await`

### Исходный код

```csharp
async Task<int> ComputeAsync() // warning CS1998
{
    return 42;
}
```

### Что не так

- Метод async, но ничего асинхронного нет. Компилятор предупредит (CS1998).

### Как лучше

```csharp
Task<int> ComputeAsync() => Task.FromResult(42);
// или просто: int Compute() => 42;
```

### Вывод

Не добавляй `async`, если нет `await`.

## Пример 5: Блокировка UI в `foreach`

### Исходный код

```csharp
foreach (var url in urls)
{
    var html = client.GetStringAsync(url).Result; // блокирует интерфейс
}
```

### Что не так

- Каждая итерация блокирует поток, UI «замерзает».

### Как лучше

```csharp
var tasks = urls.Select(u => client.GetStringAsync(u));
var results = await Task.WhenAll(tasks);
```

### Вывод

Для набора операций — `Task.WhenAll`, не последовательные `.Result`.
