# Разбор кода — Модуль 06

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: `class` вместо `struct` для мелкой value-сущности

### Исходный код

```csharp
class Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
```

### Что не так

- `Point` — мелкая value-сущность. Как `class`, она ссылочная, копирования по значению нет, добавляет аллокации.

### Как лучше

```csharp
readonly struct Point
{
    public int X { get; }
    public int Y { get; }
    public Point(int x, int y) { X = x; Y = y; }
}
```

### Вывод

Мелкие неизменяемые сущности — `readonly struct` (или `record struct`).

## Пример 2: Магические числа вместо enum

### Исходный код

```csharp
if (orderStatus == 2) { /* оплачен */ }
if (orderStatus == 5) { /* доставлен */ }
```

### Что не так

- `2` и `5` ничего не говорят. Перепутать легко, поддерживать больно.

### Как лучше

```csharp
enum OrderStatus { New, Paid, Shipped, Delivered }
if (orderStatus == OrderStatus.Paid) { }
```

### Вывод

Enum — типобезопасная замена магических чисел.

## Пример 3: Строка-«статус» вместо enum

### Исходный код

```csharp
string status = "done";
if (status == "done" || status == "DONE" || status == "Done") { }
```

### Что не так

- Строки непостоянны, сравнение с учётом регистра — источник багов. Нет автодополнения.

### Как лучше

```csharp
enum OrderStatus { None, Done }
if (status == OrderStatus.Done) { }
```

### Вывод

Enum исключает опечатки и регистр.

## Пример 4: Мутабельный record

### Исходный код

```csharp
record User
{
    public string Name { get; set; } // set — запись можно менять
}
```

### Что не так

- Смысл `record` — неизменяемый снимок значения. `set` позволяет менять поля извне, ломая value-равенство и `with`-семантику.

### Как лучше

```csharp
record User(string Name);
// или с init:
record User { public string Name { get; init; } }
```

### Вывод

`record` задуман как неизменяемый; используй `init`.

## Пример 5: `Dictionary.GetValueOrDefault` без обработки отсутствия

### Исходный код

```csharp
var dict = new Dictionary<string, int> { ["a"] = 1 };
int value = dict.GetValueOrDefault("missing"); // 0, но 0 — легитимное значение
if (value == 0) Console.WriteLine("Не найдено"); // ложная тревога
```

### Что не так

- Нельзя отличить «нет ключа» от «значение 0».

### Как лучше

```csharp
if (dict.TryGetValue("missing", out int actual))
{
    Console.WriteLine(actual);
}
else
{
    Console.WriteLine("Не найдено");
}
```

### Вывод

`TryGetValue` различает отсутствие ключа и реальное значение.
