# Разбор кода — Модуль 10

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: `Select` в цикле вместо `SelectMany`

### Исходный код

```csharp
var allNames = new List<string>();
foreach (var team in teams)
{
    allNames.AddRange(team.Players.Select(p => p.Name));
}
```

### Что не так

- Логика «сплющивания» вложенных коллекций размазана. `SelectMany` делает это выразительнее.

### Как лучше

```csharp
var allNames = teams.SelectMany(t => t.Players).Select(p => p.Name).ToList();
```

### Вывод

`SelectMany` — для превращения коллекции коллекций в плоский список.

## Пример 2: Повторный перебор «ленивого» IEnumerable

### Исходный код

```csharp
var filtered = data.Where(x => ExpensiveCheck(x));
if (filtered.Any()) { ... }   // 1-й проход
var count = filtered.Count();   // 2-й проход — ExpensiveCheck снова
```

### Что не так

- `filtered` — ленивый; каждый перебор пересчитывает `ExpensiveCheck`. Двойная работа.

### Как лучше

```csharp
var list = data.Where(x => ExpensiveCheck(x)).ToList();
if (list.Any()) { ... }
var count = list.Count;
```

### Вывод

Материализуй результат в `ToList()`/`ToArray()`, если перебираешь несколько раз.

## Пример 3: `FirstOrDefault` для value-типов

### Исходный код

```csharp
int found = numbers.FirstOrDefault(n => n > 100);
if (found != 0) Console.WriteLine(found); // неверно: 0 — легитимное значение
```

### Что не так

- `FirstOrDefault` для `int` вернёт `0`, если совпадений нет. Отличить «нет элемента» от «элемент равен 0» нельзя.

### Как лучше

```csharp
int? found = numbers.Cast<int?>().FirstOrDefault(n => n > 100);
if (found.HasValue) Console.WriteLine(found.Value);
```

или просто проверять `Any` перед `First`:

```csharp
if (numbers.Any(n => n > 100))
{
    int value = numbers.First(n => n > 100);
}
```

### Вывод

Для value-типов аккуратно используй `FirstOrDefault` — лучше явная проверка `Any`.

## Пример 4: Сортировка в цикле

### Исходный код

```csharp
foreach (var u in users)
{
    var sorted = users.OrderBy(x => x.Age); // сортировка на каждой итерации!
}
```

### Что не так

- `OrderBy` перевызывается в цикле — дорого и бессмысленно.

### Как лучше

```csharp
var sorted = users.OrderBy(u => u.Age).ToList();
foreach (var u in sorted) { ... }
```

### Вывод

Сортировка один раз, вне цикла.

## Пример 5: `string.Concat` в цикле

### Исходный код

```csharp
string result = "";
foreach (var s in items)
{
    result += s; // копирование строки на каждой итерации
}
```

### Что не так

- О(n²): каждая конкатенация копирует всю накопленную строку.

### Как лучше

```csharp
string result = string.Join("", items);
// для сложного форматирования — StringBuilder
```

### Вывод

`string.Join` решает почти все случаи. В сложных случаях — `StringBuilder`.
