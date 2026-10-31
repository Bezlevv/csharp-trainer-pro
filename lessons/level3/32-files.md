# Урок 32. Файлы

## Цель
Научиться читать и записывать файлы.

## Словарик новых слов
- **Путь** — адрес файла на диске (например, `data.txt`).
- **`File`** — класс-помощник для простых операций с файлами.
- **`File.ReadAllText`** — прочитать весь файл как текст.
- **`File.WriteAllText`** — записать текст в файл (перезаписав его).
- **`File.AppendAllText`** — дописать текст в конец файла.

## Теория «с нуля»

### Аналогия: блокнот и папка
Файл — это лист в блокноте. `ReadAllText` — прочитать весь лист. `WriteAllText` — стереть и написать заново. `AppendAllText` — дописать ещё строку снизу, не стирая ранее написанное.

### Запись файла
```csharp
File.WriteAllText("note.txt", "Привет, файл!");
```
Создаст файл `note.txt` с текстом. Если файл уже был — он будет перезаписан.

### Дописывание
```csharp
File.AppendAllText("log.txt", "Новая запись" + Environment.NewLine);
```
`Environment.NewLine` — «перевод строки» (перенос). Так каждая запись — с новой строки.

### Чтение
```csharp
string content = File.ReadAllText("note.txt");
Console.WriteLine(content);
```

### Проверка существования
```csharp
if (File.Exists("note.txt"))
{
    Console.WriteLine(File.ReadAllText("note.txt"));
}
else
{
    Console.WriteLine("Файла нет");
}
```

### Чтение по строкам
```csharp
string[] lines = File.ReadAllLines("notes.txt");
foreach (string line in lines)
{
    Console.WriteLine(line);
}
```

### Пути
Вместо зашитых путей используют `Path.Combine`:
```csharp
string path = Path.Combine(Environment.CurrentDirectory, "data", "notes.txt");
```
Это соберёт корректный путь для твоей операционной системы.

## Примеры

**Логгер (запись с датой):**
```csharp
string line = $"[{DateTime.Now:HH:mm:ss}] Пользователь вошёл";
File.AppendAllText("log.txt", line + Environment.NewLine);
```

## Сделай сам (задачи с подсказками)

1. Запиши «Привет» в `hello.txt` и прочитай его обратно.
   *Подсказка: `WriteAllText` затем `ReadAllText`.*
2. Допиши три строки в `log.txt` через `AppendAllText`.
   *Подсказка: не забывай `Environment.NewLine`.*
3. Если файл существует — выведи его содержимое, иначе создай с текстом «пусто».
   *Подсказка: `File.Exists`.*
4. Прочитай файл по строкам и выведи количество строк.
   *Подсказка: `File.ReadAllLines(...).Length`.*

## Разбор типичных ошибок

| Ошибка | Что не так | Как правильно |
|---|---|---|
| Чтение несуществующего файла | `FileNotFoundException` | проверяй `File.Exists` |
| Указан только `WriteAllText` и ждёшь, что допишется | он перезаписывает | для дописывания — `AppendAllText` |
| Путь «C:\\Users\\...» зашит жёстко | не работает на другой машине | `Path.Combine` |
| Строки без переносов | всё сливается в одну строку | `Environment.NewLine` |

## Тест — проверь себя

**Вопрос 1.** Что делает `File.WriteAllText("a.txt", "x")`?
- A) Дописывает «x» в конец.
- B) Записывает «x», перезаписав файл.
- C) Читает файл.

✅ **Верно: B.**
Почему: WriteAllText перезаписывает содержимое.
Почему неверны остальные:
- A — Дописывание — это `AppendAllText`.
- C — Чтение — это `ReadAllText`.

**Вопрос 2.** Что будет при `File.ReadAllText("нет.txt")`, если файла нет?
- A) Вернётся пустая строка.
- B) Будет брошено исключение (`FileNotFoundException`).
- C) Файл создастся автоматически.

✅ **Верно: B.**
Почему: чтение отсутствующего файла — ошибка.
Почему неверны остальные:
- A — Пустой строки не будет, будет исключение.
- C — Чтение не создаёт файл.

**Вопрос 3.** Зачем `Path.Combine`?
- A) Чтобы объединить части пути корректно для любой ОС.
- B) Чтобы прочитать файл.
- C) Чтобы удалить файл.

✅ **Верно: A.**
Почему: `Path.Combine` собирает путь безопасно.
Почему неверны остальные:
- B — Чтение — это `ReadAllText`.
- C — Удаление — это `File.Delete`.

**Вопрос 4.** Как дописать строку, не стирая прежнее содержимое?
- A) `File.WriteAllText`
- B) `File.AppendAllText`
- C) `File.ReadAllText`

✅ **Верно: B.**
Почему: `AppendAllText` добавляет в конец.
Почему неверны остальные:
- A — WriteAllText перезаписывает.
- C — ReadAllText читает.

## Итог урока
- `WriteAllText` — запись (перезапись), `AppendAllText` — дописывание.
- `ReadAllText` / `ReadAllLines` — чтение.
- `File.Exists` — проверка.
- `Path.Combine` — правильные пути.
