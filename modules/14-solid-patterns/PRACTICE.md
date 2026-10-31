# Практика — Модуль 14

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Рефакторинг God-класса

**Сложность:** ★★★
**Описание:** Разбей монолитный `OrderProcessor` на классы по SRP.

**Требования:**
- `OrderValidator`, `OrderCalculator`, `OrderRepository`, `EmailNotifier`.
- Каждый — одна ответственность.

## Задача 2: Стратегия оплаты

**Сложность:** ★★☆
**Описание:** `IPaymentProcessor` + три реализации + выбор через Factory.

**Требования:**
- `CreditCardProcessor`, `PayPalProcessor`, `BankTransferProcessor`.
- `PaymentProcessorFactory.Create(PaymentMethod)`.

## Задача 3: Observer-уведомления

**Сложность:** ★★☆
**Описание:** Система уведомлений о заказе на событиях.

**Требования:**
- Событие `OrderPlaced`.
- Логгер и почтовый подписчик.

## Задача 4: Ручной DI-контейнер

**Сложность:** ★★★
**Описание:** Мини-контейнер на `Dictionary<Type, object>`.

**Требования:**
- `Register<TInterface, TImpl>()`, `Resolve<TInterface>()`.
- Объясни, почему это прототип, а не фреймворк.

## Задача 5: Применение DIP

**Сложность:** ★★☆
**Описание:** `OrderService` с `new` внутри → конструкторная инъекция.

**Требования:**
- Введи `IOrderRepository` через конструктор.
- Напиши тест с фейк-репозиторием (модуль 13).

## Задача 6: OCP-рефакторинг

**Сложность:** ★★★
**Описание:** switch по 6 типам в 3 местах → виртуальные методы.

**Требования:**
- Базовый `abstract` класс с виртуальным методом.
- Новый тип — просто новый класс.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
class OrderValidator
{
    public bool IsValid(Order order) => !string.IsNullOrWhiteSpace(order.Id);
}

class OrderCalculator
{
    public decimal Calculate(Order order) => order.Items.Sum(i => i.Price);
}

class OrderRepository
{
    public void Save(Order order) { /* сохранение */ }
}

class EmailNotifier
{
    public void Send(Order order) { /* письмо */ }
}

// Использование:
var validator = new OrderValidator();
var calculator = new OrderCalculator();
var repo = new OrderRepository();
var mailer = new EmailNotifier();

if (validator.IsValid(order))
{
    decimal total = calculator.Calculate(order);
    repo.Save(order);
    mailer.Send(order);
}
```

**Пояснение:** Каждый класс отвечает за свою часть; изменения не пересекаются.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
interface IPaymentProcessor { void Pay(decimal amount); }

class CreditCardProcessor : IPaymentProcessor
{
    public void Pay(decimal amount) => Console.WriteLine($"Картой: {amount}");
}

class PayPalProcessor : IPaymentProcessor
{
    public void Pay(decimal amount) => Console.WriteLine($"PayPal: {amount}");
}

class BankTransferProcessor : IPaymentProcessor
{
    public void Pay(decimal amount) => Console.WriteLine($"Переводом: {amount}");
}

enum PaymentMethod { CreditCard, PayPal, BankTransfer }

static class PaymentProcessorFactory
{
    public static IPaymentProcessor Create(PaymentMethod method) => method switch
    {
        PaymentMethod.CreditCard => new CreditCardProcessor(),
        PaymentMethod.PayPal => new PayPalProcessor(),
        PaymentMethod.BankTransfer => new BankTransferProcessor(),
        _ => throw new ArgumentOutOfRangeException(nameof(method))
    };
}

var processor = PaymentProcessorFactory.Create(PaymentMethod.PayPal);
processor.Pay(1500);
```

**Пояснение:** Стратегия взаимозаменяема; Factory прячет создание.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
class OrderPublisher
{
    public event EventHandler<string>? OrderPlaced;

    public void Place(string id)
    {
        Console.WriteLine($"Заказ {id}");
        OrderPlaced?.Invoke(this, id);
    }
}

void LogOrder(object? s, string id) => Console.WriteLine($"[LOG] {id}");
void MailOrder(object? s, string id) => Console.WriteLine($"[MAIL] {id}");

var publisher = new OrderPublisher();
publisher.OrderPlaced += LogOrder;
publisher.OrderPlaced += MailOrder;

publisher.Place("A-1");
```

**Пояснение:** Наблюдатель реализуется через события.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
class SimpleContainer
{
    private readonly Dictionary<Type, Func<object>> _factories = new();

    public void Register<TInterface, TImpl>() where TImpl : TInterface, new()
    {
        _factories[typeof(TInterface)] = () => new TImpl();
    }

    public TInterface Resolve<TInterface>()
    {
        if (!_factories.TryGetValue(typeof(TInterface), out var factory))
            throw new InvalidOperationException($"Тип не зарегистрирован: {typeof(TInterface)}");

        return (TInterface)factory();
    }
}

var container = new SimpleContainer();
container.Register<IOrderRepository, JsonOrderRepository>();
var repo = container.Resolve<IOrderRepository>();
```

**Пояснение:** Это учебный пример; в реальности используй готовые контейнеры (Microsoft.Extensions.DependencyInjection).

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
interface IOrderRepository { void Save(Order order); }

class JsonOrderRepository : IOrderRepository
{
    public void Save(Order order) => Console.WriteLine("Сохранено в JSON");
}

class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;

    public void Process(Order order) => _repo.Save(order);
}

var service = new OrderService(new JsonOrderRepository());
service.Process(new Order());
```

**Пояснение:** Зависимость вводится через конструктор — можно подставить фейк для тестов.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
abstract class Document
{
    public abstract void Print();
}

class PdfDocument : Document
{
    public override void Print() => Console.WriteLine("Печать PDF");
}

class WordDocument : Document
{
    public override void Print() => Console.WriteLine("Печать Word");
}

var docs = new List<Document> { new PdfDocument(), new WordDocument() };
foreach (var d in docs) d.Print();
```

**Пояснение:** Новый тип — новый класс, без правок существующего кода (OCP).

</details>
