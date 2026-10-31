# Практика — Модуль 13

> Работаем в проекте `xunit-demo`. Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Тесты на валидацию

**Сложность:** ★☆☆
**Описание:** В `ArithmeticTests` добавь тест, что `Divide` бросает `ArgumentException` при... нет, ты уже знаешь: добавь тест на `Divide` с отрицательным делимым.

**Требования:**
- `Assert.Equal` с точным ожиданием.
- Запусти `dotnet test`.

## Задача 2: Граничные случаи

**Сложность:** ★★☆
**Описание:** Добавь тесты на `Multiply` с `decimal.MaxValue` и `decimal.MinValue`.

**Требования:**
- Обрати внимание на переполнение.
- `Assert.Throws<OverflowException>` при умножении, уходящем за диапазон.

## Задача 3: Theory-тест пароля

**Сложность:** ★★☆
**Описание:** Добавь `[Theory]` с `[InlineData]` для граничных случаев длины пароля.

**Требования:**
- 7 символов → `false`; 8 символов, если правила выполнены → `true`.

## Задача 4: Мок времени

**Сложность:** ★★★
**Описание:** Проверь `Order.IsExpired` с зависимостью от времени — подмени её интерфейсом `IClock`.

**Требования:**
- Реализуй фейк-класс `FakeClock` (вручную, без библиотек).
- Протестируй «до», «в» и «после» дедлайна.

**Подсказка:** В проекте нет мок-библиотеки, поэтому свой `FakeClock : IClock` — отличная демонстрация.

## Задача 5: Тест исключений

**Сложность:** ★★☆
**Описание:** Проверь, что `PasswordValidator.IsValid(null!)` бросает `ArgumentException`.

**Требования:**
- Используй `Assert.Throws<ArgumentException>`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
[Fact]
public void Divide_ByNegative_ReturnsQuotient()
{
    decimal result = Arithmetic.Divide(-9m, 3m);
    Assert.Equal(-3m, result);
}
```

**Пояснение:** Тест прямо описывает поведение; при изменении логики он зафиксирует отклонение.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
[Fact]
public void Multiply_MaxDecimal_ThrowsOverflow()
{
    Assert.Throws<OverflowException>(() => Arithmetic.Multiply(decimal.MaxValue, 2m));
}
```

**Пояснение:** В `checked`-режиме переполнение бросает `OverflowException`. Тест фиксирует это ожидание.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
[Theory]
[InlineData("Abcdef1")]    // 7 символов — слишком мало
[InlineData("Abcdef12")]   // 8 символов — ок
public void IsValid_LengthBoundary_MatchesRules(string password)
{
    Assert.Equal(password.Length >= 8, PasswordValidator.IsValid(password));
}
```

**Пояснение:** `Theory` с несколькими наборами данных экономит дубликаты.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
public interface IClock { DateTime Now { get; } }

public class FakeClock : IClock
{
    public DateTime Now { get; }
    public FakeClock(DateTime now) => Now = now;
}

public class Order
{
    public DateTime Deadline { get; }
    public Order(DateTime deadline) => Deadline = deadline;
}

public class OrderService
{
    private readonly IClock _clock;
    public OrderService(IClock clock) => _clock = clock;

    public bool IsExpired(Order order) => _clock.Now > order.Deadline;
}

[Fact]
public void IsExpired_BeforeDeadline_ReturnsFalse()
{
    var service = new OrderService(new FakeClock(new DateTime(2024, 1, 1)));
    var order = new Order(new DateTime(2024, 1, 10));

    Assert.False(service.IsExpired(order));
}
```

**Пояснение:** Изолировали время: тест не зависит от реального `DateTime.Now`.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
[Fact]
public void IsValid_Null_ThrowsArgumentException()
{
    Assert.Throws<ArgumentException>(() => PasswordValidator.IsValid(null!));
}
```

**Пояснение:** Утверждение проверяет именно тип исключения.

</details>
