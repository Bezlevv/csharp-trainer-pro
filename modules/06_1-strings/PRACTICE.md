# Практика — Модуль 06.1: Строки

> Реши задачи сам, потом сверься с решениями в конце.

## Задача 1: Валидация email

**Сложность:** ★☆☆
**Описание:** Проверь, что строка похожа на email: содержит `@`, после `@` есть точка, длина > 5.

**Требования:**
- Метод `bool LooksLikeEmail(string s)`.
- `Contains("@"`, `IndexOf('.')` после `@`.

## Задача 2: Обратный порядок слов

**Сложность:** ★★☆
**Описание:** Дана строка слов. Выведи слова в обратном порядке.

**Требования:**
- `Split(' ')` → `Array.Reverse` → `string.Join`.

## Задача 3: Палиндром (с очисткой)

**Сложность:** ★★☆
**Описание:** Проверь, палиндром ли фраза (без учёта регистра, пробелов и знаков).

**Требования:**
- Очисти: `ToLower()`, убери пробелы и знаки препинания.
- Сравни с развёрнутой версией.

## Задача 4: Подсчёт слов

**Сложность:** ★☆☆
**Описание:** Посчитай количество слов в строке.

**Требования:**
- `Split(' ', StringSplitOptions.RemoveEmptyEntries).Length`.

## Задача 5: Акроним

**Сложность:** ★★☆
**Описание:** Из строки «объектно-ориентированное программирование» сделай «ООП».

**Требования:**
- Разбей по словам, возьми первую букву каждого, `ToUpper()`.

## Задача 6: Сжатие пробелов

**Сложность:** ★★★
**Описание:** Замени серии пробелов на один.

**Требования:**
- Через разбиение с `RemoveEmptyEntries` + `string.Join(" ", ...)`.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```csharp
bool LooksLikeEmail(string s)
{
    if (string.IsNullOrWhiteSpace(s)) return false;
    int at = s.IndexOf('@');
    return at > 0 && s.IndexOf('.', at) > at + 1;
}
```

**Пояснение:** `@` есть, перед ним что-то есть, после него — точка дальше.
</details>

<details>
<summary>Задача 2 — решение</summary>

```csharp
string ReverseWords(string s)
{
    string[] words = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    Array.Reverse(words);
    return string.Join(" ", words);
}
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```csharp
bool IsPalindrome(string s)
{
    var cleaned = new string(s.ToLower().Where(char.IsLetterOrDigit).ToArray());
    char[] arr = cleaned.ToCharArray();
    Array.Reverse(arr);
    return cleaned == new string(arr);
}
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```csharp
int CountWords(string s) =>
    s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
```
</details>

<details>
<summary>Задача 5 — решение</summary>

```csharp
string Acronym(string phrase) =>
    string.Concat(phrase.Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(w => w[0].ToString().ToUpper()));
```
</details>

<details>
<summary>Задача 6 — решение</summary>

```csharp
string CollapseSpaces(string s) =>
    string.Join(" ", s.Split(' ', StringSplitOptions.RemoveEmptyEntries));
```
</details>
