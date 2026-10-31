# Практика — Модуль 19

> Сначала попробуй решить сам. Решения — в конце файла.

## Задача 1: Карта проекта

**Сложность:** ★☆☆
**Описание:** Спроектируй структуру папок для Web API «Магазин».

**Требования:**
- Слои: Domain, Application, Infrastructure, Presentation.
- Опиши, что лежит в каждом.

## Задача 2: Интерфейс репозитория

**Сложность:** ★★☆
**Описание:** Объяви `IProductRepository` в Application.

**Требования:**
- `GetByIdAsync`, `GetAllAsync`, `AddAsync`.
- Без зависимостей от EF.

## Задача 3: Реализация репозитория

**Сложность:** ★★☆
**Описание:** Реализуй `EfProductRepository` на EF Core в Infrastructure.

**Требования:**
- Наследует `IProductRepository`.
- Использует `AppDbContext`.

## Задача 4: Сервис-применение

**Сложность:** ★★☆
**Описание:** `ProductService` (Application) использует `IProductRepository`.

**Требования:**
- Конструкторная инъекция.
- Бизнес-логика (например, проверка цены).

## Задача 5: Регистрация в DI

**Сложность:** ★★☆
**Описание:** Зарегистрируй интерфейс и реализацию.

**Требования:**
- `AddScoped<IProductRepository, EfProductRepository>()`.

## Задача 6: Тест с фейком

**Сложность:** ★★★
**Описание:** Протестируй `ProductService`, подменив `IProductRepository` фейком.

**Требования:**
- Фейк-класс в тестах.
- Проверь бизнес-логику.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```
src/Shop/
├── Domain/
│   ├── Entities/Product.cs
│   ├── ValueObjects/Money.cs
│   └── Interfaces/IProductRepository.cs
├── Application/
│   ├── Services/ProductService.cs
│   └── Dtos/ProductDto.cs
├── Infrastructure/
│   ├── Data/AppDbContext.cs
│   └── Repositories/EfProductRepository.cs
└── Presentation/
    └── Api/Program.cs
```

**Пояснение:** Зависимости идут от Presentation к Domain.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
// в Domain/Interfaces
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default);
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken token = default);
    Task AddAsync(Product product, CancellationToken token = default);
}
```

**Пояснение:** Интерфейс принадлежит домену — инфраструктура его реализует.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
public sealed class EfProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public EfProductRepository(AppDbContext db) => _db = db;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default) =>
        await _db.Products.FirstOrDefaultAsync(p => p.Id == id, token);

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken token = default) =>
        await _db.Products.AsNoTracking().ToListAsync(token);

    public async Task AddAsync(Product product, CancellationToken token = default)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync(token);
    }
}
```

**Пояснение:** Инфраструктура знает про EF Core; домен — нет.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
public sealed class ProductService
{
    private readonly IProductRepository _repository;
    public ProductService(IProductRepository repository) => _repository = repository;

    public async Task<Product> CreateAsync(string name, decimal price, CancellationToken token = default)
    {
        if (price <= 0) throw new ArgumentException("Цена должна быть положительной");
        var product = new Product { Name = name, Price = price };
        await _repository.AddAsync(product, token);
        return product;
    }
}
```

**Пояснение:** Сервис работает с абстракцией, а не с EF напрямую.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
builder.Services.AddScoped<IProductRepository, EfProductRepository>();
builder.Services.AddScoped<ProductService>();
```

**Пояснение:** DI связывает контракт с реализацией.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _items = new();
    public Task<Product?> GetByIdAsync(Guid id, CancellationToken token = default) =>
        Task.FromResult(_items.FirstOrDefault(p => p.Id == id));
    public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken token = default) =>
        Task.FromResult<IReadOnlyList<Product>>(_items.ToList());
    public Task AddAsync(Product product, CancellationToken token = default)
    {
        _items.Add(product);
        return Task.CompletedTask;
    }
}

[Fact]
public async Task CreateAsync_NegativePrice_Throws()
{
    var service = new ProductService(new FakeProductRepository());
    await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync("Товар", -5));
}
```

**Пояснение:** Благодаря интерфейсу бизнес-логика тестируется без БД.

</details>
