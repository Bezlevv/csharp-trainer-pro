# Разбор кода — Модуль 04

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: Публичные поля вместо свойств

### Исходный код

```csharp
class BankAccount
{
    public decimal Balance;
}
```

И тут начинается хаос:

```csharp
var acc = new BankAccount { Balance = -500 }; // отрицательный баланс!
acc.Balance = 999999;                          // кто угодно меняет
```

### Что не так

- Инкапсуляция отсутствует: поле можно менять напрямую, валидацию обойти.

### Как лучше

```csharp
class BankAccount
{
    private decimal _balance;
    public decimal Balance => _balance;
    // Deposit / Withdraw с проверками
}
```

### Вывод

Внутреннее состояние — `private`. Наружу — только контролируемые методы и свойства.

## Пример 2: «Проваливание» инкапсуляции

### Исходный код

```csharp
class Cart
{
    public List<string> Items { get; } = new();
}

var cart = new Cart();
cart.Items.Add("Pizza"); // модификация извне без контроля
cart.Items.Clear();      // всё стёрто
```

### Что не так

- Публичное `List<T>` отдаёт доступ на изменение внутренней коллекции. «Защита» ничего не защищает.

### Как лучше

```csharp
class Cart
{
    private readonly List<string> _items = new();
    public IReadOnlyList<string> Items => _items;

    public void Add(string item)
    {
        if (string.IsNullOrWhiteSpace(item)) return;
        _items.Add(item);
    }
}
```

### Вывод

Отдавай наружу `IReadOnlyList<T>` (подробнее — модуль 07). Мутация — только через методы.

## Пример 3: God-класс `Program`

### Исходный код

```csharp
// Program.cs — 500 строк: и меню, и расчёты, и сохранение
var action = Console.ReadLine();
// ... если action == "1" то считаем налог, если "2" сохраняем, ...
```

### Что не так

- Один класс делает всё: ввод, бизнес-логику, хранение. Тестировать и поддерживать невозможно.

### Как лучше

```csharp
// Меню — в ConsoleUI
// Расчёты — в TaxCalculator
// Сохранение — в FileStorage
// Program.cs только связывает их
```

### Вывод

Каждый класс — одна ответственность (подробнее — модуль 14, SRP).

## Пример 4: Общий изменяемый `static`

### Исходный код

```csharp
static class Global
{
    public static int Counter = 0;
}

// в 10 местах: Global.Counter++;
// в 3 местах: Global.Counter = 0;
```

### Что не так

- Состояние общее для всей программы; его меняют непредсказуемо. Трудно отлаживать.

### Как лучше

```csharp
class Counter
{
    private int _value;
    public void Increment() => _value++;
    public void Reset() => _value = 0;
    public int Value => _value;
}
```

### Вывод

Изменяемое состояние — в конкретных объектах, а не в глобальных статических полях.

## Пример 5: Дублирование в конструкторах

### Исходный код

```csharp
class Rectangle
{
    public int Width { get; }
    public int Height { get; }

    public Rectangle(int width, int height)
    {
        if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
        Width = width;
        Height = height;
    }

    public Rectangle() // копия логики валидации
    {
        Width = 1;
        Height = 1;
    }
}
```

### Что не так

- Валидация продублирована, значения по умолчанию зашиты в двух местах.

### Как лучше

```csharp
public Rectangle(int width = 1, int height = 1)
{
    if (width <= 0 || height <= 0) throw new ArgumentOutOfRangeException();
    Width = width;
    Height = height;
}
```

### Вывод

Необязательные параметры с дефолтами вытесняют вспомогательные конструкторы.
