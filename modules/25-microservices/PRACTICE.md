# Практика — Модуль 25: Микросервисы

## Задача 1: Спроектируй систему

**Сложность:** ★★☆
**Описание:** Опиши разбиение магазина на микросервисы.

**Требования:**
- Сервисы: заказы, оплата, склад, уведомления.
- Кто с кем общается и как.

## Задача 2: События

**Сложность:** ★★☆
**Описание:** Перечисли события для магазина.

**Требования:**
- `OrderCreated`, `PaymentCompleted`, `OrderShipped` и т.д.

## Задача 3: Публикатор/подписчик на уровне кода

**Сложность:** ★★★
**Описание:** Смоделируй событие `OrderCreated` через event (как в уроке 46).

**Требования:**
- Класс `OrderService` публикует; `PaymentService`/`NotificationService` подписываются.

## Задача 4: Модель очереди

**Сложность:** ★★★
**Описание:** Опиши концептуально очередь (без реального RabbitMQ): добавление, чтение.

**Требования:**
- Очередь `Queue<string>` как модель буфера.

## Задача 5: Идемпотентность

**Сложность:** ★★★
**Описание:** Объясни и приведи пример идемпотентного обработчика.

**Требования:**
- Повторное событие не должно удваивать эффект (например, проверка по id).

## Решения

<details>
<summary>Задача 1 — решение</summary>

```
Заказы (OrderService) → события → Оплата (PaymentService), Склад (InventoryService), Уведомления (NotificationService)
```
Каждый сервис слушает свои события. Связь — через очередь.
</details>

<details>
<summary>Задача 2 — решение</summary>

`OrderCreated`, `PaymentCompleted`, `PaymentFailed`, `OrderShipped`, `OrderCancelled`.
</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
class OrderService
{
    public event EventHandler<string>? OrderCreated;
    public void Create(string id)
    {
        Console.WriteLine($"Заказ {id} создан");
        OrderCreated?.Invoke(this, id);
    }
}

var orders = new OrderService();
orders.OrderCreated += (s, id) => Console.WriteLine($"[Оплата] заказ {id}");
orders.OrderCreated += (s, id) => Console.WriteLine($"[Уведомление] заказ {id}");
orders.Create("A-1");
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
var queue = new Queue<string>();
queue.Enqueue("OrderCreated");   // producer
var msg = queue.Dequeue();       // consumer
```
Модель: producer кладёт, consumer забирает.
</details>

<details>
<summary>Задача 5 — решение</summary>

Идемпотентность — повторная обработка даёт тот же результат. Пример: перед обработкой `OrderCreated` проверяем, обработан ли уже id (по сохранённому ключу). Если да — пропускаем.
</details>
