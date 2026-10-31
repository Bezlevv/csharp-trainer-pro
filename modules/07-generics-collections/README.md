# Модуль 07: Обобщения и коллекции

> **Цель:** уверенно выбирать и использовать коллекции, понимать обобщения и их ограничения.

## Зачем это нужно

Программы постоянно работают с наборами данных: списки товаров, словари настроек, очереди задач, уникальные множества. В C# коллекции параметризованы через обобщения. Понимание их внутреннего устройства и Big O — то, что отличает «набирающего» от среднего разработчика: это влияет на скорость твоих программ при больших объёмах данных.

## Теория

### 1. Зачем обобщения

Сначала — проблема. «Старые» необобщённые коллекции хранили `object`:

```csharp
// Так было раньше (и так не надо):
var list = new System.Collections.ArrayList();
list.Add(1);
list.Add("текст");   // положили строку в список чисел
int x = (int)list[0]; // ручное приведение + риск исключения
```

Обобщения убирают оба недостатка — типобезопасность и приведение:

```csharp
List<int> numbers = new() { 1, 2, 3 };
numbers.Add(4);
// numbers.Add("текст"); // ошибка компиляции — тип уже известен
```

### 2. Обобщённые методы и классы

```csharp
T Max<T>(T a, T b) where T : IComparable<T> =>
    a.CompareTo(b) >= 0 ? a : b;
```

```csharp
Console.WriteLine(Max(3, 7));       // T = int
Console.WriteLine(Max("a", "z"));   // T = string
```

### 3. Ограничения `where`

| Ограничение | Смысл |
|---|---|
| `where T : class` | только ссылочные типы |
| `where T : struct` | только типы значения |
| `where T : new()` | обязателен конструктор без параметров |
| `where T : ISomeInterface` | реализует интерфейс |
| `where T : BaseClass` | наследник базового класса |

```csharp
void PrintAll<T>(IEnumerable<T> items) where T : class =>
    foreach (var item in items) Console.WriteLine(item);
```

### 4. Основные коллекции

**`List<T>`** — динамический массив.

```csharp
var users = new List<string> { "Иван", "Мария" };
users.Add("Пётр");
users.Remove("Иван");
Console.WriteLine(users[0]); // Мария
users.Sort();
```

**`Dictionary<TKey, TValue>`** — пары «ключ-значение», быстрый доступ по ключу.

```csharp
var ages = new Dictionary<string, int>
{
    ["Иван"] = 30
};
ages["Мария"] = 28;

if (ages.TryGetValue("Иван", out int age))
{
    Console.WriteLine(age);
}
```

**`HashSet<T>`** — уникальные элементы, мгновенная проверка принадлежности.

```csharp
var tags = new HashSet<string> { "csharp", "dotnet" };
tags.Add("csharp");      // уже есть — не добавится
Console.WriteLine(tags.Contains("dotnet")); // True
```

**`Queue<T>` / `Stack<T>`** — FIFO/LIFO.

```csharp
var tasks = new Queue<string>();
tasks.Enqueue("Первая");
tasks.Enqueue("Вторая");
var next = tasks.Dequeue(); // "Первая"

var undo = new Stack<string>();
undo.Push("A");
undo.Push("B");
var top = undo.Pop();       // "B"
```

### 5. `IEnumerable<T>` и `foreach`

`IEnumerable<T>` — минимальный контракт перебора. `foreach` работает с любым, кто его реализует.

```csharp
void PrintAll(IEnumerable<int> items)
{
    foreach (int n in items) Console.WriteLine(n);
}

PrintAll(new List<int> { 1, 2, 3 });
PrintAll(new[] { 4, 5 });
```

### 6. Интерфейсы только для чтения

На границах API не отдавай мутабельные коллекции. Отдавай `IReadOnlyList<T>`, `IReadOnlyCollection<T>`, `IReadOnlyDictionary<TKey, TValue>`:

```csharp
class Order
{
    private readonly List<string> _items = new();
    public IReadOnlyList<string> Items => _items; // снаружи не изменить
}
```

### 7. Big O коллекций

| Коллекция | Доступ по индексу/ключу | Поиск | Добавление |
|---|---|---|---|
| `List<T>` | O(1) | O(n) | O(1) в конец, O(n) в середину |
| `Dictionary<TKey,TValue>` | O(1) | O(1) | O(1) |
| `HashSet<T>` | нет | O(1) | O(1) |
| `Queue<T>`/`Stack<T>` | нет | O(n) | O(1) |
| отсортированные `SortedSet`/`SortedDictionary` | O(log n) | O(log n) | O(log n) |

### 8. Коллекционные выражения (C# 12)

```csharp
int[] a = [1, 2, 3];
List<int> b = [1, 2, 3];
int[] c = [.. a, .. b, 4]; // оператор spread
```

### 9. `yield return` (анонс)

Позволяет создавать `IEnumerable<T>` «лениво», без материализации. Подробно разберём в модуле 10.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `List<T>` там, где нужен `HashSet<T>` и наоборот | разная сложность поиска | выбирай по операции |
| Публичное `List<T>` в API | мутация извне | `IReadOnlyList<T>` |
| `Dictionary` перебор по ключу вместо `TryGetValue` | O(n) вместо O(1), некрасиво | `TryGetValue` |
| Изменение коллекции в `foreach` | `InvalidOperationException` | собрать список изменений, затем применить |
| `Search`-перебор `List.Contains` в цикле вложенном | O(n²) | `HashSet` |

## Контрольные вопросы

1. Что решают обобщения?
2. Какие ограничения `where` бывают?
3. Чем `List<T>` отличается от `Dictionary<TKey,TValue>`?
4. Зачем `HashSet<T>`?
5. Что такое `IEnumerable<T>`?
6. Почему не отдавать `List<T>` наружу?
7. Сколько стоит поиск в `Dictionary`? а в `List`?
8. Что такое коллекционные выражения?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
