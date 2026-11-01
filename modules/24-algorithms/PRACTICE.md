# Практика — Модуль 24: Алгоритмы

## Задача 1: Линейный поиск

**Сложность:** ★☆☆
**Описание:** Реализуй `LinearSearch` и протестируй.

**Требования:**
- Возврат индекса или −1.

## Задача 2: Бинарный поиск

**Сложность:** ★★☆
**Описание:** Реализуй `BinarySearch` по отсортированному массиву.

**Требования:**
- Проверь на чётном и нечётном количестве элементов.

## Задача 3: Пузырьковая сортировка

**Сложность:** ★★☆
**Описание:** Реализуй `BubbleSort` и выведи результат.

**Требования:**
- Используй кортеж для обмена (`(a, b) = (b, a)`).

## Задача 4: Уникальные элементы

**Сложность:** ★☆☆
**Описание:** Из массива выведи только уникальные значения.

**Требования:**
- Используй `HashSet<int>`.

## Задача 5: Частотный словарь

**Сложность:** ★★☆
**Описание:** Посчитай, сколько раз встречается каждое слово.

**Требования:**
- `Dictionary<string, int>`.

## Задача 6: Проверка скобок

**Сложность:** ★★★
**Описание:** Реализуй `IsBalanced` через Stack.

**Требования:**
- Проверь `"([{}])"` → true, `"([)]"` → false.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
int LinearSearch(int[] arr, int target)
{
    for (int i = 0; i < arr.Length; i++)
        if (arr[i] == target) return i;
    return -1;
}
```
</details>

<details>
<summary>Задача 2 — решение</summary>

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
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
void BubbleSort(int[] arr)
{
    for (int i = 0; i < arr.Length - 1; i++)
        for (int j = 0; j < arr.Length - 1 - i; j++)
            if (arr[j] > arr[j + 1])
                (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
}
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
var unique = new HashSet<int>(arr);
foreach (var x in unique) Console.WriteLine(x);
```
</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
var freq = new Dictionary<string, int>();
foreach (var w in words)
    freq[w] = freq.GetValueOrDefault(w) + 1;
```
</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
bool IsBalanced(string s)
{
    var stack = new Stack<char>();
    foreach (char c in s)
    {
        if (c is '(' or '[' or '{') stack.Push(c);
        else if (c is ')' or ']' or '}')
        {
            if (stack.Count == 0) return false;
            if ((stack.Pop(), c) is ('(', ')') or ('[', ']') or ('{', '}')) continue;
            return false;
        }
    }
    return stack.Count == 0;
}
```
</details>
