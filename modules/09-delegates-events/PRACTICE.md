# Практика — Модуль 09

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Калькулятор на `Func`

**Сложность:** ★☆☆
**Описание:** Принимай операцию как `Func<double, double, double>`.

**Требования:**
- `double Apply(double a, double b, Func<double, double, double> op)`.
- Вызови для `+`, `*`, `%`.

## Задача 2: Уведомления о заказе

**Сложность:** ★★☆
**Описание:** Класс `OrderService` публикует `OrderPlaced`.

**Требования:**
- Событие `EventHandler<string>` с ID заказа.
- Два подписчика: логгер и отправщик почты.
- Проверь отписку.

## Задача 3: Кастомный делегат

**Сложность:** ★★☆
**Описание:** Объяви `delegate double Operation(double a, double b);` и используй вместо `Func`.

**Требования:**
- Метод `double Calc(double a, double b, Operation op)`.
- Передай `Add`, `Multiply`, лямбду.

## Задача 4: Счётчик вызовов

**Сложность:** ★★☆
**Описание:** Лямбда-замыкание, которое накапливает вызовы.

**Требования:**
- `Func<int>` возрастающий счётчик.
- Вызови 5 раз, выведи последнее значение.

## Задача 5: Сортировка объектов

**Сложность:** ★★☆
**Описание:** Список `Person` (имя, возраст); сортируй разными способами.

**Требования:**
- Сортировка по имени, по возрасту, по возрасту по убыванию.
- Сравни с `IComparable` из модуля 05.

## Задача 6: Защита от утечки подписки

**Сложность:** ★★★
**Описание:** Класс-подписчик, который корректно отписывается в `Dispose()`.

**Требования:**
- Каждый `Subscribe` создаёт объект, который подписывается.
- При `Dispose()` — `-=`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
double Apply(double a, double b, Func<double, double, double> op) => op(a, b);

Console.WriteLine(Apply(5, 3, (x, y) => x + y));   // 8
Console.WriteLine(Apply(5, 3, (x, y) => x * y));   // 15
Console.WriteLine(Apply(5, 3, (x, y) => x % y));   // 2
```

**Пояснение:** Лямбда как операция — тот самый «поведенческий» параметр.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
class OrderService
{
    public event EventHandler<string>? OrderPlaced;

    public void PlaceOrder(string id)
    {
        Console.WriteLine($"Заказ {id} создан");
        OrderPlaced?.Invoke(this, id);
    }
}

void LogOrder(object? sender, string id) => Console.WriteLine($"[LOG] Заказ {id}");
void MailOrder(object? sender, string id) => Console.WriteLine($"[MAIL] Уведомление по {id}");

var service = new OrderService();
service.OrderPlaced += LogOrder;
service.OrderPlaced += MailOrder;

service.PlaceOrder("A-1"); // оба обработчика сработают

service.OrderPlaced -= MailOrder; // отписка
service.PlaceOrder("B-2");        // только логгер
```

**Пояснение:** `EventHandler<string>` — стандартная форма, где второй параметр вместо `EventArgs` — данные события.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
delegate double Operation(double a, double b);

static double Add(double a, double b) => a + b;
static double Multiply(double a, double b) => a * b;

double Calc(double a, double b, Operation op) => op(a, b);

Console.WriteLine(Calc(2, 3, Add));                  // 5
Console.WriteLine(Calc(2, 3, Multiply));              // 6
Console.WriteLine(Calc(2, 3, (a, b) => a - b));       // -1 (лямбдa)
```

**Пояснение:** Свой делегат полезен, когда нужны читаемость и специфичное имя.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
int counter = 0;
Func<int> increment = () => ++counter;

increment();
increment();
increment();
Console.WriteLine(increment()); // 4 — замыкание помнит counter
```

**Пояснение:** Лямбда захватила `counter` по ссылке — значение сохраняется между вызовами.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
class Person
{
    public string Name { get; }
    public int Age { get; }
    public Person(string name, int age) { Name = name; Age = age; }
}

var people = new List<Person>
{
    new("Мария", 28),
    new("Иван", 30),
    new("Анна", 20)
};

people.Sort((a, b) => a.Name.CompareTo(b.Name));   // по имени
people.Sort((a, b) => a.Age.CompareTo(b.Age));      // по возрасту
people.Sort((a, b) => b.Age.CompareTo(a.Age));       // по убыванию

foreach (var p in people) Console.WriteLine($"{p.Name}: {p.Age}");
```

**Пояснение:** Лямбда-компаратор гибче `IComparable`: сортировок может быть сколько угодно.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
class EventEmitter
{
    public event EventHandler? Fired;
    public void Fire() => Fired?.Invoke(this, EventArgs.Empty);
}

class Listener : IDisposable
{
    private readonly EventEmitter _emitter;

    public Listener(EventEmitter emitter)
    {
        _emitter = emitter;
        _emitter.Fired += OnFired;
    }

    private void OnFired(object? sender, EventArgs e) => Console.WriteLine("Получено!");

    public void Dispose() => _emitter.Fired -= OnFired;
}

var emitter = new EventEmitter();

using (var listener = new Listener(emitter))
{
    emitter.Fire(); // "Получено!"
} // Dispose() отписывает

emitter.Fire(); // никакого вывода — подписки нет
```

**Пояснение:** Если подписчик живёт дольше источника, отписка обязательна — иначе память держится (утечка).

</details>
