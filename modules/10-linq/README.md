# Модуль 10: LINQ

> **Цель:** писать декларативные запросы к коллекциям и понимать «ленивое» выполнение.

## Зачем это нужно

LINQ (Language Integrated Query) — встроенный язык запросов к коллекциям. Вместо десятков строк циклов и временных списков ты пишешь выразительный пайплайн: «отфильтруй, спроецируй, отсортируй, возьми топ-5». Это один из самых ценных навыков для .NET-разработчика — с LINQ вы сталкиваетесь практически в каждом реальном проекте.

## Теория

### 1. Два синтаксиса

**Методный (method syntax):**

```csharp
var adults = users.Where(u => u.Age >= 18).OrderBy(u => u.Name).ToList();
```

**Query-синтаксис:**

```csharp
var adults = (from u in users
              where u.Age >= 18
              orderby u.Name
              select u).ToList();
```

Оба эквивалентны. Методный встречается чаще — мы будем использовать его.

### 2. Отложенное выполнение (deferred execution)

Главная концепция! Запрос **не выполняется** в момент вызова `Where`/`Select`. Он выполняется при переборе (материализации).

```csharp
var query = numbers.Where(n => n > 2);       // пока ничего не посчитано
numbers.Add(10);                              // изменили источник
Console.WriteLine(query.Count());             // теперь пересчитается с учётом 10
```

Чтобы «зафиксировать» результат — материализуют:

```csharp
var list = query.ToList();      // или ToArray(), ToDictionary()
```

Повторный перебор «ленивого» запроса обычно пересчитывает источник — осторожно с дорогими источниками.

### 3. Основные операторы

**Фильтрация**

```csharp
var evens = numbers.Where(n => n % 2 == 0);
```

**Проекция**

```csharp
var names = users.Select(u => u.Name);
var nameAndAge = users.Select(u => new { u.Name, u.Age }); // анонимный тип
```

**Статистика**

```csharp
int count = numbers.Count();
int sum = numbers.Sum();
double avg = numbers.Average();
int min = numbers.Min();
int max = numbers.Max();
```

**Первый / последний / единственный**

```csharp
var first = numbers.First();           // исключение, если пусто
var firstOr = numbers.FirstOrDefault(); // null/0, если пусто
var single = numbers.Single(n => n == 5); // исключение, если не ровно один
var singleOr = numbers.SingleOrDefault(n => n == 5);
```

`Single` — опасен: бросит исключение, если совпадений больше одного.

**Проверки**

```csharp
bool anyEven = numbers.Any(n => n % 2 == 0);
bool allPositive = numbers.All(n => n > 0);
```

**Сортировка**

```csharp
var sorted = users.OrderBy(u => u.Name).ThenBy(u => u.Age);
var desc = users.OrderByDescending(u => u.Age);
```

**Уникальность и «нарезка»**

```csharp
var distinct = numbers.Distinct();
var top3 = numbers.OrderByDescending(n => n).Take(3);
var page = numbers.Skip(20).Take(10);
```

**Агрегация**

```csharp
int product = numbers.Aggregate(1, (acc, n) => acc * n);
```

**Группировка**

```csharp
var byAge = users.GroupBy(u => u.Age);
foreach (var group in byAge)
{
    Console.WriteLine($"{group.Key}: {group.Count()}");
}
```

**Join**

```csharp
var joined = authors.Join(books,
    author => author.Id,
    book => book.AuthorId,
    (author, book) => new { Author = author.Name, Book = book.Title });
```

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Не понимать deferred execution | повторный перебор, неожиданные результаты | материализуй `ToList()` при необходимости |
| `Select` в цикле вместо `SelectMany` | вложенные коллекции не «сплющиваются» | `SelectMany` |
| `Single` там, где возможен не один элемент | исключение в рантайме | `FirstOrDefault` / проверка `Any` |
| Вложенный `OrderBy` в цикле | лишняя пересортировка | сортируй один раз |
| `User.Where(...).ToList()` без нужды | лишняя аллокация | оставляй «ленивым» при однократном переборе |

## Контрольные вопросы

1. Что значит «отложенное выполнение»?
2. Когда нужна материализация `ToList()`/`ToArray()`?
3. Чем `FirstOrDefault` отличается от `SingleOrDefault`?
4. Что делает `SelectMany`?
5. Как сделать сортировку по двум ключам?
6. Как получить топ-N элементов?
7. Что возвращает `GroupBy`?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
