# Модуль 08: Исключения и обработка ошибок

> **Цель:** грамотно бросать, ловить и обрабатывать ошибки; проектировать собственные исключения.

## Зачем это нужно

Программа работает с ненадёжным внешним миром: пользователь вводит мусор, файл отсутствует, сеть недоступна. Исключения — главный механизм сообщения об ошибках в C#. Умение писать код, который не падает молча и **диагностирует** проблему, — необходимый навык.

## Теория

### 1. Зачем исключения

Можно вернуть «код ошибки» (например, `-1`). Но тогда каждый вызов нужно проверять. Исключения — единый механизм: метод бросает `throw`, а вызывающий может перехватить `catch`.

Исключение — это объект, наследуемый от `Exception`.

### 2. `try` / `catch` / `finally`

```csharp
try
{
    int x = int.Parse(input);
    Console.WriteLine(x);
}
catch (FormatException)
{
    Console.WriteLine("Это не число");
}
catch (Exception ex)
{
    Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
}
finally
{
    Console.WriteLine("Выполнится в любом случае");
}
```

### 3. Стандартные исключения

| Исключение | Когда |
|---|---|
| `ArgumentException` | неверный аргумент |
| `ArgumentNullException` | `null` на входе |
| `ArgumentOutOfRangeException` | значение вне диапазона |
| `InvalidOperationException` | операция недопустима в текущем состоянии |
| `FormatException` | неверный формат строки |
| `DivideByZeroException` | деление на ноль |
| `NullReferenceException` | разыменование `null` |
| `NotImplementedException` | метод не реализован |

Валидация аргументов — guard clauses:

```csharp
void SetAge(int age)
{
    if (age <= 0) throw new ArgumentOutOfRangeException(nameof(age), "Возраст должен быть положительным");
    ...
}
```

### 4. `throw` vs `rethrow`

Никогда не теряй стек вызовов — бросай «как есть»:

```csharp
catch (Exception ex)
{
    Log(ex);
    throw; // сохраняет стек — правильно
    // throw ex; // ПЛОХО: обнуляет стек
}
```

### 5. `throw`-выражения

В тернарном операторе и инициализаторах:

```csharp
static string EnsureNotEmpty(string? value) =>
    string.IsNullOrWhiteSpace(value)
        ? throw new ArgumentException("Пустое значение")
        : value;
```

### 6. Фильтры `when`

```csharp
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
{
    // перелогиниться
}
catch (HttpRequestException)
{
    // другая ошибка сети
}
```

### 7. Собственные исключения

```csharp
class InvalidEmailException : Exception
{
    public InvalidEmailException(string email)
        : base($"Некорректный email: {email}")
    {
        Email = email;
    }

    public string Email { get; }
}
```

```csharp
throw new InvalidEmailException("not-an-email");
```

- Имя заканчивается на `Exception`.
- Наследуемся от `Exception` (или от конкретного типа).
- Сообщения — понятные.

### 8. `using` и `IDisposable`

`IDisposable` — интерфейс с `Dispose()`. Типы, владеющие ресурсами (файлы, соединения), его реализуют. `using` гарантирует освобождение даже при исключении.

```csharp
using (var file = File.CreateText("log.txt"))
{
    file.WriteLine("запись");
} // Dispose() вызывается автоматически
```

**Using declaration** (C# 8) — проще:

```csharp
using var file = File.CreateText("log.txt");
file.WriteLine("запись");
```

Освобождение происходит в конце области видимости.

### 9. Чего не делать

- Пустой `catch { }` — проглатывание ошибок.
- Использовать исключения для управления потоком (как `goto`).
- Ловить `catch (Exception)` без нужды.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Пустой `catch { }` | молча теряется причина | логировать и/или перебросить |
| `throw ex;` | теряется стек вызовов | `throw;` |
| `catch (Exception)` везде | ловит и «корявые» баги | конкретные типы |
| `new Exception("...")` | непонятный тип | свой класс или стандартный тип |
| Ресурсы без `using` | утечка | `using` |
| Исключения как поток управления | нечитаемо | нормальный `if`/возврат |

## Контрольные вопросы

1. Зачем исключения, а не коды ошибок?
2. Что делает `finally`?
3. Когда `throw;` отличает от `throw ex;`?
4. Что даёт `catch (Exception ex) when`?
5. Как создать своё исключение?
6. Что реализует `IDisposable` и зачем `using`?
7. Почему пустой `catch` вреден?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
