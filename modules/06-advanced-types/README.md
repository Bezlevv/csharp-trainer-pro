# Модуль 06: Продвинутые типы данных

> **Цель:** выбирать подходящий тип (struct/enum/record/tuple), использовать паттерн-матчинг и безопасно работать с nullable.

## Зачем это нужно

К этому моменту ты знаешь классы и интерфейсы. Но C# даёт и другие типы, каждый — под свою задачу: `enum` вместо «магических чисел», `record` для неизменяемых данных, `struct` для мелких value-сущностей, туплы для быстрых многосоставных значений. Плюс паттерн-матчинг — современный способ красиво разбирать данные. Это уровень «сильного среднего» разработчика.

## Теория

### 1. `enum` — перечисления

```csharp
enum OrderStatus
{
    New,
    Paid,
    Shipped,
    Delivered
}
```

Использование:

```csharp
OrderStatus status = OrderStatus.Paid;
switch (status)
{
    case OrderStatus.New: Console.WriteLine("Новый"); break;
    case OrderStatus.Paid: Console.WriteLine("Оплачен"); break;
    default: Console.WriteLine("Другое"); break;
}
```

- По умолчанию базовый тип — `int` (можно сменить).
- `[Flags]` позволяет комбинировать значения (побитово).
- `Enum.TryParse("Paid", out OrderStatus s)` — безопасный разбор строки.

**Зачем:** вместо «магических чисел» — понятные имена, автодополнение, проверки на этапе компиляции.

### 2. `struct` — тип значения

```csharp
struct Point
{
    public int X { get; init; }
    public int Y { get; init; }
}
```

- Значение хранится в переменной напрямую; копируется при присваивании.
- Не может наследоваться (кроме `interface`).
- Идеален для **мелких неизменяемых** сущностей: координаты, RGB-цвет, дата.
- Большие и мутабельные — лучше класс.

**`readonly struct`** — запрет мутации:

```csharp
readonly struct Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
}
```

### 3. `record` — value-равенство

`record` — класс с автоматическим `Equals`/`GetHashCode`/`ToString` по значениям.

```csharp
record Person(string Name, int Age);
```

```csharp
var a = new Person("Иван", 30);
var b = new Person("Иван", 30);
Console.WriteLine(a == b);       // True — по значениям
Console.WriteLine(a with { Age = 31 }); // копия с изменением
```

**`record struct`** — record на базе struct:

```csharp
record struct Point(int X, int Y);
```

**Для чего:** DTO, конфигурации, данные, которые сравнивают по содержимому.

### 4. Туплы

```csharp
var t = (X: 10, Y: 20);
Console.WriteLine(t.X); // 10
```

```csharp
(int Min, int Max) FindRange(int[] numbers) => (numbers.Min(), numbers.Max());
var (min, max) = FindRange(new[] { 3, 1, 4 });
```

- Удобны для возврата нескольких значений без отдельного класса.

### 5. Nullable value types

```csharp
int? maybe = null; // System.Nullable<int>
```

```csharp
int? a = null;
int b = a ?? 42;   // b == 42
Console.WriteLine(a.HasValue); // false
```

Операции с nullable ограничены — проверяй через `??`, `?.` или `hasValue`.

### 6. Паттерн-матчинг (C# 9–12)

**Type pattern:**

```csharp
if (obj is string s)
{
    Console.WriteLine(s.Length);
}
```

**Property pattern:**

```csharp
bool IsAdult(Person p) => p is { Age: >= 18 };
```

**Relational + logical:**

```csharp
string Classify(int n) => n switch
{
    < 0 => "отрицательное",
    > 0 => "положительное",
    _ => "ноль"
};
```

**Combined (and/or/not):**

```csharp
bool IsWorkTime(DayOfWeek day) => day is
    DayOfWeek.Monday or DayOfWeek.Tuesday or DayOfWeek.Wednesday or
    DayOfWeek.Thursday or DayOfWeek.Friday;
```

**Discards (`_`):**

```csharp
_ => "default"
```

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `class` для мелких value-сущностей | боксинг/аллокации, нет value-семантики | `struct` или `record struct` |
| `struct` для большой мутабельной сущности | копирование дорого, состояние «плывёт» | класс |
| Магические числа вместо enum | непонятно, легко перепутать | enum |
| Мутабельный record (`set` вместо `init`) | теряется смысл «неизменяемого снимка» | `init` или позиционный record |
| `dict.GetValueOrDefault` без обработки отсутствия | может вернуть `null`/дефолт | проверяй `TryGetValue` |

## Контрольные вопросы

1. Когда выбрать `struct`, а когда `class`?
2. Что автоматически генерирует `record`?
3. Что такое `record struct`?
4. Как создать тупл с именованными элементами?
5. Что такое nullable-тип и как безопасно работать с ним?
6. Какие бывают виды паттернов?
7. Что делает дискард `_`?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
