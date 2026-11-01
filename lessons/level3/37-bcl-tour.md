# Урок 37. Экскурсия по стандартной библиотеке

## Цель
Познакомиться с часто используемыми готовыми помощниками .NET: `string`, `Math`, `Random`, `DateTime`, `Guid`.

## Словарик новых слов
- **BCL (Base Class Library)** — стандартная библиотека классов .NET. В ней — сотни готовых инструментов.
- **Класс-помощник** — класс со статическими методами (например, `Math`).

## Теория «с нуля»

### Зачем это нужно
Не нужно писать всё самому. Многие задачи уже решены за нас — важно знать, что они существуют.

### string — работа с текстом
```csharp
string s = "Привет, мир!";
Console.WriteLine(s.Length);         // 13
Console.WriteLine(s.ToUpper());      // ПРИВЕТ, МИР!
Console.WriteLine(s.ToLower());      // привет, мир!
Console.WriteLine(s.Trim());         // убирает пробелы с краёв
Console.WriteLine(s.Replace("мир", "C#")); // Привет, C#!
string[] parts = s.Split(", ");      // ["Привет", "мир!"]
Console.WriteLine(s.StartsWith("Привет")); // True
```

### Math — математика
```csharp
Console.WriteLine(Math.Max(3, 9));    // 9
Console.WriteLine(Math.Min(3, 9));    // 3
Console.WriteLine(Math.Abs(-5));      // 5
Console.WriteLine(Math.Pow(2, 10));   // 1024
Console.WriteLine(Math.Sqrt(81));     // 9
Console.WriteLine(Math.Round(3.14159, 2)); // 3.14
```

### Random — случайные числа
```csharp
Random rnd = new Random();
Console.WriteLine(rnd.Next(1, 101));   // целое от 1 до 100
Console.WriteLine(rnd.NextDouble());   // дробное от 0 до 1
```
`Random.Shared` — один общий генератор (удобно): `Random.Shared.Next(1, 101)`.

### DateTime — даты и время
```csharp
DateTime now = DateTime.Now;        // текущее (локальное)
DateTime utc = DateTime.UtcNow;     // текущее (UTC — для логики)
Console.WriteLine(now.ToString("dd.MM.yyyy"));
TimeSpan days = DateTime.Today - new DateTime(2000, 5, 17);
Console.WriteLine(days.Days);       // сколько дней прошло
```

### Guid — уникальные идентификаторы
```csharp
Guid id = Guid.NewGuid();
Console.WriteLine(id);   // например, 3f2a...-...-...
```
`Guid` — почти гарантированно уникальный идентификатор (для id записей).

## Примеры

```csharp
// Случайный пароль из 8 символов (маленькая демонстрация)
Random rnd = Random.Shared;
string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
string password = "";
for (int i = 0; i < 8; i++) password += chars[rnd.Next(chars.Length)];
Console.WriteLine(password);
```

## Сделай сам (задачи с подсказками)

1. Выведи длину, верхний регистр и слово по индексу для `"C# — это круто"`.
   *Подсказка: `Split(' ')` → `parts[0]`.*
2. Посчитай `2^10` и `√(144)` через Math.
   *Подсказка: `Math.Pow(2,10)`, `Math.Sqrt(144)`.*
3. Выведи 3 случайных числа от 1 до 6.
   *Подсказка: `Random.Shared.Next(1,7)` в цикле.*
4. Выведи текущую дату в формате «дд.мм.гггг».
   *Подсказка: `DateTime.Today.ToString("dd.MM.yyyy")`.*
5. Создай случайный Guid и выведи его.
   *Подсказка: `Guid.NewGuid()`.*

## Разбор типичных ошибок

| Ошибка | Что не так | Как правильно |
|---|---|---|
| `rnd.Next(1, 101)` и ожидание 101 | верхняя граница НЕ включается | `Next(1, 100)` для 1..100 |
| `DateTime.Now` в логике | локальное время, лучше Utc | `DateTime.UtcNow` |
| `Split(" ")` с несколькими пробелами | пустые элементы | `Split(' ', StringSplitOptions.RemoveEmptyEntries)` |
| Складывать строки в цикле без нужды | медленно | `StringBuilder`/join |

## Тест — проверь себя

**Вопрос 1.** Что вернёт `Math.Max(4, 7)`?
- A) 4
- B) 7
- C) 11

✅ **Верно: B.**
Почему: Max возвращает большее.
Почему неверны остальные:
- A — 4 меньше.
- C — 11 — это сумма.

**Вопрос 2.** `Random.Shared.Next(1, 7)` даёт числа:
- A) от 1 до 7 включительно.
- B) от 1 до 6 включительно.
- C) от 0 до 6.

✅ **Верно: B.**
Почему: верхняя граница не включается → 1..6.
Почему неверны остальные:
- A — 7 не включается.
- C — нижняя граница 1, не 0.

**Вопрос 3.** Что делает `s.Trim()`?
- A) Делит строку.
- B) Убирает пробелы с краёв.
- C) Делает строку заглавной.

✅ **Верно: B.**
Почему: Trim обрезает пробелы по краям.
Почему неверны остальные:
- A — Деление — это Split.
- C — Заглавные — это ToUpper.

**Вопрос 4.** Для уникального идентификатора лучше всего использовать:
- A) `Math.Max`
- B) `Guid.NewGuid()`
- C) `DateTime.Now`

✅ **Верно: B.**
Почему: Guid почти гарантированно уникален.
Почему неверны остальные:
- A — Max — это не про уникальность.
- C — Время не уникально.

## Итог урока
- `string` — методы для работы с текстом.
- `Math` — математические функции.
- `Random` — случайные числа (`Next(1, 7)` не включает 7).
- `DateTime` — даты (в логике — `UtcNow`).
- `Guid.NewGuid()` — уникальные id.
