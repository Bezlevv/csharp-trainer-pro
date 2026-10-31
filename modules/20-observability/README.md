# Модуль 20: Наблюдаемость — логирование, метрики, трейсинг

> **Цель:** проектировать сервисы так, чтобы их было видно и можно было диагностировать в проде.

## Зачем это нужно

В разработке код «работает», а в проде — «лежит». Наблюдаемость (observability) — способность понять, что происходит в системе: **логи** (что случилось), **метрики** (как быстро/много), **трейсы** (где потрачено время). Это отличает senior: он пишет код, который можно расследовать.

## Теория

### 1. Три столпа наблюдаемости

| Столп | Вопрос | Инструмент в .NET |
|---|---|---|
| **Логи** | что произошло? | `ILogger<T>` |
| **Метрики** | насколько загружена система? | `Meter`, OpenTelemetry |
| **Трейсы** | где время в запросе? | `Activity`, OpenTelemetry (Tracing) |

### 2. Логирование: `ILogger<T>`

В ASP.NET Core логирование встроено через DI:

```csharp
public class OrderService
{
    private readonly ILogger<OrderService> _logger;
    public OrderService(ILogger<OrderService> logger) => _logger = logger;

    public async Task CreateAsync(Order order)
    {
        _logger.LogInformation("Создание заказа {OrderId}", order.Id);
        // ...
        _logger.LogInformation("Заказ создан {OrderId} за {ElapsedMs} мс", order.Id, 12);
    }
}
```

**Структурированное логирование** — сообщение с параметрами, а не конкатенация:

```csharp
// ПЛОХО:
_logger.LogInformation($"Создан заказ {order.Id}");
// ХОРОШО (параметры отдельно):
_logger.LogInformation("Создан заказ {OrderId}", order.Id);
```

Уровни: `Trace`, `Debug`, `Information`, `Warning`, `Error`, `Critical`.

### 3. Метрики

`System.Diagnostics.Metrics.Meter`:

```csharp
var meter = new Meter("Shop.Orders", "1.0");
var ordersCreated = meter.CreateCounter<int>("orders.created");
// ...
ordersCreated.Add(1);
```

Метрики собираются и экспортируются (Prometheus и т.п.).

### 4. Трейсинг: `Activity` и OpenTelemetry

`Activity` — единица работы (например, HTTP-запрос). OpenTelemetry связывает логи, метрики и трейсы, отправляя их в коллектор.

```csharp
using var activity = new ActivitySource("Shop.Orders").StartActivity("CreateOrder");
// внутри activity можно вешать теги/span-ы
```

### 5. Структурированные логи — зачем

Поиск и фильтрация по полям, агрегация, трассировка запроса по `TraceId`.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Конкатенация в логах | неструктурированно, не фильтруется | параметры `{Name}` |
| `LogInformation` для всего | шум, засоряет | выбирать уровень |
| Логировать секреты | утечка | никогда не логировать пароли/токены |
| Не логировать ошибки в catch | тишина при сбое | `LogError(ex, ...)` |

## Контрольные вопросы

1. Что такое три столпа наблюдаемости?
2. Зачем параметры в сообщениях лога?
3. Что такое метрики?
4. Что такое трейс?
5. Какие бывают уровни логирования?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки
