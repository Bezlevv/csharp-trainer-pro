# Практика — Модуль 12

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Логгер в файл

**Сложность:** ★☆☆
**Описание:** Пиши строки в файл с меткой времени.

**Требования:**
- `File.AppendAllText`.
- Каждая запись: `[2024-01-15 12:00:00] сообщение`.

## Задача 2: Заметки с хранением в JSON

**Сложность:** ★★☆
**Описание:** Меню «добавить / показать / выйти»; заметки сохраняются в `notes.json`.

**Требования:**
- `List<string>` → `JsonSerializer.Serialize` → `File.WriteAllText`.
- При старте — загрузка через `Deserialize`.

## Задача 3: Чтение большого файла построчно

**Сложность:** ★★☆
**Описание:** Посчитай число строк и слов в файле через `StreamReader`.

**Требования:**
- `while ((line = reader.ReadLine()) is not null)`.
- Покажи разницу с `ReadAllText`.

## Задача 4: Копирование с проверкой

**Сложность:** ★★☆
**Описание:** Скопируй файл, обработай `FileNotFoundException`.

**Требования:**
- `File.Copy` с проверкой источника.
- Поймай `FileNotFoundException` и `IOException`.

## Задача 5: CSV → JSON

**Сложность:** ★★★
**Описание:** Прочитай CSV, преврати в список records, сериализуй в JSON.

**Требования:**
- Формат: `Имя;Возраст;Город`.
- `record Person(string Name, int Age, string City)`.
- Выведи JSON в консоль.

## Задача 6: Даты — возраст и дедлайн

**Сложность:** ★★☆
**Описание:** Вычисли возраст по `DateOnly` рождения; сколько дней до дедлайна.

**Требования:**
- `DateOnly` и `TimeSpan`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
string logPath = Path.Combine(Environment.CurrentDirectory, "log.txt");
string message = "Пользователь вошёл в систему";

string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
File.AppendAllText(logPath, line + Environment.NewLine);

Console.WriteLine(File.ReadAllText(logPath));
```

**Пояснение:** `File.AppendAllText` добавляет без перезаписи. Формат даты — через `:yyyy-MM-dd HH:mm:ss`.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
string path = "notes.json";
List<string> notes = File.Exists(path)
    ? JsonSerializer.Deserialize<List<string>>(File.ReadAllText(path)) ?? new List<string>()
    : new List<string>();

while (true)
{
    Console.WriteLine("1 — добавить, 2 — показать, 0 — выйти");
    string? key = Console.ReadLine();

    if (key == "1")
    {
        Console.Write("Заметка: ");
        notes.Add(Console.ReadLine() ?? "");
        File.WriteAllText(path, JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true }));
    }
    else if (key == "2")
    {
        foreach (var note in notes) Console.WriteLine($"- {note}");
    }
    else if (key == "0")
    {
        break;
    }
}
```

**Пояснение:** `??` для защиты от `null` при десериализации. Каждое добавление перезаписывает файл.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
int lineCount = 0, wordCount = 0;

using var reader = new StreamReader("big.txt");
string? line;

while ((line = reader.ReadLine()) is not null)
{
    lineCount++;
    wordCount += line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
}

Console.WriteLine($"Строк: {lineCount}, слов: {wordCount}");
```

**Пояснение:** `StreamReader` держит в памяти лишь одну строку — большие файлы читаются без перегруза памяти.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
string source = "data.txt";
string dest = "data_copy.txt";

try
{
    if (!File.Exists(source)) throw new FileNotFoundException("Файл не найден", source);
    File.Copy(source, dest, overwrite: true);
    Console.WriteLine("Скопировано");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"Источник не найден: {ex.FileName}");
}
catch (IOException ex)
{
    Console.WriteLine($"Ошибка копирования: {ex.Message}");
}
```

**Пояснение:** `FileNotFoundException` — специфичная обработка отсутствия файла; `IOException` ловит проблемы ввода-вывода.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
record Person(string Name, int Age, string City);

string csv = "Иван;30;Москва\nМария;28;Питер\n";

var people = new List<Person>();
foreach (string row in csv.Split('\n', StringSplitOptions.RemoveEmptyEntries))
{
    string[] parts = row.Split(';');
    people.Add(new Person(parts[0], int.Parse(parts[1]), parts[2]));
}

string json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(json);
```

**Пояснение:** Разбор CSV построчно, затем сериализация в JSON.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
DateOnly today = DateOnly.FromDateTime(DateTime.Today);
DateOnly birth = new(2000, 5, 17);

int age = today.Year - birth.Year;
if (today < birth.AddYears(age)) age--;

DateOnly deadline = new(2024, 12, 31);
TimeSpan until = deadline.ToDateTime(TimeOnly.MinValue) - DateTime.Today;

Console.WriteLine($"Возраст: {age}");
Console.WriteLine($"До дедлайна: {until.Days} дней");
```

**Пояснение:** Коррекция возраста для дней рождения «в будущем». `DateOnly`→`DateTime` для разницы.

</details>
