# Разбор кода — Модуль 13

> Формат: плохой тест → находим проблемы → улучшаем.

## Пример 1: Тест-«пустышка»

### Исходный код

```csharp
[Fact]
public void TestSomething()
{
    var x = 1 + 1;
    // нет Assert — тест «ничего не проверяет»
}
```

### Что не так

- Без утверждений тест бесполезен: он всегда «проходит», даже если код сломан.

### Как лучше

```csharp
[Fact]
public void Add_TwoNumbers_ReturnsSum()
{
    Assert.Equal(2, 1 + 1);
}
```

### Вывод

Каждый тест обязан заканчиваться осмысленным `Assert.*`.

## Пример 2: Хрупкий тест на время

### Исходный код

```csharp
[Fact]
public void IsExpired_BeforeNow_ReturnsTrue()
{
    var service = new OrderService();
    var order = new Order(DateTime.Now.AddSeconds(30)); // зависит от текущего момента
    Assert.False(service.IsExpired(order)); // может успеть истеть, если тест долгий
}
```

### Что не так

- Поведение зависит от реальных часов. Прогон «на стыке» секунд даёт плавающий результат.

### Как лучше

```csharp
var service = new OrderService(new FakeClock(new DateTime(2024, 1, 1)));
var order = new Order(new DateTime(2024, 1, 2));
Assert.False(service.IsExpired(order)); // всегда false
```

### Вывод

Зависимости от времени — через `IClock`/параметры.

## Пример 3: Один тест с множеством актов

### Исходный код

```csharp
[Fact]
public void BankAccount_FullFlow()
{
    var acc = new BankAccount();
    acc.Deposit(100);
    Assert.Equal(100m, acc.Balance);
    acc.Withdraw(30);
    Assert.Equal(70m, acc.Balance);
    acc.Withdraw(80); // исключение
}
```

### Что не так

- Если падает «Withdraw(80)», непонятно, какой именно участок сломан. Один тест проверяет слишком много.

### Как лучше

```csharp
[Fact]
public void Deposit_AddsToBalance()
{
    var acc = new BankAccount();
    acc.Deposit(100);
    Assert.Equal(100m, acc.Balance);
}

[Fact]
public void Withdraw_ReducesBalance()
{
    var acc = new BankAccount { /* Начальный баланс */ };
    acc.Withdraw(30);
    Assert.Equal(70m, acc.Balance);
}
```

### Вывод

Дроби на отдельные сценарии — так проще локализовать поломку.

## Пример 4: Тест-дубликат

### Исходный код

```csharp
[Fact] public void Add_1And2_Is3() => Assert.Equal(3, Arithmetic.Add(1, 2));
[Fact] public void Add_1And2_Is3Again() => Assert.Equal(3, Arithmetic.Add(1, 2));
```

### Что не так

- Дублирующийся тест замедляет прогон и не даёт нового покрытия.

### Как лучше

```csharp
[Theory]
[InlineData(1, 2, 3)]
[InlineData(-1, 1, 0)]
[InlineData(0, 0, 0)]
public void Add_TwoNumbers_ReturnsSum(int a, int b, int expected)
{
    Assert.Equal(expected, Arithmetic.Add(a, b));
}
```

### Вывод

`[Theory]` с `InlineData` покрывает несколько случаев одним тестом.

## Пример 5: Тест на фреймворк

### Исходный код

```csharp
[Fact]
public void Linq_Where_Filters()
{
    var result = new[] { 1, 2, 3 }.Where(n => n > 1).ToList();
    Assert.Equal(new[] { 2, 3 }, result);
}
```

### Что не так

- Проверяется поведение LINQ — библиотеки платформы, а не наш код. Такой тест ничего не защищает.

### Как лучше

Тестировать собственную бизнес-логику:

```csharp
[Fact]
public void FilterActiveUsers_ReturnsOnlyActive()
{
    var users = new[] { new User("Иван", true), new User("Мария", false) };
    var result = UserService.FilterActive(users);
    Assert.Single(result);
}
```

### Вывод

Тестируй свой код, а не инструменты.
