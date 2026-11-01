# Практика — Модуль 28: Шпаргалка

## Задача 1: Мини-проект

**Сложность:** ★☆☆
**Описание:** Создай консольный проект и добавь его в решение.

**Требования:**
- `dotnet new console`, `dotnet new sln`, `dotnet sln add`.

## Задача 2: Тесты

**Сложность:** ★★☆
**Описание:** Добавь тестовый проект и запусти `dotnet test`.

**Требования:**
- ссылка на основной проект.

## Задача 3: Пакет

**Сложность:** ★☆☆
**Описание:** Добавь пакет `Newtonsoft.Json`.

**Требования:**
- `dotnet add package`.

## Задача 4: Публикация

**Сложность:** ★★☆
**Описание:** Опубликуй в `out`.

**Требования:**
- `dotnet publish -c Release -o out`.

## Задача 5: Ми (ция

**Сложность:** ★★★
**Описание:** Для EF-проекта создай миграцию.

**Требования:**
- `dotnet ef migrations add InitialCreate` (нужен EF-проект).

## Решения

<details>
<summary>Задача 1 — решение</summary>

```bash
dotnet new console -n MyApp
dotnet new sln -n MyApp
dotnet sln add MyApp/MyApp.csproj
```
</details>

<details>
<summary>Задача 2 — решение</summary>

```bash
dotnet new xunit -n MyApp.Tests
dotnet add MyApp.Tests reference MyApp/MyApp.csproj
dotnet sln add MyApp.Tests/MyApp.Tests.csproj
dotnet test
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```bash
dotnet add package Newtonsoft.Json
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```bash
dotnet publish -c Release -o out
```
</details>

<details>
<summary>Задача 5 — решение</summary>

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```
</details>
