# Практика — Модуль 10

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Фильтрация пользователей

**Сложность:** ★☆☆
**Описание:** Список записей `Person(Name, Age)`. Отбери совершеннолетних, спроецируй имена.

**Требования:**
- `Where`, `Select`, `ToList`.

## Задача 2: Группировка заказов

**Сложность:** ★★☆
**Описание:** `Order(Client, Amount)`. Сгруппируй по клиенту, для каждого посчитай сумму.

**Требования:**
- `GroupBy`, внутри группы `Sum`.
- Выведи клиент → общая сумма.

## Задача 3: Join авторов и книг

**Сложность:** ★★★
**Описание:** Два списка: `Author(Id, Name)` и `Book(AuthorId, Title)`. Соедини.

**Требования:**
- `Join` + проекция в анонимный тип.
- Выведи: автор → список его книг.

## Задача 4: Топ-N и пагинация

**Сложность:** ★★☆
**Описание:** `Score(Player, Points)`.

**Требования:**
- Топ-3 игрока (`OrderByDescending` + `Take`).
- Вторая «страница» (Skip/Take).

## Задача 5: Рефакторинг трёх циклов

**Сложность:** ★★★
**Описание:** Дан массив `int[]`. Тремя циклами считались: чётные, сумма, максимум.

**Требования:**
- Перепиши в один LINQ-пайплайн (или в три понятных оператора).

## Задача 6: `Batch<T>`

**Сложность:** ★★★
**Описание:** Напиши extension-метод `Batch<T>(this IEnumerable<T>, int size)`, возвращающий «страницы».

**Требования:**
- Возвращает `IEnumerable<IEnumerable<T>>`.
- Используй `yield return`.
- Массив из 10, `Batch(4)` → 4, 4, 2.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
var people = new[] { new Person("Иван", 16), new Person("Мария", 30) };

var adults = people
    .Where(p => p.Age >= 18)
    .Select(p => p.Name)
    .ToList();

foreach (var name in adults) Console.WriteLine(name); // Мария

record Person(string Name, int Age);
```

**Пояснение:** `record` для данных — компактно и читаемо. `Where`→`Select` — типичный пайплайн.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
var orders = new[]
{
    new Order("Иван", 100),
    new Order("Мария", 250),
    new Order("Иван", 50),
    new Order("Мария", 300)
};

var byClient = orders
    .GroupBy(o => o.Client)
    .Select(g => new { Client = g.Key, Total = g.Sum(o => o.Amount) });

foreach (var item in byClient)
{
    Console.WriteLine($"{item.Client}: {item.Total}");
}
```

**Пояснение:** `GroupBy` возвращает группы, у каждой есть `Key` и агрегаты (`Sum`).

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
var authors = new[]
{
    new Author(1, "Толстой"),
    new Author(2, "Достоевский")
};

var books = new[]
{
    new Book(1, "Война и мир"),
    new Book(1, "Анна Каренина"),
    new Book(2, "Преступление и наказание")
};

var joined = authors.Join(books,
    author => author.Id,
    book => book.AuthorId,
    (author, book) => new { author.Name, book.Title });

foreach (var item in joined)
{
    Console.WriteLine($"{item.Name}: {item.Title}");
}
```

**Пояснение:** `Join` — внутреннее соединение по ключу `Id` ↔ `AuthorId`. Проекция собирает итог.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
var scores = new[]
{
    new Score("Иван", 10),
    new Score("Мария", 8),
    new Score("Пётр", 9),
    new Score("Анна", 7),
    new Score("Олег", 6)
};

var top3 = scores.OrderByDescending(s => s.Points).Take(3);
var page2 = scores.OrderByDescending(s => s.Points).Skip(3).Take(2);

Console.WriteLine("Топ-3:");
foreach (var s in top3) Console.WriteLine($"{s.Player}: {s.Points}");
```

**Пояснение:** `Take`/`Skip` — работают как пагинация после сортировки.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
int[] numbers = { 1, 2, 3, 4, 5, 6 };

var evens = numbers.Where(n => n % 2 == 0).ToArray();
int sum = numbers.Sum();
int max = numbers.Max();

Console.WriteLine($"Чётные: {string.Join(", ", evens)}"); // 2, 4, 6
Console.WriteLine($"Сумма: {sum}");
Console.WriteLine($"Максимум: {max}");
```

**Пояснение:** Три понятных оператора вместо трёх циклов с временными переменными.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
static class EnumerableExtensions
{
    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
    {
        if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size));

        var batch = new List<T>(size);
        foreach (var item in source)
        {
            batch.Add(item);
            if (batch.Count == size)
            {
                yield return batch;
                batch = new List<T>(size);
            }
        }

        if (batch.Count > 0) yield return batch;
    }
}

var data = Enumerable.Range(1, 10);
foreach (var chunk in data.Batch(4))
{
    Console.WriteLine(string.Join(", ", chunk)); // 1 2 3 4 | 5 6 7 8 | 9 10
}
```

**Пояснение:** `yield return` делает метод «ленивым»: элементы поставляются по мере перебора, без полной материализации.

</details>
