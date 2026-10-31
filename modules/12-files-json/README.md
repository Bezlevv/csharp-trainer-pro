# Модуль 12: Файлы, даты и сериализация

> **Цель:** работать с файловой системой, датами и JSON как с повседневными инструментами.

## Зачем это нужно

Любое реальное приложение читает и пишет данные: конфигурацию, логи, пользовательский контент, обмен с API. Этим правят три технологии: `System.IO` (файлы), `System.Text.Json` (сериализация), `DateTime`/`DateOnly` (время). Уверенное владение ими делает тебя продуктивным сразу.

## Теория

### 1. Работа с файлами: `System.IO`

**Пути.**

Не склеивай строки вручную — `Path.Combine`:

```csharp
string root = Environment.CurrentDirectory;
string logPath = Path.Combine(root, "logs", "app.log");
```

**Проверка существования.**

```csharp
Console.WriteLine(File.Exists("data.txt"));
Console.WriteLine(Directory.Exists("logs"));
```

**Чтение.**

```csharp
string all = File.ReadAllText("data.txt");
string[] lines = File.ReadAllLines("data.txt");
```

**Запись.**

```csharp
File.WriteAllText("out.txt", "текст");
File.AppendAllText("log.txt", "новая строка\n");
```

**Построчно (для больших файлов):**

```csharp
using var reader = new StreamReader("big.log");
string? line;
while ((line = reader.ReadLine()) is not null)
{
    Console.WriteLine(line.Length);
}
```

**Кодировки.**

По умолчанию — UTF-8. Проблемы «кракозябр» обычно из-за неверной кодировки на чтении/записи:

```csharp
File.WriteAllText("file.txt", "текст", System.Text.Encoding.UTF8);
```

### 2. Сериализация JSON: `System.Text.Json`

`JsonSerializer` — встроен в .NET.

```csharp
record Product(string Name, decimal Price);
```

**Сериализация:**

```csharp
var product = new Product("Мышь", 1499.99m);
string json = JsonSerializer.Serialize(product);
Console.WriteLine(json); // {"Name":"Мышь","Price":1499.99}
```

**Десериализация:**

```csharp
var loaded = JsonSerializer.Deserialize<Product>(json);
Console.WriteLine(loaded!.Name);
```

**Варианты опций:**

```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};
```

**Десериализация списка:**

```csharp
var products = JsonSerializer.Deserialize<List<Product>>(json);
```

### 3. Работа с датами

**`DateTime`** — момент времени (дата + время). Удобен для меток времени.

```csharp
DateTime now = DateTime.UtcNow;    // для логики
DateTime local = DateTime.Now;     // для отображения пользователю
```

**`DateOnly` / `TimeOnly`** — просто дата / просто время (C# 10+).

```csharp
DateOnly today = DateOnly.FromDateTime(DateTime.Today);
DateOnly birthday = new(2000, 5, 17);
int age = today.Year - birthday.Year;
```

**`TimeSpan`** — отрезок времени.

```csharp
TimeSpan elapsed = now - start;
Console.WriteLine(elapsed.TotalMinutes);
```

**Разбор и форматирование:**

```csharp
DateTime date = DateTime.Parse("2024-01-15");
string text = date.ToString("dd.MM.yyyy");      // 15.01.2024
string iso = date.ToString("O");                // ISO 8601
```

> Для пользовательского ввода используй `TryParse` с `CultureInfo`, для внутреннего представления — `InvariantCulture`.

### 4. Сериализация дат

`System.Text.Json` сериализует в ISO 8601. При десериализации переопределяй формат явно.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Путь жёстко зашит | программа не работает на другой машине | `Path.Combine` + относительные пути |
| `File.ReadAllText` на огромном логе | память | `StreamReader` построчно |
| `DateTime.Now` в бизнес-логике | недетерминированно, трудно тестировать | параметр или `IClock`/`DateTime.UtcNow` |
| Игнорирование кодировок | кракозябры | `Encoding.UTF8` |

## Контрольные вопросы

1. Зачем `Path.Combine`?
2. Как прочитать файл построчно?
3. Что делает `JsonSerializer`?
4. Как включить отступы в JSON-выводе?
5. Чем `DateTime.UtcNow` отличается от `DateTime.Now`?
6. Что такое `DateOnly`?
7. Что делает `TimeSpan`?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
