# Практика — Модуль 20

> Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Сервис с логированием

**Сложность:** ★★☆
**Описание:** Добавь `ILogger` в `OrderService` и логируй создание заказа.

**Требования:**
- Параметры `{OrderId}` в сообщении.
- Уровень `Information`.

## Задача 2: Обработка ошибок с логированием

**Сложность:** ★★☆
**Описание:** В `catch` логируй через `LogError(ex, ...)`.

**Требования:**
- Не проглатывать исключение.
- Лог содержит исключение.

## Задача 3: Метрика

**Сложность:** ★★★
**Описание:** Создай `Meter` и счётчик созданных заказов.

**Требования:**
- `CreateCounter<int>("orders.created")`.
- Наращивай при создании.

## Задача 4: Уровни

**Сложность:** ★☆☆
**Описание:** Разведи уровни: `Information` для обычного, `Warning` для подозрительного, `Error` для сбоев.

**Требования:**
- Примеры для каждого уровня.

## Задача 5: Трейс `Activity`

**Сложность:** ★★★
**Описание:** Оберни создание заказа в `Activity`.

**Требования:**
- `ActivitySource(name).StartActivity("CreateOrder")`.

## Задача 6: Структурированный лог списка

**Сложность:** ★★★
**Описание:** Залогируй количество заказов с параметром.

**Требования:**
- `LogInformation("Заказов: {Count}", orders.Count)`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger) => _logger = logger;

    public async Task<Order> CreateAsync(Order order)
    {
        _logger.LogInformation("Создание заказа {OrderId}", order.Id);
        // ...
        _logger.LogInformation("Заказ создан {OrderId}", order.Id);
        return order;
    }
}
```

**Пояснение:** Параметры в `{...}` позволяют фильтровать логи по полям.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
try
{
    // ...
}
catch (Exception ex)
{
    _logger.LogError(ex, "Не удалось создать заказ {OrderId}", order.Id);
    throw; // не глотаем
}
```

**Пояснение:** `LogError` принимает исключение — стек и причина попадают в лог.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
using System.Diagnostics.Metrics;

public static class ShopMetrics
{
    private static readonly Meter Meter = new("Shop.Orders", "1.0");
    public static Counter<int> OrdersCreated { get; } = Meter.CreateCounter<int>("orders.created");
}

// в сервисе:
ShopMetrics.OrdersCreated.Add(1);
```

**Пояснение:** Счётчик наращивается; коллектор (Prometheus) считывает метрику.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
_logger.LogInformation("Заказ {OrderId} принят", order.Id);
_logger.LogWarning("Необычно долгая обработка {OrderId}", order.Id);
_logger.LogError(ex, "Сбой при оплате {OrderId}", order.Id);
```

**Пояснение:** Верный уровень — читабельность и алертинг.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
public static class Telemetry
{
    public static readonly ActivitySource Source = new("Shop.Orders");
}

using var activity = Telemetry.Source.StartActivity("CreateOrder");
// внутри — полезная работа
activity?.SetTag("order.id", order.Id);
```

**Пояснение:** `Activity` описывает единицу работы для трейсинга.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
_logger.LogInformation("Всего заказов: {Count}", orders.Count);
```

**Пояснение:** Параметр числа — отдельное поле для агрегации.

</details>
