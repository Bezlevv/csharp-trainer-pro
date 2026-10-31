# Практика — Модуль 04

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: `BankAccount`

**Сложность:** ★☆☆
**Описание:** Класс банковского счёта с защищённым балансом.

**Требования:**
- Поле баланс — `private`.
- `Deposit(decimal amount)` — пополнение; отрицательное/нулевое — `ArgumentOutOfRangeException`.
- `Withdraw(decimal amount)` — снятие; нельзя уйти в минус.
- Свойство `Balance` — только чтение.

**Подсказка:** `private set` в свойстве или просто `get`-свойство с приватным полем.

## Задача 2: `Point`

**Сложность:** ★☆☆
**Описание:** Класс точки на плоскости с методом `DistanceTo`.

**Требования:**
- `X`, `Y` — свойства только для чтения (`init` или `private set`).
- `double DistanceTo(Point other)` — евклидово расстояние.

**Подсказка:** `Math.Sqrt((X-other.X)^2 + (Y-other.Y)^2)`.

## Задача 3: `Student`

**Сложность:** ★★☆
**Описание:** Класс студента с валидацией в свойствах.

**Требования:**
- `Name` — непустая строка (иначе `ArgumentException`).
- `Age` — от 14 до 100 (иначе `ArgumentOutOfRangeException`).
- `Gpa` (средний балл) — от 0 до 5.

## Задача 4: `record` + `with`

**Сложность:** ★☆☆
**Описание:** Используй `record Book(string Title, string Author, int Year)`.

**Требования:**
- Создай экземпляр, склонируй через `with`, изменив год.
- Покажи, что `==` сравнивает по значениям.

## Задача 5: Статический класс `TemperatureConverter`

**Сложность:** ★☆☆
**Описание:** Статический класс с двумя методами для температур.

**Требования:**
- `CelsiusToFahrenheit(double)` и `FahrenheitToCelsius(double)`.
- Вызови из `Program` без создания объекта.

## Задача 6: `Countdown`-таймер

**Сложность:** ★★★
**Описание:** Класс обратного отсчёта с инкапсуляцией.

**Требования:**
- `Remaining` — свойство (чтение), приватный сеттер.
- `Tick()` — уменьшает на 1; при 0 бросает `InvalidOperationException`.
- `Reset(int seconds)` — устанавливает новое значение (только положительное).

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
class BankAccount
{
    private decimal _balance;

    public decimal Balance => _balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");

        _balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "Сумма должна быть положительной");

        if (amount > _balance)
            throw new InvalidOperationException("Недостаточно средств");

        _balance -= amount;
    }
}

var acc = new BankAccount();
acc.Deposit(1000);
acc.Withdraw(350);
Console.WriteLine(acc.Balance); // 650
```

**Пояснение:** Баланс скрыт, менять его можно только через методы с проверками. `nameof(amount)` даёт имя параметра в сообщении — удобно и переименование не забудется.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
class Point
{
    public int X { get; init; }
    public int Y { get; init; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public double DistanceTo(Point other) =>
        Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
}

var a = new Point(0, 0);
var b = new Point(3, 4);
Console.WriteLine(a.DistanceTo(b)); // 5
```

**Пояснение:** `init`-свойства можно задать только при создании — точка по сути неизменяема (immutable).

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
class Student
{
    private string _name;
    private int _age;
    private double _gpa;

    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Имя не может быть пустым", nameof(Name));
            _name = value;
        }
    }

    public int Age
    {
        get => _age;
        set
        {
            if (value is < 14 or > 100)
                throw new ArgumentOutOfRangeException(nameof(Age), "Возраст от 14 до 100");
            _age = value;
        }
    }

    public double Gpa
    {
        get => _gpa;
        set
        {
            if (value is < 0 or > 5)
                throw new ArgumentOutOfRangeException(nameof(Gpa), "GPA от 0 до 5");
            _gpa = value;
        }
    }
}
```

**Пояснение:** Валидация живёт в сеттерах: состояние не может стать некорректным. Паттерн `value is < 14 or > 100` компактно выражает проверку диапазона.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
record Book(string Title, string Author, int Year);

var original = new Book("Война и мир", "Л. Толстой", 1869);
var updated = original with { Year = 2025 };

Console.WriteLine(original == updated); // False — год разный
Console.WriteLine(original.Title);       // Война и мир
```

**Пояснение:** `with` создаёт копию с изменёнными свойствами. `==` у record сравнивает по значениям, поэтому две записи с одинаковым содержимым «равны».

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
static class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double c) => c * 9 / 5 + 32;
    public static double FahrenheitToCelsius(double f) => (f - 32) * 5 / 9;
}

Console.WriteLine(TemperatureConverter.CelsiusToFahrenheit(0));   // 32
Console.WriteLine(TemperatureConverter.FahrenheitToCelsius(212)); // 100
```

**Пояснение:** Статический класс не создаётся — методы вызываются через имя класса.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
class Countdown
{
    private int _remaining;

    public int Remaining => _remaining;

    public void Reset(int seconds)
    {
        if (seconds <= 0)
            throw new ArgumentOutOfRangeException(nameof(seconds), "Должно быть положительным");

        _remaining = seconds;
    }

    public void Tick()
    {
        if (_remaining == 0)
            throw new InvalidOperationException("Таймер уже закончился");

        _remaining--;
    }
}

var cd = new Countdown();
cd.Reset(3);
cd.Tick();
Console.WriteLine(cd.Remaining); // 2
```

**Пояснение:** Снаружи виден только `Remaining` для чтения. Внутреннее состояние меняется только через `Tick`/`Reset`.

</details>
