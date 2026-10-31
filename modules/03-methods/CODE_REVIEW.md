# Разбор кода — Модуль 03

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: God-метод

### Исходный код

```csharp
void ProcessOrder(string orderId)
{
    // 1. Валидация
    // 2. Расчёт скидок
    // 3. Проверка остатков
    // 4. Оформление
    // 5. Отправка e-mail
    // ... 80 строк логики в одном методе
}
```

### Что не так

- Метод делает пять разных вещей. Его нельзя протестировать по частям; одну ошибку сложно найти; команда конфликтует при правках.

### Как лучше

```csharp
void ProcessOrder(string orderId)
{
    if (!ValidateOrder(orderId)) return;
    decimal total = CalcTotal(orderId);
    if (!CheckStock(orderId)) return;
    PlaceOrder(orderId, total);
    NotifyCustomer(orderId);
}
```

### Вывод

Один метод — одно действие. Имя метода должно полностью описывать то, что он делает.

## Пример 2: Дублирование логики

### Исходный код

```csharp
void SaveUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Имя не должно быть пустым");
        return;
    }
    // сохраняем
}

void UpdateUser(string name)
{
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Имя не должно быть пустым");
        return;
    }
    // обновляем
}
```

### Что не так

- Проверка продублирована. Захочешь поменять сообщение или порог — придётся менять в двух местах (и забыть в одном).

### Как лучше

```csharp
bool IsValidName(string name) => !string.IsNullOrWhiteSpace(name);

void SaveUser(string name)
{
    if (!IsValidName(name)) { Console.WriteLine("Имя не должно быть пустым"); return; }
    // сохраняем
}

void UpdateUser(string name)
{
    if (!IsValidName(name)) { Console.WriteLine("Имя не должно быть пустым"); return; }
    // обновляем
}
```

### Вывод

Общее правило — в общий метод. Дублирование — источник рассинхронизации поведения.

## Пример 3: Отсутствие guard clauses

### Исходный код

```csharp
double Divide(double a, double b)
{
    if (b != 0)
    {
        return a / b;
    }
    else
    {
        throw new DivideByZeroException();
    }
}
```

### Что не так

- Основной путь спрятан внутри `if`, а обработка ошибки — в `else`. Читается тяжело.

### Как лучше

```csharp
double Divide(double a, double b)
{
    if (b == 0) throw new DivideByZeroException();
    return a / b;
}
```

### Вывод

Исключительные случаи обрабатывай первыми и выходи рано. Основная логика всегда остаётся плоской.

## Пример 4: `out`-параметр для понятного результата

### Исходный код

```csharp
void SplitFullName(string full, out string first, out string last)
{
    var parts = full.Split(' ', 2);
    first = parts.Length > 0 ? parts[0] : "";
    last = parts.Length > 1 ? parts[1] : "";
}

SplitFullName("Иван Иванов", out string f, out string l);
Console.WriteLine($"{f} / {l}");
```

### Что не так

- Кортеж читается яснее: метод возвращает одно значение, а не «протыкает дыры» через `out`. Плюс без `out` проще использовать в выражениях.

### Как лучше

```csharp
(string First, string Last) SplitFullName(string full)
{
    var parts = full.Split(' ', 2);
    return (parts.Length > 0 ? parts[0] : "", parts.Length > 1 ? parts[1] : "");
}

var (first, last) = SplitFullName("Иван Иванов");
Console.WriteLine($"{first} / {last}");
```

### Вывод

Предпочитай возврат кортежа `out`-параметрам — код становится декларативнее.

## Пример 5: Магические числа в теле

### Исходный код

```csharp
double BikePrice(int typeCode)
{
    if (typeCode == 1) return 25000;
    if (typeCode == 2) return 35000;
    return 15000;
}
```

### Что не так

- `25000`, `35000`, `15000` — непонятные цены. `1`, `2` — непонятные коды. Изменение цены заставляет искать по всему проекту.

### Как лучше

```csharp
const decimal KidsBikePrice = 15000m;
const decimal CityBikePrice = 25000m;
const decimal MountainBikePrice = 35000m;

decimal BikePrice(int typeCode) => typeCode switch
{
    1 => CityBikePrice,
    2 => MountainBikePrice,
    _ => KidsBikePrice
};
```

### Вывод

Именованные константы и `switch`-выражение делают назначение чисел явным.
