# Практика — Модуль 11

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Асинхронная загрузка

**Сложность:** ★☆☆
**Описание:** Метод `Task<int> FetchAsync(int id)`, имитирующий задержку через `Task.Delay`.

**Требования:**
- `Take.Delay(500)`.
- Используй `await` в `Main`.
- Выведи до и после `await`.

## Задача 2: Параллельные загрузки

**Сложность:** ★★☆
**Описание:** Запусти три `FetchAsync` параллельно через `Task.WhenAll`.

**Требования:**
- Суммируй результаты.
- Замерь время (Stopwatch) — должно быть ~500 мс, а не 1500.

## Задача 3: Отмена по таймауту

**Сложность:** ★★★
**Описание:** Используй `CancellationTokenSource` с `CancelAfter`.

**Требования:**
- Операция дольше таймаута → `OperationCanceledException`.
- Обработай через `try-catch`.

## Задача 4: «Кто быстрее»

**Сложность:** ★★☆
**Описание:** `Task.WhenAny` — первый ответ из трёх источников.

**Требования:**
- Три задержки (300/600/900 мс).
- Получи первый завершившийся результат.

## Задача 5: Цепочка с `CancellationToken`

**Сложность:** ★★★
**Описание:** Сымитируй «UI → сервис → репозиторий» с пробросом токена.

**Требования:**
- Каждый метод принимает `CancellationToken`.
- Токен пробрасывается насквозь, проверяется `ThrowIfCancellationRequested()`.

## Задача 6: Рефакторинг синхронного кода

**Сложность:** ★★★
**Описание:** Перепиши «блокирующий» код на async-версию.

**Требования:**
- До: `GetStringAsync(...).Result`.
- После: `await`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
async Task<int> FetchAsync(int id)
{
    Console.WriteLine($"Начало #{id}");
    await Task.Delay(500);
    Console.WriteLine($"Конец #{id}");
    return id * 10;
}

Console.WriteLine("До await");
int result = await FetchAsync(1);
Console.WriteLine($"После await, результат = {result}");
```

**Пояснение:** `await` временно приостанавливает метод, но поток свободен — UI не «зависнет».

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
async Task<int> FetchAsync(int id)
{
    await Task.Delay(500);
    return id;
}

var watch = System.Diagnostics.Stopwatch.StartNew();

Task<int> t1 = FetchAsync(1);
Task<int> t2 = FetchAsync(2);
Task<int> t3 = FetchAsync(3);

int[] results = await Task.WhenAll(t1, t2, t3);
watch.Stop();

Console.WriteLine($"Сумма: {results.Sum()}, время: {watch.ElapsedMilliseconds} мс");
```

**Пояснение:** `WhenAll` запускает задачи параллельно — суммарное время ≈ времени самой долгой.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
async Task LongOperationAsync(CancellationToken token)
{
    for (int i = 0; i < 100; i++)
    {
        token.ThrowIfCancellationRequested();
        await Task.Delay(100, token);
    }
}

using var cts = new CancellationTokenSource();
cts.CancelAfter(250);

try
{
    await LongOperationAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Операция отменена по таймауту");
}
```

**Пояснение:** `CancelAfter` автоматически отменяет токен через заданный интервал.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
async Task<int> DelayIdAsync(int id, int ms)
{
    await Task.Delay(ms);
    return id;
}

Task<int> a = DelayIdAsync(1, 300);
Task<int> b = DelayIdAsync(2, 600);
Task<int> c = DelayIdAsync(3, 900);

Task<int> first = await Task.WhenAny(a, b, c);
Console.WriteLine($"Первый завершился: #{await first}");
```

**Пояснение:** `WhenAny` возвращает первый завершившийся `Task`.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
async Task<string> UiOperationAsync(CancellationToken token)
{
    string data = await RepositoryAsync(token);
    return data.ToUpper();
}

async Task<string> RepositoryAsync(CancellationToken token)
{
    token.ThrowIfCancellationRequested();
    await Task.Delay(500, token);
    return "данные";
}

using var cts = new CancellationTokenSource();
cts.CancelAfter(300);

try
{
    string result = await UiOperationAsync(cts.Token);
    Console.WriteLine(result);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Цепочка прервана");
}
```

**Пояснение:** Токен пробрасывается через все уровни — отмену можно остановить в любом звене.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
// ДО — блокирующе и опасно:
// var client = new HttpClient();
// string html = client.GetStringAsync("https://example.com").Result;

// ПОСЛЕ — корректно:
async Task<string> GetPageAsync(HttpClient client)
{
    return await client.GetStringAsync("https://example.com");
}

using var client = new HttpClient();
string html = await GetPageAsync(client);
Console.WriteLine(html.Length);
```

**Пояснение:** `await` вместо `.Result` — без блокировки потока и без риска дедлока.

</details>
