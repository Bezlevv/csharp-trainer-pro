# Модуль 27: Интеграционное и E2E-тестирование

> **Цель:** понять иерархию тестов и как тестировать взаимодействие с БД/API/браузером.

## Зачем это нужно
Юнит-тесты проверяют «кирпичики». Но целое тоже должно работать: интеграционные тесты проверяют связку (БД + API), E2E — полный сценарий пользователя. Это следующий уровень надёжности.

## Теория

### 1. Пирамида тестов
```
       /\  E2E (мало, дорого)
      /  \
     /    \  Интеграционные (средне)
    /      \
   /________\  Юнит (много, быстро)
```

- Юнит: изолированно, быстро.
- Интеграционные: БД, API, очереди — несколько компонентов вместе.
- E2E: браузер/пользовательский сценарий.

### 2. Интеграционные тесты API (WebApplicationFactory)
Проверяют настоящий запрос через весь конвейер ASP.NET Core:
```csharp
public class ApiTests
{
    [Fact]
    public async Task Get_Returns200()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var res = await client.GetAsync("/api/tasks");
        Assert.Equal(System.Net.HttpStatusCode.OK, res.StatusCode);
    }
}
```

### 3. Интеграционные тесты с БД
Вместо реальной БД — **in-memory** (EF Core) или Testcontainers (реальная БД в контейнере):
```csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .Options;
```

### 4. E2E (браузер): Playwright
Полный пользовательский сценарий:
```csharp
using var page = await browser.NewPageAsync();
await page.GotoAsync("http://localhost:5000");
await page.ClickAsync("#submit");
await page.WaitForSelectorAsync("#result");
```

### 5. Smoke-тесты
Быстрая проверка «собирается и живо»: `dotnet test --filter Category=Smoke`.

### 6. Что покрывать
- Главные сценарии пользователя.
- Критические потоки (оплата, заказ).
- Граничные случаи бизнес-логики.

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| E2E на всё подряд | медленно и хрупко | только ключевые сценарии |
| Интеграционные на реальную БД без изоляции | зависят от окружения | in-memory / Testcontainers |
| Юнит-тест лезет в сеть/БД | медленно, нестабильно | моки |
| Нет smoke-тестов | долгий фидбек | быстрый smoke-набор |

## Контрольные вопросы
1. Что такое пирамида тестов?
2. Что проверяют интеграционные тесты?
3. Зачем WebApplicationFactory?
4. Чем E2E отличается от юнит?
5. Что такое smoke-тесты?

## Дальше
- [Практика](PRACTICE.md)
- [Разбор кода](CODE_REVIEW.md)
