# Практика — Модуль 07

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Телефонная книга

**Сложность:** ★☆☆
**Описание:** Функции-«книги» на `Dictionary<string, string>` (имя → телефон).

**Требования:**
- Добавить, найти, удалить контакт.
- `TryGetValue` для поиска.

## Задача 2: Частотный анализ слов

**Сложность:** ★★☆
**Описание:** Разбей текст на слова, посчитай частоту каждого.

**Требования:**
- `Dictionary<string, int>`.
- Игнорируй знаки препинания и регистр.
- Выведи топ-5 слов.

**Подсказка:** `Split` с разделителями, `ToLower()`, сортировка пары по значению.

## Задача 3: Уникальные посетители

**Сложность:** ★★☆
**Описание:** Два списка визитов за день; найди общих и уникальных посетителей.

**Требования:**
- `HashSet<string>`.
- Выведи: только в день 1, только в день 2, общие.

## Задача 4: Обобщённый стек с максимумом

**Сложность:** ★★★
**Описание:** Класс `MaxStack<T>` с ограничением `where T : IComparable<T>`.

**Требования:**
- `Push`, `Pop`, `Max` (максимум среди элементов).
- Поле хранит стек; `Max()` — проход по всем.

**Подсказка:** `IComparable<T>`, сравнение `CompareTo`.

## Задача 5: Очередь печати

**Сложность:** ★☆☆
**Описание:** Симуляция очереди печати через `Queue<int>`.

**Требования:**
- Добавляй задачи (`Enqueue`), обрабатывай (`Dequeue`), пока не пусто.

## Задача 6: Рефакторинг на `List<T>`

**Сложность:** ★★☆
**Описание:** Массив участников соревнования → список с возможностью добавить/удалить.

**Требования:**
- Замени `int[]` на `List<int>`.
- Реализуй «добавить участника», «удалить по номеру», «вывести средний балл».

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
var phoneBook = new Dictionary<string, string>();

phoneBook["Иван"] = "+7 900 000-00-01";
phoneBook["Мария"] = "+7 900 000-00-02";

Console.Write("Чей номер ищем? ");
string name = Console.ReadLine() ?? "";

if (phoneBook.TryGetValue(name, out string? number))
{
    Console.WriteLine($"{name}: {number}");
}
else
{
    Console.WriteLine("Контакт не найден");
}
phoneBook.Remove("Мария");
```

**Пояснение:** `TryGetValue` безопасно возвращает значение по ключу и сообщает о его наличии.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
string text = "кот и кот, собака и кот. собака и птица";
char[] separators = { ' ', ',', '.', ';', ':', '!', '?' };

var freq = new Dictionary<string, int>();
foreach (string word in text.Split(separators, StringSplitOptions.RemoveEmptyEntries))
{
    string normalized = word.ToLower();
    freq[normalized] = freq.GetValueOrDefault(normalized) + 1;
}

foreach (var pair in freq.OrderByDescending(p => p.Value).Take(5))
{
    Console.WriteLine($"{pair.Key}: {pair.Value}");
}
```

**Пояснение:** `GetValueOrDefault` удобен, когда отсутствие ключа = 0. `OrderByDescending` — оператор LINQ (подробно — модуль 10).

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
var day1 = new HashSet<string> { "Иван", "Мария", "Пётр" };
var day2 = new HashSet<string> { "Мария", "Анна", "Пётр" };

var common = day1.Intersect(day2);
var onlyDay1 = day1.Except(day2);
var all = day1.Union(day2);

Console.WriteLine("Общие: " + string.Join(", ", common));
Console.WriteLine("Только день 1: " + string.Join(", ", onlyDay1));
Console.WriteLine("Всего уникальных: " + all.Count());
```

**Пояснение:** `HashSet` предоставляет готовые операции множеств, эффективнее ручных переборов.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
class MaxStack<T> where T : IComparable<T>
{
    private readonly Stack<T> _items = new();

    public void Push(T item) => _items.Push(item);

    public T Pop() => _items.Pop();

    public T Max()
    {
        if (_items.Count == 0)
            throw new InvalidOperationException("Стек пуст");

        T max = _items.Peek();
        foreach (T item in _items)
        {
            if (item.CompareTo(max) > 0) max = item;
        }
        return max;
    }
}

var ms = new MaxStack<int>();
ms.Push(3);
ms.Push(9);
ms.Push(4);
Console.WriteLine(ms.Max()); // 9
```

**Пояснение:** Ограничение `where T : IComparable<T>` позволяет сравнивать элементы любого обобщённого типа.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
var printQueue = new Queue<int>();
printQueue.Enqueue(101);
printQueue.Enqueue(102);
printQueue.Enqueue(103);

while (printQueue.Count > 0)
{
    int job = printQueue.Dequeue();
    Console.WriteLine($"Печатаю задачу {job}");
}
```

**Пояснение:** FIFO — первой обрабатывается задача, добавленная первой.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
var participants = new List<int> { 10, 20, 30 };

participants.Add(40);   // кандидат
participants.Remove(20); // дисквалифицирован

if (participants.Count == 0)
{
    Console.WriteLine("Участников нет");
}
else
{
    double average = participants.Average();
    Console.WriteLine($"Участников: {participants.Count}, средний балл: {average:F2}");
}
```

**Пояснение:** `List<T>` удобнее массива: добавление и удаление без ручного изменения размера.

</details>
