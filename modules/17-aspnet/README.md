# Модуль 17: Введение в ASP.NET Core

> **Цель:** построить первое веб-API на ASP.NET Core и понять, как серверы обрабатывают HTTP-запросы.

## Зачем это нужно

Ты прошёл C# и Git. Теперь самое востребованное применение языка — **веб-разработка**: REST-API, бэкенд мобильных приложений, сервисы, микросервисы. ASP.NET Core — современный кроссплатформенный веб-фреймворк от Microsoft. Умение писать API — прямой путь к работе backend-разработчиком.

## Теория

### 1. Как работает веб

Клиент (браузер, мобильное приложение) отправляет **HTTP-запрос** серверу, сервер отвечает **HTTP-ответом**.

Ключевые вещи запроса:

| Понятие | Пример |
|---|---|
| Метод | `GET`, `POST`, `PUT`, `DELETE` |
| Путь (URL) | `/api/tasks` |
| Заголовки | `Content-Type: application/json` |
| Тело (body) | JSON-данные |

Семинатика методов: `GET` — получить, `POST` — создать, `PUT` — обновить целиком, `DELETE` — удалить.

Коды ответов: `200 OK`, `201 Created`, `400 Bad Request`, `404 Not Found`, `500 Server Error`.

### 2. Что такое ASP.NET Core

- **Кроссплатформенный** — работает на Windows, Linux, macOS.
- **Встроенный веб-сервер** — Kestrel.
- **Minimal API** и **Controllers** — два стиля написания API.
- Мощный встроенный контейнер **DI**.

### 3. Minimal API

Самый простой способ:

```csharp
var app = WebApplication.CreateBuilder(args).Build();

app.MapGet("/hello", () => "Привет, мир!");
app.MapPost("/sum", (SumRequest r) => r.A + r.B);

app.Run();
```

Каждый `app.Map...` — обработчик маршрута. Параметры приходят из query, пути или тела запроса.

### 4. Структура типичного проекта

```
src/TaskApi/
├── Program.cs          # конфигурация, DI, маршруты
├── Models/             # DTO и модели
├── Services/           # бизнес-логика
├── Storage/            # доступ к данным (репозиторий)
└── appsettings.json     # конфигурация
```

### 5. Dependency Injection

В ASP.NET Core DI встроен:

```csharp
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<TaskService>();
```

Контейнер сам создаёт зависимости и внедряет их в обработчики (параметры методов) и конструкторы. Это тот самый DIP из модуля 14 — только теперь автоматически.

### 6. Middleware и конвейер

Запрос проходит через цепочку middleware. По умолчанию в minimal API уже есть маршрутизация.

```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine($"Запрос: {context.Request.Method} {context.Request.Path}");
    await next();
});
```

### 7. Сериализация JSON

ASP.NET Core по умолчанию работает с System.Text.Json: объекты сами превращаются в JSON и обратно. Имена свойств — `camelCase`.

### 8. Запуск и тестирование

```bash
dotnet run --project src/TaskApi
# GET http://localhost:5000/api/tasks
```

Для тестов API есть `WebApplicationFactory<Program>`, который поднимает сервер в памяти (см. `minimal-api-demo/tests`).

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| Вся логика в `Program.cs` | God-метод, нетестируемо | вынести в сервисы |
| `new` сервисов внутри обработчиков | нельзя подменить для тестов | DI |
| Возврат всего `OK` | не информативно для клиента | корректные коды (`201`, `400`) |
| Синхронные вызовы в async | блокирует поток | `async`/`await` всюду |
| Не использовать `CancellationToken` | отмену нельзя пробросить | принимать и передавать дальше |

## Контрольные вопросы

1. Чем `GET` отличается от `POST`?
2. Что такое Minimal API?
3. Зачем DI в ASP.NET Core?
4. Что такое middleware?
5. Как протестировать API без реального запуска сервера?
6. Что означает статус-код 201?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
- Рабочий пример: [`minimal-api-demo`](minimal-api-demo)
