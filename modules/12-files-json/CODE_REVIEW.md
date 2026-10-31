# Разбор кода — Модуль 12

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: Захардкоженный путь

### Исходный код

```csharp
string path = "C:\\Users\\ivan\\Documents\\save.txt";
File.WriteAllText(path, data);
```

### Что не так

- Не работает на другой машине/аккаунте, ломается при переименовании папок.

### Как лучше

```csharp
string path = Path.Combine(Environment.CurrentDirectory, "data", "save.txt");
Directory.CreateDirectory(Path.GetDirectoryName(path)!);
```

### Вывод

Относительные пути + `Path.Combine` — переносимость.

## Пример 2: `File.ReadAllText` на огромном логе

### Исходный код

```csharp
string entire = File.ReadAllText("server.log"); // может быть сотни МБ
```

### Что не так

- Весь файл загружается в память. Для логов/стоков — зря.

### Как лучше

```csharp
using var reader = new StreamReader("server.log");
string? line;
while ((line = reader.ReadLine()) is not null)
{
    // обработка одной строки
}
```

### Вывод

Построчное чтение для больших файлов.

## Пример 3: `DateTime.Now` в бизнес-логике

### Исходный код

```csharp
bool IsExpired(Order order)
{
    return DateTime.Now > order.Deadline; // зависит от времени запуска
}
```

### Что не так

- Тесты непредсказуемы: результат зависит от текущего момента. Плюс `DateTime.Now` — локальное время, не всегда уместно.

### Как лучше

```csharp
bool IsExpired(Order order, DateTime now)
{
    return now > order.Deadline;
}
```

### Вывод

Время — параметр или абстракция (`IClock`). Код становится детерминированным и тестируемым.

## Пример 4: Игнорирование кодировки

### Исходный код

```csharp
File.WriteAllText("file.txt", "Привет, мир"); // может записаться не в UTF-8
```

### Что не так

- Разные платформы могут дать «кракозябры» при чтении.

### Как лучше

```csharp
File.WriteAllText("file.txt", "Привет, мир", System.Text.Encoding.UTF8);
```

### Вывод

Явно задавай кодировку для текстовых файлов.

## Пример 5: Копирование без проверки существования

### Исходный код

```csharp
File.Copy("old.txt", "new.txt"); // FileNotFoundException, если old.txt нет
```

### Что не так

- Необработанное исключение обрывает программу неожиданно.

### Как лучше

```csharp
if (File.Exists("old.txt"))
{
    File.Copy("old.txt", "new.txt", overwrite: true);
}
else
{
    Console.WriteLine("old.txt не существует");
}
```

### Вывод

Проверяй существование и обрабатывай исключения ввода-вывода.
