# Практика — Модуль 26: Производительность

## Задача 1: Замер скорости

**Сложность:** ★☆☆
**Описание:** Используй `Stopwatch`, чтобы замерить время заполнения списка.

**Требования:**
- `Stopwatch.StartNew()`, `ElapsedMilliseconds`.

## Задача 2: List → HashSet

**Сложность:** ★★☆
**Описание:** Перепиши поиск пересечения двух списков через HashSet.

**Требования:**
- Покажи результат.

## Задача 3: StringBuilder

**Сложность:** ★★☆
**Описание:** Собери строку из 1000 чисел через StringBuilder (вместо +=).

**Требования:**
- `using System.Text;`.

## Задача 4: Простой BenchmarkDotNet

**Сложность:** ★★★
**Описание:** Напиши класс с `[Benchmark]` и запусти в Release.

**Требования:**
- Пакет `BenchmarkDotNet`.
- `[MemoryDiagnoser]`.

## Задача 5: Найди узкое место

**Сложность:** ★★★
**Описание:** Дан код с `List.Contains` в цикле. Перепиши, замерь до/после.

**Требования:**
- Stopwatch до и после.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
var sw = Stopwatch.StartNew();
var list = new List<int>();
for (int i = 0; i < 100000; i++) list.Add(i);
sw.Stop();
Console.WriteLine($"{sw.ElapsedMilliseconds} мс");
```
</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
var setB = listB.ToHashSet();
var common = listA.Where(setB.Contains).ToList();
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++) sb.Append(i);
Console.WriteLine(sb.ToString());
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
[MemoryDiagnoser]
public class Bench
{
    [Benchmark]
    public int Sum() => Enumerable.Range(1, 10000).Sum();
}
```
Запуск: `dotnet run -c Release`.
</details>

<details>
<summary>Задача 5 — решение</summary>

До: `foreach (var x in a) if (b.Contains(x)) ...` — O(n²).
После: `var sb = b.ToHashSet();` и `sb.Contains(x)` — O(n). Замерь оба Stopwatch'ем.
</details>
