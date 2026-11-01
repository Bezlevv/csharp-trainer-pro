# Модуль 28: Шпаргалка — команды .NET CLI и Git

> **Цель:** собрать самые нужные команды в одно место для быстрой справки.

## .NET CLI

| Задача | Команда |
|---|---|
| Создать консольный проект | `dotnet new console -n MyApp` |
| Создать решение | `dotnet new sln -n MyApp` |
| Добавить проект в решение | `dotnet sln add src/MyApp/MyApp.csproj` |
| Создать проект тестов | `dotnet new xunit -n MyApp.Tests` |
| Добавить ссылку на проект | `dotnet add MyApp.Tests reference src/MyApp` |
| Добавить пакет NuGet | `dotnet add package Newtonsoft.Json` |
| Собрать | `dotnet build` |
| Запустить | `dotnet run` |
| Запустить тесты | `dotnet test` |
| Тесты только smoke | `dotnet test --filter Category=Smoke` |
| Публикация | `dotnet publish -c Release -o out` |

## EF Core

| Задача | Команда |
|---|---|
| Установить инструмент | `dotnet tool install --global dotnet-ef` |
| Создать миграцию | `dotnet ef migrations add InitialCreate` |
| Применить миграции | `dotnet ef database update` |

## Git

| Задача | Команда |
|---|---|
| Посмотреть статус | `git status` |
| Индексировать всё | `git add -A` |
| Коммит | `git commit -m "feat: ..."` |
| Ветка | `git switch -c feature/x` |
| Слияние | `git merge feature/x` |
| Обновить от origin | `git pull --rebase origin main` |
| ЗапушИТ | `git push -u origin main` |
| Откатить опубликованное | `git revert <sha>` |

## Полезные приёмы

- `git log --oneline --graph` — наглядная история.
- `dotnet format` — форматирование.
- `gh pr create` — создание pull request в GitHub CLI.

## Типичные ошибки

| Ошибка | Решение |
|---|---|
| `dotnet test` не находит проект | добавь тестовый проект в решение |
| Пуш без `-u` в первый раз | `git push -u origin main` |
| Миграция не видна | проверь `dotnet ef` установлен |

## Контрольные вопросы
1. Команда создания консольного проекта?
2. Команда добавления пакета?
3. Команда запуска тестов?
4. Команда создания миграции?
5. Команда revert?

## Дальше
- [Практика](PRACTICE.md)
- [Разбор кода](CODE_REVIEW.md)
