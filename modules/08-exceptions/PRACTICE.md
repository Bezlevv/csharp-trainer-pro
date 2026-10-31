# Практика — Модуль 08

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Собственное исключение

**Сложность:** ★★☆
**Описание:** Создай `InvalidEmailException : Exception` и используй его при валидации email.

**Требования:**
- Проверка: содержит `@` и `.`.
- При некорректном — `throw new InvalidEmailException(email)`.

## Задача 2: Guard-валидация

**Сложность:** ★☆☆
**Описание:** Метод `Withdraw(decimal amount, decimal balance)`.

**Требования:**
- `amount <= 0` → `ArgumentOutOfRangeException`.
- `amount > balance` → `InvalidOperationException`.
- Используй `nameof`.

## Задача 3: TryParse-паттерн

**Сложность:** ★★☆
**Описание:** Напиши метод, возвращающий в `out` разобранную дату и `bool` успешности.

**Требования:**
- `bool TryParseDate(string s, out DateTime date)`.
- Внутри — блок try-catch с `FormatException`.

## Задача 4: `when`-фильтр

**Сложность:** ★★★
**Описание:** Обработай исключения пользователя и остальные- по-разному.

**Требования:**
- `MyAppException` — «пользователь ошибся» → понятное сообщение.
- Другое `Exception` — лог + переброс (`throw;`).

## Задача 5: `IDisposable` + `using`

**Сложность:** ★★☆
**Описание:** Класс `FakeConnection` с имитацией ресурса.

**Требования:**
- Реализуй `IDisposable` (пиши в консоль, что соединение закрыто).
- Используй `using`-декларацию. Покажи, что `Dispose` вызовется при исключении.

## Задача 6: Рефакторинг null-возвратов

**Сложность:** ★★☆
**Описание:** Замени возврат `null` на исключение в методе, который не должен возвращать `null`.

**Требования:**
- Например, `FindUser` в списке, обязанный найти.
- Объясни, когда `null` уместен, а когда — `throw`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
class InvalidEmailException : Exception
{
    public string Email { get; }

    public InvalidEmailException(string email)
        : base($"Некорректный email: {email}")
    {
        Email = email;
    }
}

void ValidateEmail(string email)
{
    if (!email.Contains('@') || !email.Contains('.'))
        throw new InvalidEmailException(email);
}

try
{
    ValidateEmail("not-an-email");
}
catch (InvalidEmailException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine($"Переданный email: {ex.Email}");
}
```

**Пояснение:** Своё исключение несёт дополнительную информацию (сам email) и позволяет точечно отлавливать именно этот кейс.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
void Withdraw(decimal amount, decimal balance)
{
    if (amount <= 0)
        throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");

    if (amount > balance)
        throw new InvalidOperationException("Недостаточно средств");

    Console.WriteLine($"Снято: {amount}");
}

try
{
    Withdraw(500, 100);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}
```

**Пояснение:** `nameof(amount)` автоматически подставит имя параметра в сообщение.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
bool TryParseDate(string s, out DateTime date)
{
    try
    {
        date = DateTime.Parse(s);
        return true;
    }
    catch (FormatException)
    {
        date = default;
        return false;
    }
}

if (TryParseDate("2024-01-15", out DateTime parsed))
{
    Console.WriteLine(parsed.ToShortDateString());
}
else
{
    Console.WriteLine("Неверная дата");
}
```

**Пояснение:** Это классический паттерн: `FormatException` превращается в `bool`-флаг, вызывающий код не падает.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
class MyAppException : Exception
{
    public MyAppException(string message) : base(message) { }
}

void DoWork()
{
    try
    {
        // любой код, бросающий исключения
        throw new MyAppException("Пользователь ввёл что-то не то");
    }
    catch (MyAppException ex) when (ex.Message.Contains("Пользователь"))
    {
        Console.WriteLine($"Ошибка пользователя: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
        throw; // не глотаем — перебрасываем дальше
    }
}
```

**Пояснение:** Фильтр `when` выбирает ветку по условию, не теряя информацию. `throw;` сохраняет исходный стек.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
class FakeConnection : IDisposable
{
    private bool _disposed;

    public void Open() => Console.WriteLine("Соединение открыто");

    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine("Соединение закрыто (Dispose)");
            _disposed = true;
        }
    }
}

using var connection = new FakeConnection();
connection.Open();
throw new Exception("Что-то пошло не так");
// Dispose() вызовется автоматически ДО того, как исключение уйдёт наверх
```

**Пояснение:** `using`-декларация гарантирует `Dispose` даже при исключении — ресурс не утечёт.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
class User
{
    public string Name { get; }
    public User(string name) => Name = name;
}

User FindRequiredUser(List<User> users, string name)
{
    foreach (var user in users)
    {
        if (user.Name == name) return user;
    }

    throw new KeyNotFoundException($"Пользователь не найден: {name}");
}

var users = new List<User> { new("Иван") };

try
{
    var u = FindRequiredUser(users, "Мария");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine(ex.Message);
}
```

**Пояснение:** Метод, который «обязан» найти, не должен возвращать `null` — это скрывает ошибку. А вот поиск «необязательный» (например, `TryGetValue`) честно возвращает `bool`/`null`.

</details>
