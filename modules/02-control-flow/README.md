# Модуль 02: Управление потоком

> **Цель:** свободно ветвить логику и организовывать циклы.

## Зачем это нужно

Программа без ветвлений — просто последовательность команд. Любая реальная программа принимает решения: если пользователь ввёл неверный пароль, если баланс меньше суммы — и повторяет действия: пока не введено число, пока не закончились товары в списке. Управление потоком — это «мозг» программы: здесь рождается вся её логика.

## Теория

### 1. Условные конструкции `if` / `else if` / `else`

```csharp
int age = 25;

if (age < 18)
{
    Console.WriteLine("Несовершеннолетний");
}
else if (age < 65)
{
    Console.WriteLine("Взрослый");
}
else
{
    Console.WriteLine("Пенсионер");
}
```

**Правило:** чем раньше выходишь из ветвлений, тем читабельнее код. Это называется **guard clauses**:

```csharp
void SetAge(int age)
{
    if (age <= 0 || age > 120)
    {
        Console.WriteLine("Некорректный возраст");
        return; // ранний выход
    }

    Console.WriteLine($"Возраст установлен: {age}");
}
```

**Тернарный оператор** — для простого выбора значения:

```csharp
string status = age >= 18 ? "Взрослый" : "Несовершеннолетний";
```

### 2. `switch` и `switch`-выражения

**Классический `switch`:**

```csharp
string day = Console.ReadLine() ?? "";

switch (day.ToLower())
{
    case "пн":
    case "вт":
    case "ср":
        Console.WriteLine("Рабочий день");
        break;
    case "сб":
    case "вс":
        Console.WriteLine("Выходной");
        break;
    default:
        Console.WriteLine("Не знаю такой день");
        break;
}
```

**`switch`-выражение** (C# 8+) — компактнее:

```csharp
string message = day.ToLower() switch
{
    "пн" or "вт" or "ср" => "Рабочий день",
    "сб" or "вс" => "Выходной",
    _ => "Не знаю такой день"
};
```

**Паттерны** (C# 9+):

```csharp
string Classify(int number) => number switch
{
    > 0 => "Положительное",
    < 0 => "Отрицательное",
    _ => "Ноль"
};
```

`when` — дополнительное условие:

```csharp
string Describe(int score) => score switch
{
    >= 90 when score <= 100 => "Отлично",
    >= 70 => "Хорошо",
    >= 50 => "Удовлетворительно",
    _ => "Плохо"
};
```

### 3. Циклы

**`for`** — когда известно количество повторений:

```csharp
for (int i = 0; i < 5; i++)
{
    Console.WriteLine(i); // 0 1 2 3 4
}
```

**`while`** — когда условие проверяется в начале:

```csharp
int count = 0;
while (count < 3)
{
    Console.WriteLine("Повтор");
    count++;
}
```

**`do-while`** — когда тело должно выполниться хотя бы раз:

```csharp
string? input;
do
{
    Console.Write("Введи 'да': ");
    input = Console.ReadLine();
}
while (input != "да");
```

**`foreach`** — перебор коллекций и строк:

```csharp
string word = "C#";
foreach (char ch in word)
{
    Console.WriteLine(ch);
}

int[] numbers = { 1, 2, 3 };
foreach (int n in numbers)
{
    Console.WriteLine(n);
}
```

### 4. `break` и `continue`

- `break` — немедленно выйти из цикла.
- `continue` — перейти к следующей итерации.

```csharp
for (int i = 1; i <= 10; i++)
{
    if (i == 3) continue;   // пропустить 3
    if (i == 8) break;      // остановиться на 8
    Console.WriteLine(i);    // 1 2 4 5 6 7
}
```

### 5. Вечный цикл с выходом

```csharp
while (true)
{
    Console.Write("Команда (exit — выход): ");
    string command = Console.ReadLine() ?? "";

    if (command == "exit") break;
    Console.WriteLine($"Выполняю: {command}");
}
```

### 6. Вложенные циклы

```csharp
for (int i = 1; i <= 3; i++)
{
    for (int j = 1; j <= 3; j++)
    {
        Console.Write($"{i * j} ");
    }
    Console.WriteLine();
}
```

### 7. Почему не `goto`

`goto` существует, но почти всегда превращает код в «спагетти». Практически любую проблему можно решить циклом, `break` или методом. Не используй `goto` без крайней нужды.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `else` после `return` | Лишние блоки ухудшают читаемость | ранний `return` вместо `else` |
| Сравнение `==` с `"Да"` без учета регистра | `"да"` ≠ `"Да"` | сравнивай через `.Trim().ToLower()` или `StringComparison.OrdinalIgnoreCase` |
| Бесконечный цикл без выхода | Зависание программы | контролируй `break` или условие |
| Глубокая вложенность if'ов | Код трудно читать | guard clauses, ранний выход |

## Контрольные вопросы

1. Когда используется `else if`?
2. Что такое guard clause? Приведи пример.
3. Чем классический `switch` отличается от `switch`-выражения?
4. Что делает `when` в паттерне?
5. В чём разница `while` и `do-while`?
6. Что делает `break`, а что `continue`?
7. Когда использовать `for`, а когда `foreach`?
8. Как запилить вечный цикл с выходом по команде?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
