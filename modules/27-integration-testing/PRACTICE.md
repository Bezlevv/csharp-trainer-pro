# Практика — Модуль 27: Интеграционные и E2E-тесты

## Задача 1: WebApplicationFactory

**Сложность:** ★★★
**Описание:** Напиши интеграционный тест: GET /api/tasks → 200.

**Требования:**
- Пакет `Microsoft.AspNetCore.Mvc.Testing`.

## Задача 2: In-memory БД

**Сложность:** ★★☆
**Описание:** Тест для EF-контекста на in-memory БД.

**Требования:**
- `UseInMemoryDatabase`.

## Задача 3: Smoke-набор

**Сложность:** ★★☆
**Описание:** Пометь «смоук»-тесты атрибутом Trait.

**Требования:**
- Запуск: `dotnet test --filter Category=Smoke`.

## Задача 4: Проверка POST

**Сложность:** ★★★
**Описание:** Интеграционный тест: POST /api/tasks создаёт задачу (201).

**Требования:**
- `PostAsJsonAsync`.

## Задача 5: Пирамида на своём проекте

**Сложность:** ★★☆
**Описание:** Для модуля 15 (TaskTracker) опиши, какие тесты куда отнести.

**Требования:**
- Юнит: TaskService. Интеграционные: хранилище. E2E: не обязательно.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
[Fact]
public async Task Get_Returns200()
{
    using var factory = new WebApplicationFactory<Program>();
    using var client = factory.CreateClient();
    var res = await client.GetAsync("/api/tasks");
    Assert.Equal(HttpStatusCode.OK, res.StatusCode);
}
```
</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
var options = new DbContextOptionsBuilder<AppDbContext>()
    .UseInMemoryDatabase(Guid.NewGuid().ToString())
    .Options;
await using var db = new AppDbContext(options);
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
[Trait("Category", "Smoke")]
[Fact]
public void Build_Smoke() { /* быстрая проверка */ }
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
var res = await client.PostAsJsonAsync("/api/tasks", new { title = "Тест" });
Assert.Equal(HttpStatusCode.Created, res.StatusCode);
```
</details>

<details>
<summary>Задача 5 — решение</summary>

- Юнит: `TaskService` (валидация, создание, статусы).
- Интеграционные: `JsonTaskRepository` (файл).
- E2E: не обязателен для консольного приложения.
</details>
