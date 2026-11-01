# Модуль 06.1: Строки и их методы (продолжение)

> **Цель:** уверенно работать со строками: поиск, замена, разбиение, сравнение, культура.

## Зачем это нужно
Строки — самый часто используемый тип данных. Методы `string` решают 90% текстовых задач: разбор ввода, поиск, форматирование, валидация. Знание этих методов экономит часы работы.

## Теория

### 1. Строки неизменяемы (immutable)
`string` нельзя изменить «на месте» — каждый метод возвращает **новую** строку, а оригинал остаётся. Это важно понимать, чтобы не «терять» результат:

```csharp
string s = "Привет";
s.ToUpper();          // строка НЕ изменилась! (результат брошен)
Console.WriteLine(s); // Привет

string up = s.ToUpper(); // теперь up == "ПРИВЕТ"
```

### 2. Поиск и проверка

```csharp
string s = "C# — это круто";
Console.WriteLine(s.Length);              // 14
Console.WriteLine(s.Contains("круто"));   // True
Console.WriteLine(s.StartsWith("C#"));    // True
Console.WriteLine(s.EndsWith("!"));       // False
Console.WriteLine(s.IndexOf("круто"));    // 8 (позиция)
Console.WriteLine(s.IndexOf("zzz"));      // -1 (не найдено)
```

### 3. Вырезание и замена

```csharp
Console.WriteLine(s.Substring(0, 2));     // "C#" — первые 2 символа
Console.WriteLine(s.Replace("круто", "сильно")); // "C# — это сильно"
Console.WriteLine(s.Trim());              // убирает пробелы по краям
Console.WriteLine("  a  ".TrimStart());   // "a  "
Console.WriteLine("  a  ".TrimEnd());     // "  a"
```

### 4. Разбиение и сборка
```csharp
string csv = "Иван;30;Москва";
string[] parts = csv.Split(';');
Console.WriteLine(parts[0]);   // Иван

string joined = string.Join(", ", parts);
Console.WriteLine(joined);     // Иван, 30, Москва
```

### 5. Сравнение строк
`==` сравнивает по значению (для string это работает), но есть нюансы:
```csharp
Console.WriteLine("abc" == "abc");          // True
Console.WriteLine("abc" == "ABC");          // False (регистр важен)

Console.WriteLine(string.Equals("abc", "ABC", StringComparison.OrdinalIgnoreCase)); // True
Console.WriteLine("abc".Equals("ABC", StringComparison.OrdinalIgnoreCase));         // True
```
Для культуры-зависимых сравнений (русский текст) — `StringComparison.CurrentCulture`; для технических — `Ordinal`.

### 6. Пустые строки и null
```csharp
Console.WriteLine(string.IsNullOrEmpty(""));      // True
Console.WriteLine(string.IsNullOrWhiteSpace("  ")); // True
```

### 7. Интерполяция и форматирование
```csharp
string name = "Иван";
int age = 30;
Console.WriteLine($"{name} — {age} лет");

Console.WriteLine($"Цена: {1234.5:F2}");  // Цена: 1234,50
Console.WriteLine($"Число: {42:D5}");     // Число: 00042
Console.WriteLine($"{0.25:P0}");          // 25%
```

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `s.ToUpper()` без присваивания | результат теряется | `s = s.ToUpper();` |
| `Split(" ")` с несколькими пробелами | пустые элементы | `Split(' ', StringSplitOptions.RemoveEmptyEntries)` |
| `IndexOf` вернул −1, а мы берём Substring | исключение | проверяй результат >= 0 |
| Сравнение `==` со строками из ввода | регистр/невидимые символы | `Trim()` + `StringComparison` |

## Контрольные вопросы
1. Почему `s.ToUpper()` не меняет `s`?
2. Что вернёт `IndexOf` при отсутствии подстроки?
3. Как разбить строку по `;`?
4. Чем `Equals(..., OrdinalIgnoreCase)` лучше `==` для ввода?
5. Что вернёт `IsNullOrWhiteSpace("  ")`?

## Дальше
- [Практика](PRACTICE.md) — задачи
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки
