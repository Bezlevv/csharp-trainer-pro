# Разбор кода — Модуль 14

> Формат: плохой код → находим проблемы → рефакторим.

## Пример 1: God-класс Program.cs

### Исходный код

```csharp
// Program.cs — 500 строк: меню, расчёты, ввод, сохранение, валидация
Console.WriteLine("Меню: 1 — добавить, 2 — показать");
// ... вся логика прям во входной точке
```

### Что не так

- Всё в одном месте: менять сложно, тестировать невозможно, конфликты неизбежны.

### Как лучше

```csharp
// ConsoleUI — ввод/вывод
// OrderService — логика
// OrderRepository — хранение
// Program.cs — только связка
```

### Вывод

Входная точка — тонкая; логика — в классах с ответственностью.

## Пример 2: Статический «глобальный» сервис

### Исходный код

```csharp
static class UserService
{
    public static void Save(User user) { /* new UserRepository() внутри */ }
}
```

### Что не так

- Скрытая зависимость от `UserRepository`: её нельзя подменить в тестах, код жёстко связан.

### Как лучше

```csharp
class UserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;
    public void Save(User user) => _repo.Save(user);
}
```

### Вывод

Зависимости — через конструктор (DIP).

## Пример 3: switch по enum в 5 местах

### Исходный код

```csharp
// в пяти методах повторяется:
switch (order.Status)
{
    case OrderStatus.New: ...
    case OrderStatus.Paid: ...
}
```

### Что не так

- Новый статус — правки во всех пяти местах. Нарушен OCP.

### Как лучше

```csharp
abstract class OrderState
{
    public abstract string Describe();
}

class NewState : OrderState
{
    public override string Describe() => "Новый";
}
```

### Вывод

Распределяй поведение по полиморфным классам.

## Пример 4: Интерфейс-«мусорка»

### Исходный код

```csharp
interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void PrintReport();
}
```

### Что не так

- Каждый клиент обязан реализовать всё, даже ненужное. Меняется один метод — страдают все.

### Как лучше

```csharp
interface IWorker { void Work(); }
interface IReportable { void PrintReport(); }
```

### Вывод

Тонкие интерфейсы по ролям (ISP).

## Пример 5: `new` внутри сервиса

### Исходный код

```csharp
class OrderService
{
    private readonly SqlOrderRepository _repo = new(); // жёсткая связь
}
```

### Что не так

- Нельзя подменить репозиторий (фейк для тестов), класс не переиспользуется.

### Как лучше

```csharp
class OrderService
{
    private readonly IOrderRepository _repo;
    public OrderService(IOrderRepository repo) => _repo = repo;
}
```

### Вывод

Конструкторная инъекция — основа тестируемости.
