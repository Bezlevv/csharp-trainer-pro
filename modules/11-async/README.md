# Модуль 11: Асинхронность

> **Цель:** использовать `async`/`await` для неблокирующих операций и корректно отменять их.

## Зачем это нужно

Операции ввода-вывода (сеть, диск, базы данных) — в тысячи раз медленнее вычислений. Если ждать их «в лоб» — программа зависает. Асинхронность позволяет не блокировать поток, пока идёт ожидание. Это обязательно для веб-API, десктопных UI и любых сервисов.

## Теория

### 1. Проблема

```csharp
var response = httpClient.GetStringAsync("https://example.com").Result; // плохо!
```

Ожидание результата блокирует поток. В интерфейсе это «зависшее окошко».

### 2. `async` / `await`

```csharp
async Task DownloadAsync()
{
    var client = new HttpClient();
    var content = await client.GetStringAsync("https://example.com");
    Console.WriteLine(content.Length);
}
```

- `await` приостанавливает метод, **не блокируя поток**. Поток освобождается.
- `async Task` — стандарт. `async void` — только для обработчиков событий (иначе ошибки теряются).
- `Task<T>` возвращает значение: `var data = await GetDataAsync();`

### 3. Что возвращает `await`

- `await` «разворачивает» `Task<T>` в `T`.
- Ошибки внутри `Task` всплывают в точке `await` как исключения.

### 4. `CancellationToken`

Для отмены:

```csharp
async Task ProcessAsync(CancellationToken token)
{
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested(); // проверим отмену
        await Task.Delay(100, token);
    }
}
```

```csharp
using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromMilliseconds(300));

try
{
    await ProcessAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Операция отменена");
}
```

### 5. `Task.WhenAll` и `Task.WhenAny`

**WhenAll** — ждать все параллельно:

```csharp
Task<int> t1 = GetAsync(1);
Task<int> t2 = GetAsync(2);
int[] results = await Task.WhenAll(t1, t2);
```

**WhenAny** — ждать первый завершившийся:

```csharp
var first = await Task.WhenAny(DownloadA(), DownloadB());
```

### 6. `Task.Run` для CPU-bound

`Task.Run` запускает код в фоновом потоке. Для CPU-bound-вычислений (не IO) это может помочь не блокировать UI:

```csharp
int result = await Task.Run(() => HeavyComputation());
```

Но для IO (сеть/диск) — `async`-методы без `Task.Run`.

### 7. `.Result` / `.Wait()` и deadlock

Никогда не вызывай `.Result`/`.Wait()` внутри async-кода — это блокирует и может дать **deadlock** (взаимоблокировку) в окружениях с контекстом синхронизации (UI, ASP.NET). Всегда `await`.

### 8. `ConfigureAwait(false)`

Библиотечный код без UI/ASP.NET-контекста может использовать `ConfigureAwait(false)`, чтобы не захватывать контекст:

```csharp
var data = await GetAsync().ConfigureAwait(false);
```

Не обязательно для новичка, но полезно знать.

### 9. `IAsyncEnumerable<T>` и `await foreach`

Асинхронный перебор (например, чтение большого потока построчно):

```csharp
await foreach (var chunk in ReadChunksAsync())
{
    Console.WriteLine(chunk);
}
```

## Типичные ошибки

| Ошибка | Почему плохо | Как правильно |
|---|---|---|
| `async void` не в обработчике | ошибки теряются + нельзя await | `async Task` |
| `.Result`/`.Wait()` | блокировка, дедлок | `await` |
| Забытый `await` в async-методе | warning CS4014/CS1998 | добавить `await` или осознанно `_ = ...` |
| Fire-and-forget | потерянные исключения | дожидаться через await |
| `async` в методе без `await` | лишний статусный автомат | убрать `async` |

## Контрольные вопросы

1. Что значит «не блокирует поток»?
2. Почему `async void` опасен?
3. Что делает `CancellationToken`?
4. Как запустить несколько задач параллельно?
5. В чём разница `WhenAll` и `WhenAny`?
6. Почему `.Result` — плохо?
7. Что делает `ConfigureAwait(false)`?

## Дальше

- [Практика](PRACTICE.md) — задачи на закрепление
- [Разбор кода](CODE_REVIEW.md) — типичные ошибки и рефакторинг
