# Практика — Модуль 06

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: enum-меню

**Сложность:** ★☆☆
**Описание:** Создай `enum MenuAction { Add, List, Remove, Exit }` и прочитай действие с клавиатуры.

**Требования:**
- `Enum.TryParse` для разбора.
- `switch`-выражение для вывода сообщения.

## Задача 2: struct Money

**Сложность:** ★★☆
**Описание:** `readonly struct Money` с валютами и методом сложения.

**Требования:**
- Поля: `decimal Amount`, `string Currency`.
- `Money Add(Money other)` — складывает только если валюта совпадает (иначе `InvalidOperationException`).
- Покажи, что присваивание создаёт копию.

## Задача 3: record с `with`

**Сложность:** ★☆☆
**Описание:** `record Person(string Name, int Age)`.

**Требования:**
- Создай два равных экземпляра, убедись, что `==` вернёт `true`.
- Через `with` создай копию с другим возрастом.

## Задача 4: Деструктуризация туплов

**Сложность:** ★★☆
**Описание:** Метод возвращает минимум, максимум и среднее массива.

**Требования:**
- Верни `(int Min, int Max, double Avg)`.
- Разбери результат через `var (min, max, avg) = ...`.

**Подсказка:** Пустой массив — брось `ArgumentException`.

## Задача 5: Калькулятор на паттернах

**Сложность:** ★★☆
**Описание:** Калькулятор с операторами `+ - * / %` через `switch`-выражение с реляционными паттернами для проверки деления на ноль.

**Требования:**
- Обработай деление на ноль — верни `double.NaN` или брось исключение.

## Задача 6: Обработка nullable

**Сложность:** ★★★
**Описание:** Напиши метод, который ищет максимальное значение в массиве `int?` и безопасно возвращает `int?`.

**Требования:**
- `int? FindMax(int?[] numbers)`.
- Учитывай `null`-элементы: они игнорируются.
- Если все элементы `null` — верни `null`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
enum MenuAction { Add, List, Remove, Exit }

Console.Write("Действие (Add/List/Remove/Exit): ");
string input = Console.ReadLine() ?? "";

if (Enum.TryParse(input, ignoreCase: true, out MenuAction action))
{
    string message = action switch
    {
        MenuAction.Add => "Добавление...",
        MenuAction.List => "Список...",
        MenuAction.Remove => "Удаление...",
        MenuAction.Exit => "Выход",
        _ => "Неизвестно"
    };
    Console.WriteLine(message);
}
else
{
    Console.WriteLine("Неизвестное действие");
}
```

**Пояснение:** `Enum.TryParse` с `ignoreCase: true` разбирает строки без учёта регистра.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
readonly struct Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Разные валюты");

        return new Money(Amount + other.Amount, Currency);
    }
}

var a = new Money(100, "RUB");
var b = new Money(50, "RUB");
Console.WriteLine(a.Add(b).Amount); // 150

var copy = a; // копия (value type)
copy = new Money(0, "RUB");
Console.WriteLine(a.Amount); // 100 — оригинал не изменился
```

**Пояснение:** `readonly struct` — неизменяемая, копируется по значению, поэтому `a` не пострадал от изменений `copy`.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
record Person(string Name, int Age);

var p1 = new Person("Иван", 30);
var p2 = new Person("Иван", 30);
Console.WriteLine(p1 == p2); // True

var older = p1 with { Age = 31 };
Console.WriteLine(older); // Person { Name = Иван, Age = 31 }
```

**Пояснение:** `record` сравнивает поля по значениям. `with` создаёт новую запись с изменённым полем.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
(int Min, int Max, double Avg) FindRange(int[] numbers)
{
    if (numbers.Length == 0)
        throw new ArgumentException("Массив пуст", nameof(numbers));

    int min = numbers[0], max = numbers[0];
    double sum = 0;

    foreach (int n in numbers)
    {
        if (n < min) min = n;
        if (n > max) max = n;
        sum += n;
    }

    return (min, max, sum / numbers.Length);
}

var (min, max, avg) = FindRange(new[] { 3, 1, 4, 1, 5, 9, 2, 6 });
Console.WriteLine($"{min} {max} {avg:F2}"); // 1 9 3.88
```

**Пояснение:** Кортеж даёт понятные имена элементам результата.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
Console.Write("Первое число: ");
double.TryParse(Console.ReadLine(), out double a);

Console.Write("Операция: ");
string op = Console.ReadLine() ?? "";

Console.Write("Второе число: ");
double.TryParse(Console.ReadLine(), out double b);

double result = (op, b) switch
{
    ("+", _) => a + b,
    ("-", _) => a - b,
    ("*", _) => a * b,
    ("/", 0) => double.NaN,      // деление на ноль
    ("/", _) => a / b,
    ("%", 0) => double.NaN,
    ("%", _) => a % b,
    _ => double.NaN              // неизвестная операция
};

Console.WriteLine(result == double.NaN ? "Ошибка" : $"Результат: {result}");
```

**Пояснение:** Паттерн на кортеж + реляционный паттерн `0` ловит деление на ноль до общего случая.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
int? FindMax(int?[] numbers)
{
    int? max = null;

    foreach (int? n in numbers)
    {
        if (n.HasValue && (!max.HasValue || n > max))
        {
            max = n;
        }
    }

    return max;
}

int?[] data = { null, 5, null, 9, 2 };
Console.WriteLine(FindMax(data));        // 9
Console.WriteLine(FindMax(new int?[] { null, null })); // null
```

**Пояснение:** Накапливаем максимум только среди значимых элементов; пустой или полностью `null`-массив даёт `null`.

</details>
