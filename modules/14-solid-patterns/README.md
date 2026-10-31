# Модуль 14: SOLID и паттерны проектирования

> **Цель:** проектировать код, который легко расширять и поддерживать; знать основные паттерны.

## Зачем это нужно

Код пишется один раз, а читается — много раз. SOLID — пять принципов, которые делают код **устойчивым к изменениям**: ты добавляешь новую функциональность, не ломая существующую. Паттерны — проверенные решения повторяющихся задач. Это — уровень middle-разработчика.

## Теория

### 1. SOLID

#### S — Single Responsibility (единая ответственность)

Класс должен иметь только одну причину для изменения.

```csharp
// ПЛОХО: один класс делает всё
class OrderProcessor
{
    public void Process(Order order) { /* расчёт, БД, email */ }
}

// ХОРОШО: раздельные обязанности
class OrderCalculator { public decimal Calculate(Order order) => ...; }
class OrderRepository { public void Save(Order order) => ...; }
class EmailNotifier { public void Send(Order order) => ...; }
```

#### O — Open/Closed (открытость для расширения)

Классы открыты для расширения, но закрыты для модификации.

```csharp
// ПЛОХО: switch по типу — каждый новый тип требует правки
double Area(Shape shape) => shape switch
{
    Circle c => Math.PI * c.Radius * c.Radius,
    Rectangle r => r.Width * r.Height,
    // новый тип → правка здесь
};

// ХОРОШО: расширение через полиморфизм
abstract class Shape { public abstract double Area(); }
class Circle : Shape { public override double Area() => ...; }
```

#### L — Liskov (принцип Лисков)

Наследники не должны ломать поведение базового класса. Если переопределённый метод бросает исключение там, где базовый «всегда работает» — иерархия плохая.

#### I — Interface Segregation (разделение интерфейсов)

Клиенты не должны зависеть от методов, которые не используют. Большие интерфейсы — дробить.

```csharp
// ПЛОХО: «жирный» интерфейс
interface IWorker { void Work(); void Eat(); void Sleep(); }

// ХОРОШО
interface IWorker { void Work(); }
interface IReportable { void PrintReport(); }
```

#### D — Dependency Inversion (инверсия зависимостей)

Высокоуровневые модули не должны зависеть от низкоуровневых — и те, и другие зависят от абстракций.

```csharp
// ПЛОХО: жёсткая зависимость
class OrderService
{
    private readonly SqlOrderRepository _repo = new(); // new внутри
}

// ХОРОШО: конструкторная инъекция абстракции
class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}
```

### 2. Паттерны

**Factory** — централизованное создание объектов:

```csharp
interface IPaymentProcessor { void Pay(decimal amount); }

class PaymentProcessorFactory
{
    public static IPaymentProcessor Create(PaymentMethod method) => method switch
    {
        PaymentMethod.CreditCard => new CreditCardProcessor(),
        PaymentMethod.PayPal => new PayPalProcessor(),
        _ => throw new ArgumentOutOfRangeException(nameof(method))
    };
}
```

**Strategy** — взаимозаменяемые алгоритмы за интерфейсом (как `IPaymentProcessor` выше).

**Observer** — через события (модуль 09): издатель публикует, подписчики реагируют.

**Singleton** — один экземпляр:

```csharp
sealed class Logger
{
    public static Logger Instance { get; } = new();
    private Logger() { }
    public void Log(string msg) => Console.WriteLine(msg);
}
```

Осторожно: синглтон скрывает зависимости и мешает тестам.

**Dependency Injection** — внедрение зависимостей (вручную):

```csharp
var repo = new JsonOrderRepository();
var service = new OrderService(repo);
var ui = new ConsoleUI(service);
```

### 3. Антипаттерны

- **God Class** — класс, которому «всё можно».
- **Magic Numbers** — числа без смысла.
- **Spaghetti Code** — запутанные переходы.
- **Golden Hammer** — «молоток» для всех проблем.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Всё в `Program` | не тестируется, не понятно | слои/классы |
| Статический «глобальный» сервис | скрытая зависимость | DI/конструктор |
| switch по enum в 5 местах | нарушает OCP | полиморфизм/стратегия |
| Интерфейс-«мусорка» | сложные реализации | ISP |

## Контрольные вопросы

1. Что такое единая ответственность?
2. В чём суть принципа Open/Closed?
3. Что такое инверсия зависимостей?
4. Что даёт паттерн Factory?
5. Когда нужен Strategy?
6. Почему синглтон контроверсен?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
