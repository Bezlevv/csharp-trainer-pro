# Практика — Модуль 21

> Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Хэширование пароля

**Сложность:** ★★☆
**Описание:** Используй `PasswordHasher<T>` (или самописный хэш) для хранения пароля.

**Требования:**
- Не хранить пароль в открытом виде.
- Сравнивать только хэши.

## Задача 2: JWT-настройка

**Сложность:** ★★★
**Описание:** Подключи аутентификацию JWT в ASP.NET Core.

**Требования:**
- `AddAuthentication(...) + AddJwtBearer(...)`.
- `TokenValidationParameters` с issuer/audience/ключом.

## Задача 3: Ролевая авторизация

**Сложность:** ★★★
**Описание:** Защити эндпоинт только для роли `Admin`.

**Требования:**
- `RequireAuthorization("Admin")`.

## Задача 4: Секреты через env

**Сложность:** ★★☆
**Описание:** Вынеси строку подключения и ключ в переменные окружения.

**Требования:**
- `Environment.GetEnvironmentVariable("DB_CONNECTION")`.

## Задача 5: Валидация входных данных

**Сложность:** ★★☆
**Описание:** Добавь валидацию модели запроса.

**Требования:**
- Проверка длины/формата.
- Не доверять клиенту.

## Задача 6: Запрет сырых SQL

**Сложность:** ★★★
**Описание:** Перепиши сырой SQL на EF Core/параметризованные запросы.

**Требования:**
- Никакой конкатенации в запрос.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
using Microsoft.AspNetCore.Identity;

var hasher = new PasswordHasher<object>();
string hash = hasher.HashPassword(null!, "секрет123");

bool ok = hasher.VerifyHashedPassword(null!, hash, "секрет123") != PasswordVerificationResult.Failed;
```

**Пояснение:** Хэшируем, а не храним пароль. Пароль сравнивается через `VerifyHashedPassword`.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var keyBytes = System.Text.Encoding.UTF8.GetBytes(secret); // secret — из env

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "myapp",
            ValidateAudience = true,
            ValidAudience = "myapp",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };
    });
```

**Пояснение:** JWT проверяется через симметричный ключ. Секрет — из окружения.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
app.MapGet("/admin/panel", () => "Панель")
   .RequireAuthorization("Admin");
```

**Пояснение:** `RequireAuthorization` включает проверку политики/роли.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
string? connection = Environment.GetEnvironmentVariable("DB_CONNECTION");
string? jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET");
```

**Пояснение:** Секреты — вне репозитория, в окружении.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
public record CreateOrderRequest(string Title, decimal Amount);

// валидация на границе:
if (string.IsNullOrWhiteSpace(request.Title) || request.Title.Length > 100)
    return Results.BadRequest("Некорректное название");
```

**Пояснение:** Проверяй всё, что приходит извне.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
// ПЛОХО (инъекция):
// var sql = $"SELECT * FROM Products WHERE Name = '{name}'";

// ХОРОШО — EF Core:
var products = await db.Products.Where(p => p.Name == name).ToListAsync();
```

**Пояснение:** EF Core параметризует запросы — инъекция невозможна.

</details>
