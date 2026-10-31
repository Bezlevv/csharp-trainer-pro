# Практика — Модуль 03

> Сначала попробуй решить сам. Подсказка — если застрял. Решение — в конце файла, в свёрнутом блоке.

## Задача 1: Утилиты для строк

**Сложность:** ★☆☆
**Описание:** Напиши методы: `IsPalindrome(string)` (палиндром — читается одинаково в обе стороны), `ReverseString(string)`.

**Требования:**
- `IsPalindrome` без учёта регистра и пробелов.
- Проверь на: `"Аргентина манит негра"`.

**Подсказка:** `string.Join("", s.Split(' '))` убирает пробелы; сравнение без учёта регистра — через `ToLower()`.

## Задача 2: GCD (наибольший общий делитель)

**Сложность:** ★☆☆
**Описание:** Реализуй алгоритм Евклида через цикл (не рекурсию).

**Требования:**
- Метод `int GCD(int a, int b)`.
- `GCD(48, 18) == 6`.

**Подсказка:** `while (b != 0) { int t = b; b = a % b; a = t; }`.

## Задача 3: `params`-сумма

**Сложность:** ★☆☆
**Описание:** Напиши метод `Sum(params int[] numbers)` и вызови его с разным числом аргументов.

**Требования:**
- `Sum(1, 2, 3)` → `6`, `Sum(10, 20)` → `30`.
- Покажи, что метод работает с любым числом аргументов.

## Задача 4: Обмен через `ref`

**Сложность:** ★★☆
**Описание:** Напиши метод `Swap(ref int a, ref int b)` и докажи, что он меняет оригиналы.

**Требования:**
- После `Swap(ref x, ref y)` значения `x` и `y` меняются местами.
- Объясни в комментарии, почему без `ref` это не сработает.

## Задача 5: Рекурсивная сумма цифр

**Сложность:** ★★☆
**Описание:** Реализуй вычисление суммы цифр числа рекурсией.

**Требования:**
- Метод `int SumOfDigits(int n)`.
- Базовый случай: `n < 10` → `n`.
- Пример: `SumOfDigits(1234) == 10`.

**Подсказка:** `n % 10 + SumOfDigits(n / 10)`.

## Задача 6: Бинарный поиск

**Сложность:** ★★★
**Описание:** Реализуй бинарный поиск по отсортированному массиву. Верни индекс элемента или `-1`, если его нет.

**Требования:**
- Метод `int BinarySearch(int[] sorted, int target)`.
- Массив отсортирован по возрастанию.
- Пустой массив → `-1`.

**Подсказка:** Поддерживай `left` и `right`; средина `mid = (left + right) / 2`; сужай диапазон в два раза на каждом шаге.

## Задача 7: Перегрузка `Area`

**Сложность:** ★★☆
**Описание:** Реализуй три перегрузки метода `Area` для круга, прямоугольника и треугольника.

**Требования:**
- `Area(double radius)` — круг (πr²).
- `Area(double width, double height)` — прямоугольник.
- `Area(double a, double b, double c)` — треугольник по формуле Герона.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
string Clean(string s) => string.Join("", s.Split(' ')).ToLower();

bool IsPalindrome(string s)
{
    string clean = Clean(s);
    int n = clean.Length;
    for (int i = 0; i < n / 2; i++)
    {
        if (clean[i] != clean[n - 1 - i]) return false;
    }
    return true;
}

string ReverseString(string s)
{
    char[] arr = s.ToCharArray();
    Array.Reverse(arr);
    return new string(arr);
}

Console.WriteLine(IsPalindrome("Аргентина манит негра")); // True
Console.WriteLine(ReverseString("C#")); // #C
```

**Пояснение:** `Clean` убирает пробелы и приводит к нижнему регистру. Сравнение идёт с двух концов к середине.

</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
int GCD(int a, int b)
{
    while (b != 0)
    {
        int t = b;
        b = a % b;
        a = t;
    }
    return a;
}

Console.WriteLine(GCD(48, 18)); // 6
```

**Пояснение:** Алгоритм Евклида: заменяем `(a, b)` на `(b, a % b)`, пока остаток не станет нулём.

</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int n in numbers) total += n;
    return total;
}

Console.WriteLine(Sum(1, 2, 3));        // 6
Console.WriteLine(Sum(10, 20));        // 30
Console.WriteLine(Sum());              // 0 (пустой вызов тоже валиден)
```

**Пояснение:** `params` собирает произвольное число аргументов в массив. Пустой вызов тоже допустим.

</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
void Swap(ref int a, ref int b)
{
    (a, b) = (b, a);
}

int x = 1, y = 2;
Console.WriteLine($"{x} {y}"); // 1 2
Swap(ref x, ref y);
Console.WriteLine($"{x} {y}"); // 2 1
```

**Пояснение:** Без `ref` метод получил бы копии, и обмен «испарился бы» по завершении. `ref` передаёт ссылку на оригинал, поэтому изменения видны вызывающему.

</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
int SumOfDigits(int n)
{
    if (n < 0) n = -n;
    if (n < 10) return n;
    return n % 10 + SumOfDigits(n / 10);
}

Console.WriteLine(SumOfDigits(1234)); // 10
```

**Пояснение:** Базовый случай — однозначное число. Иначе берём последнюю цифру и рекурсивно считаем сумму остальных.

</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
int BinarySearch(int[] sorted, int target)
{
    int left = 0, right = sorted.Length - 1;

    while (left <= right)
    {
        int mid = left + (right - left) / 2;

        if (sorted[mid] == target) return mid;
        if (sorted[mid] < target) left = mid + 1;
        else right = mid - 1;
    }

    return -1;
}

int[] arr = { 1, 3, 5, 7, 9 };
Console.WriteLine(BinarySearch(arr, 5));  // 2
Console.WriteLine(BinarySearch(arr, 4));  // -1
```

**Пояснение:** `left + (right - left) / 2` защищает от переполнения при больших `left+right`. Каждый шаг сужает диапазон вдвое — сложность O(log n).

</details>

<details>
<summary>Задача 7 — решение</summary>

```csharp
const double Pi = 3.14159;

double Area(double radius) => Pi * radius * radius;

double Area(double width, double height) => width * height;

double Area(double a, double b, double c)
{
    double p = (a + b + c) / 2;
    return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
}

Console.WriteLine(Area(2));                // круг: ~12.566
Console.WriteLine(Area(3, 4));             // прямоугольник: 12
Console.WriteLine(Area(3, 4, 5));          // треугольник: 6
```

**Пояснение:** Компилятор выбирает перегрузку по количеству и типам аргументов. Формула Герона использует полупериметр `p`.

</details>
