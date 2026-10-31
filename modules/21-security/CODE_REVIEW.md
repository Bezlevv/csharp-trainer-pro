# Разбор кода — Модуль 21

> Формат: небезопасный код → находим проблемы → рефакторим.

## Пример 1: SQL-инъекция

### Исходный код

```csharp
var sql = $"SELECT * FROM Users WHERE Name = '{name}'";
```

### Что не так

- Если `name` пришёл от пользователя, внедрение `' OR '1'='1` опустошит таблицу.

### Как лучше

```csharp
var users = await db.Users.Where(u => u.Name == name).ToListAsync();
```

### Вывод

Параметризация — обязательна.

## Пример 2: Пароль в открытом виде

### Исходный код

```csharp
db.Users.Add(new User { Password = rawPassword });
```

### Что не так

- При утечке БД крадутся пароли в открытом виде.

### Как лучше

```csharp
var hasher = new PasswordHasher<object>();
db.Users.Add(new User { PasswordHash = hasher.HashPassword(null!, rawPassword) });
```

### Вывод

Храни хэш, не пароль.

## Пример 3: Открытый эндпоинт

### Исходный код

```csharp
app.MapGet("/private/data", handler); // без RequireAuthorization
```

### Что не так

- Доступ без проверки личности.

### Как лучше

```csharp
app.MapGet("/private/data", handler).RequireAuthorization();
```

### Вывод

Защищай приватные маршруты.

## Пример 4: Секреты в коде

### Исходный код

```csharp
var secret = "my-super-secret-123"; // в коде!
```

### Что не так

- Секрет попадёт в репозиторий.

### Как лучше

```csharp
var secret = Environment.GetEnvironmentVariable("JWT_SECRET") ??
    throw new InvalidOperationException("JWT_SECRET is not set");
```

### Вывод

Секреты — только из окружения/хранилища.

## Пример 5: Доверие входным данным

### Исходный код

```csharp
app.MapPost("/upload", (string input) => Process(input));
// input идёт прямо в обработку без проверки
```

### Что не так

- Неконтролируемый ввод — путь к XSS, инъекциям и переполнению.

### Как лучше

Валидируй формат, длину, тип на границе API.

### Вывод

Любой ввод — потенциально опасен.
