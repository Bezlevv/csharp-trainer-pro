# Разбор кода — Модуль 06.1: Строки

## Пример 1: Потерянный результат

```csharp
string s = "Привет";
s.ToUpper();          // результат брошен!
Console.WriteLine(s); // Привет — не изменилась
```

**Проблема:** строки неизменяемы; `ToUpper()` возвращает новую, а мы её не сохранили.
**Как надо:** `s = s.ToUpper();`

## Пример 2: Split с пустыми элементами

```csharp
"a  b".Split(' ');   // ["a", "", "", "b"]
```

**Проблема:** несколько пробелов → пустые элементы.
**Как надо:** `Split(' ', StringSplitOptions.RemoveEmptyEntries)` → `["a", "b"]`.

## Пример 3: IndexOf == -1

```csharp
string s = "abc";
Console.WriteLine(s.Substring(s.IndexOf("zzz"))); // IndexOutOfRangeException
```

**Проблема:** `IndexOf` вернул −1 → `Substring(-1)` падает.
**Как надо:** проверяй `int idx = s.IndexOf("zzz"); if (idx >= 0) ...`

## Пример 4: Регистр в сравнениях

```csharp
if (input == "yes") { ... }  // "Yes" не пройдёт
```

**Как надо:** `if (input.Trim().Equals("yes", StringComparison.OrdinalIgnoreCase))`

## Пример 5: Конкатенация в цикле

```csharp
string s = "";
for (int i = 0; i < 10000; i++) s += i;   // медленно
```

**Как надо:** `StringBuilder` или `string.Join`.

## Вывод
Строки неизменяемы → всегда сохраняй результат. Используй `StringSplitOptions.RemoveEmptyEntries`, проверяй `IndexOf`, и сравнивай с учётом регистра осознанно.
