# Практика — Модуль 05

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Иерархия `Shape`

**Сложность:** ★★☆
**Описание:** Абстрактный класс `Shape` с абстрактным `Area()`. Наследники `Circle`, `Rectangle`, `Triangle`.

**Требования:**
- `Shape` — `abstract`, метод `Area()` — `abstract`.
- `Circle` — по радиусу; `Rectangle` — по сторонам; `Triangle` — по Герону.
- Создай массив `Shape[]` и вычисли сумму площадей через полиморфизм.

## Задача 2: Интерфейс `IPayable`

**Сложность:** ★★☆
**Описание:** Интерфейс для платежей, реализуют несколько типов.

**Требования:**
- `IPayable` с `decimal CalculateAmount()`.
- `Salary` (фикс. оклад) и `Contract` (ставка × часы).

**Подсказка:** `decimal` для денег.

## Задача 3: Животные

**Сложность:** ★☆☆
**Описание:** `Animal` с `virtual Speak()`, наследники `Dog`, `Cat`, `Cow` переопределяют.

**Требования:**
- Список `List<Animal>` с разными животными; выведи `Speak()` по каждому.

## Задача 4: Явная реализация `IComparable<Person>`

**Сложность:** ★★★
**Описание:** Сортировка людей по возрасту.

**Требования:**
- Класс `Person` + реализация `IComparable<Person>` (по возрасту).
- Массив людей, `Array.Sort`, вывод по возрастанию.

**Подсказка:** `CompareTo` возвращает отрицательное, ноль или положительное.

## Задача 5: Банк

**Сложность:** ★★☆
**Описание:** Интерфейс `IBankAccount` + абстрактный базовый класс `BankAccount` и `SavingsAccount` с процентами.

**Требования:**
- `IBankAccount`: `Deposit`, `Withdraw`, `Balance`.
- `BankAccount` — базовый, `SavingsAccount` добавляет `ApplyInterest(double rate)`.

## Задача 6: `sealed` vs наследование

**Сложность:** ★☆☆
**Описание:** Покажи разницу между `sealed` и обычным классом.

**Требования:**
- Создай `sealed class Point2D { }`.
- Покажи в комментарии, почему он не может иметь наследников и когда это надо.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
abstract class Shape
{
    public abstract double Area();
}

class Circle : Shape
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;
    public override double Area() => Math.PI * Radius * Radius;
}

class Rectangle : Shape
{
    public double Width { get; }
    public double Height { get; }
    public Rectangle(double w, double h) { Width = w; Height = h; }
    public override double Area() => Width * Height;
}

class Triangle : Shape
{
    private readonly double _a, _b, _c;
    public Triangle(double a, double b, double c) { _a = a; _b = b; _c = c; }
    public override double Area()
    {
        double p = (_a + _b + _c) / 2;
        return Math.Sqrt(p * (p - _a) * (p - _b) * (p - _c));
    }
}

Shape[] shapes = { new Circle(2), new Rectangle(3, 4), new Triangle(3, 4, 5) };

double total = 0;
foreach (Shape s in shapes) total += s.Area();

Console.WriteLine($"Сумма площадей: {total:F2}");
```

**Пояснение:** Полиморфизм — все фигуры складываются в `Shape[]`, и `Area()` вызывается у каждой фактической реализации.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
interface IPayable
{
    decimal CalculateAmount();
}

class Salary : IPayable
{
    public decimal Monthly { get; set; }
    public decimal CalculateAmount() => Monthly;
}

class Contract : IPayable
{
    public decimal HourlyRate { get; set; }
    public int Hours { get; set; }
    public decimal CalculateAmount() => HourlyRate * Hours;
}

var payments = new IPayable[] { new Salary { Monthly = 50000m }, new Contract { HourlyRate = 1500m, Hours = 80 } };
foreach (var p in payments) Console.WriteLine(p.CalculateAmount());
```

**Пояснение:** Интерфейс позволяет обращаться ко всем «платежам» единообразно.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
class Animal
{
    public string Name { get; set; } = "";
    public virtual string Speak() => "...";
}

class Dog : Animal
{
    public override string Speak() => "Гав!";
}

class Cat : Animal
{
    public override string Speak() => "Мяу!";
}

class Cow : Animal
{
    public override string Speak() => "Му!";
}

var animals = new List<Animal>
{
    new Dog { Name = "Рекс" },
    new Cat { Name = "Мурка" },
    new Cow { Name = "Бурёнка" }
};

foreach (var a in animals) Console.WriteLine($"{a.Name}: {a.Speak()}");
```

**Пояснение:** `virtual`/`override` позволяют каждому животному отвечать по-своему через общую ссылку.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
class Person : IComparable<Person>
{
    public string Name { get; }
    public int Age { get; }
    public Person(string name, int age) { Name = name; Age = age; }

    public int CompareTo(Person? other) =>
        other is null ? 1 : Age.CompareTo(other.Age);
}

Person[] people =
{
    new("Мария", 25),
    new("Иван", 30),
    new("Анна", 20)
};

Array.Sort(people);
foreach (var p in people) Console.WriteLine($"{p.Name}: {p.Age}");
```

**Пояснение:** `IComparable<Person>` даёт типу естественный порядок сортировки. `CompareTo` возвращает знак сравнения. (Полное сравнение по возрасту — порядок возрастания.)

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
interface IBankAccount
{
    void Deposit(decimal amount);
    void Withdraw(decimal amount);
    decimal Balance { get; }
}

abstract class BankAccount : IBankAccount
{
    protected decimal _balance;
    public decimal Balance => _balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0 || amount > _balance) throw new InvalidOperationException();
        _balance -= amount;
    }
}

class SavingsAccount : BankAccount
{
    public void ApplyInterest(decimal rate)
    {
        if (rate <= 0) throw new ArgumentOutOfRangeException(nameof(rate));
        _balance += _balance * rate;
    }
}

var acc = new SavingsAccount();
acc.Deposit(1000);
acc.ApplyInterest(0.05m);
Console.WriteLine(acc.Balance); // 1050
```

**Пояснение:** `BankAccount` реализует общий контракт, `SavingsAccount` переиспользует его и добавляет своё поведение.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
sealed class Point2D
{
    public int X { get; }
    public int Y { get; }
    public Point2D(int x, int y) { X = x; Y = y; }
}

// class Point3D : Point2D { } // Ошибка: нельзя наследоваться от sealed

class Vector
{
    public Point2D Start { get; }
    public Point2D End { get; }
    public Vector(Point2D s, Point2D e) { Start = s; End = e; }
}
```

**Пояснение:** `sealed` уместен, когда иерархия не должна развиваться (например, DTO), а расширение — через композицию (как `Vector`, содержащий точки).

</details>
