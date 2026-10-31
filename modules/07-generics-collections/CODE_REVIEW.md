# Разбор кода — Модуль 07

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: `List.Contains` в цикле — O(n²)

### Исходный код

```csharp
var listA = ...; // 10 000 элементов
var listB = ...; // 10 000 элементов
var duplicates = new List<int>();

foreach (var x in listA)
{
    if (listB.Contains(x)) // O(n) на каждую проверку
    {
        duplicates.Add(x);
    }
}
```

### Что не так

- `List.Contains` — O(n); в цикле по 10 000 элементов это ~100 млн операций.

### Как лучше

```csharp
var setB = listB.ToHashSet();
var duplicates = listA.Where(setB.Contains).ToList(); // O(n)
```

### Вывод

Для проверок существования используй `HashSet`/`Dictionary` — O(1) вместо O(n).

## Пример 2: Публичный `List<T>` в API

### Исходный код

```csharp
class Cart
{
    public List<string> Items { get; } = new();
}

var cart = new Cart();
cart.Items.Clear(); // кто угодно стирает
```

### Что не так

- `List` раскрывает внутреннее состояние на изменение. Инкапсуляция нарушена.

### Как лучше

```csharp
class Cart
{
    private readonly List<string> _items = new();
    public IReadOnlyList<string> Items => _items;
}
```

### Вывод

Отдавай наружу `IReadOnlyList<T>` — потребители читают, но не меняют.

## Пример 3: Изменение коллекции в `foreach`

### Исходный код

```csharp
var names = new List<string> { "Иван", "Мария", "Пётр" };

foreach (var name in names)
{
    if (name.StartsWith("М")) names.Remove(name); // InvalidOperationException!
}
```

### Что не так

- Модификация коллекции во время перебора бросает `InvalidOperationException`.

### Как лучше

```csharp
var toRemove = names.Where(n => n.StartsWith("М")).ToList();
foreach (var name in toRemove) names.Remove(name);
```

### Вывод

Сначала собери список того, что менять, затем применяй изменения.

## Пример 4: Перебор ключей ради поиска

### Исходный код

```csharp
if (dict.Keys.Contains(key))
{
    return dict[key];
}
```

### Что не так

- Двойной проход (поиск ключа, затем доступ) и лишний разбор.

### Как лучше

```csharp
if (dict.TryGetValue(key, out var value))
{
    return value;
}
```

### Вывод

`TryGetValue` — один проход, один разбор, O(1).

## Пример 5: Необобщённые коллекции с боксингом

### Исходный код

```csharp
var items = new System.Collections.ArrayList();
items.Add(1);
items.Add(2);
int sum = 0;
foreach (int item in items) sum += item;
```

### Что не так

- `ArrayList` хранит `object`, вызывает боксинг/анбоксинг и требует приведение типов.

### Как лучше

```csharp
var items = new List<int> { 1, 2 };
int sum = items.Sum();
```

### Вывод

Всегда обобщённые коллекции: безопасность и производительность.
