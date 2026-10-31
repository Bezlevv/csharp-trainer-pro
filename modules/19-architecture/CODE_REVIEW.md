# Разбор кода — Модуль 19

> Формат: плохая архитектура → находим проблемы → рефакторим.

## Пример 1: Домен знает про EF Core

### Исходный код

```csharp
// Domain/Entities/Order.cs
public class Order
{
    public void Save() => new AppDbContext().SaveChanges(); // зависимость от EF!
}
```

### Что не так

- Домен связан с инфраструктурой. Смена БД ломает домен; тесты невозможны без EF.

### Как лучше

Домен держит только бизнес-правила. Сохранение — через репозиторий в Infrastructure.

### Вывод

Домен не должен знать про БД.

## Пример 2: Сервис-«каша» без границ

### Исходный код

```csharp
public class OrderService
{
    // и создание, и оплата, и отчёт, и email — всё здесь
}
```

### Что не так

- Класс разрастается, нарушения SRP, сложно поддерживать.

### Как лучше

Разбить на `OrderCreationService`, `OrderPaymentService`, `OrderReportService`, `NotificationService`.

### Вывод

Use cases — отдельными классами.

## Пример 3: Нет интерфейсов на границах

### Исходный код

```csharp
public class OrderService
{
    private readonly EfOrderRepository _repo = new();
}
```

### Что не так

- Жёсткая связь с конкретной реализацией. Тесты требуют настоящую БД.

### Как лучше

```csharp
public class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}
```

### Вывод

Контракты на границах слоёв обязательны.

## Пример 4: Вся логика в инфраструктуре

### Исходный код

```csharp
public class AppDbContext : DbContext
{
    // сюда добавили и расчёты скидок, и валидацию
}
```

### Что не так

- Контекст — это не место для бизнес-логики.

### Как лучше

Валидация и расчёты — в Application/Domain; контекст отвечает за доступ к данным.

### Вывод

Инфраструктура хранит данные, а не правила.

## Пример 5: Прямые ссылки между сервисами

### Исходный код

```csharp
public class CheckoutService
{
    private readonly PaymentService _payment = new();
    private readonly InventoryService _inventory = new();
}
```

### Что не так

- Сервисы жёстко связаны; переиспользование и тестирование затруднены.

### Как лучше

Инъекция через конструктор + интерфейсы. При большом числе зависимостей — MediatR/Mediator.

### Вывод

Зависимости — через DI и контракты.
