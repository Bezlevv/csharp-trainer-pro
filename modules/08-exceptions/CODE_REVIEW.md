# Разбор кода — Модуль 08

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: Пустой `catch`

### Исходный код

```csharp
try
{
    ProcessFile(path);
}
catch
{
    // ничего не делаем
}
```

### Что не так

- Ошибка проглатывается молча. Приложение работает «как ни в чём не бывало», но данные теряются. Диагностика невозможна.

### Как лучше

```csharp
try
{
    ProcessFile(path);
}
catch (IOException ex)
{
    Console.WriteLine($"Не могу прочитать файл: {ex.Message}");
    throw; // или обработать осмысленно
}
```

### Вывод

Пустой `catch` — зло. Либо обрабатывай, либо перебрасывай, либо хотя бы логируй.

## Пример 2: `throw ex;`

### Исходный код

```csharp
catch (Exception ex)
{
    Log(ex);
    throw ex; // ПЛОХО
}
```

### Что не так

- `throw ex;` **сбрасывает** стек вызовов: настоящая точка отказа скрывается, отладка превращается в гадание.

### Как лучше

```csharp
catch (Exception ex)
{
    Log(ex);
    throw; // сохраняет исходный стек
}
```

### Вывод

Для переброса используй голый `throw;`, не `throw ex;`.

## Пример 3: Один `catch (Exception)` на всё

### Исходный код

```csharp
try
{
    var result = ProcessOrder(order);
    Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine("Что-то пошло не так: " + ex.Message);
}
```

### Что не так

- Ловится всё подряд: и ожидаемая ошибка бизнес-логики, и `NullReferenceException` (баг), и `OutOfMemoryException`. Все обрабатываются одинаково.

### Как лучше

```csharp
try
{
    var result = ProcessOrder(order);
    Console.WriteLine(result);
}
catch (OrderValidationException ex)
{
    Console.WriteLine($"Некорректный заказ: {ex.Message}");
}
catch (NullReferenceException)
{
    Console.WriteLine("Внутренний баг: обращение к null");
}
```

### Вывод

Лови конкретные типы — и только то, что можешь осмысленно обработать.

## Пример 4: `new Exception("...")`

### Исходный код

```csharp
if (account.Balance < amount)
    throw new Exception("Недостаточно средств");
```

### Что не так

- `Exception` слишком общий: невозможно отловить именно эту ситуацию, не поймав остальное. Сообщение — единственное отличие.

### Как лучше

```csharp
if (account.Balance < amount)
    throw new InvalidOperationException("Недостаточно средств");
```

### Вывод

Используй семантически точный тип исключения. Если стандартных не хватает — создай свой.

## Пример 5: Ресурсы без `using`

### Исходный код

```csharp
var reader = new StreamReader("data.txt");
var line = reader.ReadLine();
Console.WriteLine(line);
// reader.Dispose() так и не вызовется, если будет исключение
```

### Что не так

- Если `ReadLine` бросит исключение, файл останется открытым (утечка хэндла). При повторных запусках — «файл занят другим процессом».

### Как лучше

```csharp
using var reader = new StreamReader("data.txt");
var line = reader.ReadLine();
Console.WriteLine(line);
```

### Вывод

Все `IDisposable` — под `using` (или в `finally` с `Dispose`).

## Пример 6: Исключения как поток управления

### Исходный код

```csharp
try
{
    if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException();
    ...
}
catch (ArgumentException)
{
    // «поймали» ожидаемую ситуацию, чтобы что-то сделать
}
```

### Что не так

- Исключения используются вместо обычных условий. Это медленно, нечитаемо и запутывает.

### Как лучше

```csharp
if (string.IsNullOrWhiteSpace(name))
{
    Console.WriteLine("Имя не задано");
    return;
}
```

### Вывод

Исключения — для исключительных ситуаций, а не для рутины.
