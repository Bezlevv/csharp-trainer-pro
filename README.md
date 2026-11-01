# 🚀 C# Trainer Pro — курс по C# с нуля до middle

[![Build](https://github.com/Bezlevv/csharp-trainer-pro/actions/workflows/build.yml/badge.svg)](https://github.com/Bezlevv/csharp-trainer-pro/actions/workflows/build.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

Полный курс по языку C# и платформе .NET: **теория → практика → разбор кода**.
16 модулей, более 100 задач, десятки разборов «как не надо» → «как надо», и финальный
проект с архитектурой и тестами.

## Для кого

- Полный новичок, который хочет выучить C# и устроиться junior-разработчиком.
- Junior, которому не хватает системности: ООП, обобщения, LINQ, async, тесты, SOLID.
- Разработчик с другого языка, быстро осваивающий идиомы C#/.NET.

## Требования

- [ .NET SDK 8.0+ ](https://dotnet.microsoft.com/download) (курс написан на C# 12 / .NET 8,
  код компилируется и более свежими SDK).
- IDE на выбор: Visual Studio 2022, JetBrains Rider или VS Code + C# Dev Kit.
- Желание писать код руками, а не копировать.

## Как учиться

> 🗺️ **Есть полная дорожная карта:** [ROADMAP.md](ROADMAP.md) — пошаговый план от нуля до Senior с теорией «как для ребёнка», задачами, подсказками и чек-поинтами.
>
> 📖 **Развёрнутые уроки с тестами:** [`lessons/level0/`](lessons/level0/), [`lessons/level1/`](lessons/level1/), [`lessons/level2/`](lessons/level2/), [`lessons/level3/`](lessons/level3/) и [`lessons/level4/`](lessons/level4/) — полный путь от нуля до Senior: теория с нуля, практика и проверочные тесты с объяснением каждого ответа.

На каждый модуль отводится 3–7 дней. Алгоритм:

1. **Теория** (`README.md` модуля) — читай, набирай примеры руками.
2. **Контрольные вопросы** — в конце теории, отвечай вслух или письменно.
3. **Практика** (`PRACTICE.md`) — задачи по возрастанию сложности. Решения —
   в свёрнутых блоках `<details>` в конце файла: сначала попробуй сам.
4. **Разбор кода** (`CODE_REVIEW.md`) — посмотри, какие ошибки делают все,
   и почему предложенный рефакторинг лучше.
5. Вернись к модулю через неделю — повтори контрольные вопросы.

Примерный план: **12–16 недель** при 1–2 часа в день.

### 🌐 Интерактивная веб-версия

Весь курс доступен как **один автономный HTML-файл**: [`web/course-standalone.html`](web/course-standalone.html).

Открой его двойным кликом — внутри уже всё: теория, практика, разбор кода по всем 22 модулям, интерактивные задачи (25 вопросов + 25 заданий на код), боковое меню, вкладки и подсветка кода. Работает **без сервера, без интернета и без установки чего-либо**.

### 📕 Печатная версия (PDF)

Готовый **PDF всего курса** (план + уроки 0–4 + модули 00–21): [`web/course.pdf`](web/course.pdf) (~6.7 МБ).
Пересобрать после правок: `node web/build-print.cjs` → затем конвертировать `web/course-print.html` в PDF (например, через браузер «Печать → Сохранить как PDF»).

### 🔬 Реальная песочница C# (запуск настоящего кода)

Хочешь **реально запускать** C# из браузера (с компиляцией и выводом) — а не только проходить тесты? Есть локальная песочница:

```bash
# 1. Запусти локальный сервер песочницы (нужен установленный .NET SDK):
node web/sandbox-server.js
# 2. Открой в браузере:
#   http://localhost:8091/
```

Пишешь код в левой панели, жмёшь «▶ Запустить» — и видишь **настоящий вывод** (через `dotnet run`). Есть готовые примеры: цикл, класс, LINQ.
Команда компилирует и выполняет код на твоей машине, поэтому работает офлайн. (Ввод `Console.ReadLine` в песочнице не поддерживается.)

## Структура курса

| № | Модуль | Темы | Теория | Практика | Разбор |
|---|--------|------|--------|----------|--------|
| 00 | [Введение в C# и .NET](modules/00-intro/README.md) | CLR, IL/JIT, SDK, CLI, NuGet, первая программа | [теория](modules/00-intro/README.md) | [практика](modules/00-intro/PRACTICE.md) | [разбор](modules/00-intro/CODE_REVIEW.md) |
| 01 | [Основы синтаксиса](modules/01-basics/README.md) | типы, переменные, строки, операторы, приведение | [теория](modules/01-basics/README.md) | [практика](modules/01-basics/PRACTICE.md) | [разбор](modules/01-basics/CODE_REVIEW.md) |
| 02 | [Управление потоком](modules/02-control-flow/README.md) | if, switch-паттерны, циклы | [теория](modules/02-control-flow/README.md) | [практика](modules/02-control-flow/PRACTICE.md) | [разбор](modules/02-control-flow/CODE_REVIEW.md) |
| 03 | [Методы](modules/03-methods/README.md) | параметры, ref/out, перегрузка, рекурсия | [теория](modules/03-methods/README.md) | [практика](modules/03-methods/PRACTICE.md) | [разбор](modules/03-methods/CODE_REVIEW.md) |
| 04 | [ООП: классы и инкапсуляция](modules/04-oop-basics/README.md) | классы, свойства, конструкторы, static | [теория](modules/04-oop-basics/README.md) | [практика](modules/04-oop-basics/PRACTICE.md) | [разбор](modules/04-oop-basics/CODE_REVIEW.md) |
| 05 | [Наследование и полиморфизм](modules/05-inheritance/README.md) | virtual/abstract, интерфейсы, sealed | [теория](modules/05-inheritance/README.md) | [практика](modules/05-inheritance/PRACTICE.md) | [разбор](modules/05-inheritance/CODE_REVIEW.md) |
| 06 | [Продвинутые типы](modules/06-advanced-types/README.md) | struct, enum, record, туплы, паттерн-матчинг | [теория](modules/06-advanced-types/README.md) | [практика](modules/06-advanced-types/PRACTICE.md) | [разбор](modules/06-advanced-types/CODE_REVIEW.md) |
| 06.1 | [Строки: методы](modules/06_1-strings/README.md) | поиск, замена, Split, сравнение, форматирование | [теория](modules/06_1-strings/README.md) | [практика](modules/06_1-strings/PRACTICE.md) | [разбор](modules/06_1-strings/CODE_REVIEW.md) |
| 07 | [Обобщения и коллекции](modules/07-generics-collections/README.md) | generics, List/Dictionary/HashSet, IEnumerable | [теория](modules/07-generics-collections/README.md) | [практика](modules/07-generics-collections/PRACTICE.md) | [разбор](modules/07-generics-collections/CODE_REVIEW.md) |
| 08 | [Исключения](modules/08-exceptions/README.md) | try/catch/finally, свои исключения, using | [теория](modules/08-exceptions/README.md) | [практика](modules/08-exceptions/PRACTICE.md) | [разбор](modules/08-exceptions/CODE_REVIEW.md) |
| 09 | [Делегаты, лямбды, события](modules/09-delegates-events/README.md) | delegate, Func/Action, event | [теория](modules/09-delegates-events/README.md) | [практика](modules/09-delegates-events/PRACTICE.md) | [разбор](modules/09-delegates-events/CODE_REVIEW.md) |
| 10 | [LINQ](modules/10-linq/README.md) | deferred execution, операторы, Join/GroupBy | [теория](modules/10-linq/README.md) | [практика](modules/10-linq/PRACTICE.md) | [разбор](modules/10-linq/CODE_REVIEW.md) |
| 11 | [Асинхронность](modules/11-async/README.md) | Task, async/await, CancellationToken | [теория](modules/11-async/README.md) | [практика](modules/11-async/PRACTICE.md) | [разбор](modules/11-async/CODE_REVIEW.md) |
| 12 | [Файлы, даты, сериализация](modules/12-files-json/README.md) | File/Path, System.Text.Json, DateTime | [теория](modules/12-files-json/README.md) | [практика](modules/12-files-json/PRACTICE.md) | [разбор](modules/12-files-json/CODE_REVIEW.md) |
| 13 | [Юнит-тесты](modules/13-unit-testing/README.md) | xUnit, AAA, моки, TDD | [теория](modules/13-unit-testing/README.md) | [практика](modules/13-unit-testing/PRACTICE.md) | [разбор](modules/13-unit-testing/CODE_REVIEW.md) |
| 14 | [SOLID и паттерны](modules/14-solid-patterns/README.md) | 5 принципов, Factory, Strategy, Observer, DI | [теория](modules/14-solid-patterns/README.md) | [практика](modules/14-solid-patterns/PRACTICE.md) | [разбор](modules/14-solid-patterns/CODE_REVIEW.md) |
| 15 | [Итоговый проект](modules/15-final-project/README.md) | консольный трекер задач: архитектура, слои, тесты | [теория](modules/15-final-project/README.md) | [практика](modules/15-final-project/PRACTICE.md) | [разбор](modules/15-final-project/CODE_REVIEW.md) |
| 16 | [Git и рабочий процесс](modules/16-git/README.md) | ветки, merge/rebase, PR, CI, conventional commits | [теория](modules/16-git/README.md) | [практика](modules/16-git/PRACTICE.md) | [разбор](modules/16-git/CODE_REVIEW.md) |
| 17 | [Введение в ASP.NET Core](modules/17-aspnet/README.md) | minimal API, HTTP, DI, middleware, тесты API | [теория](modules/17-aspnet/README.md) | [практика](modules/17-aspnet/PRACTICE.md) | [разбор](modules/17-aspnet/CODE_REVIEW.md) |
| 18 | [Базы данных и EF Core](modules/18-databases/README.md) | DbContext, LINQ-запросы, миграции, провайдеры | [теория](modules/18-databases/README.md) | [практика](modules/18-databases/PRACTICE.md) | [разбор](modules/18-databases/CODE_REVIEW.md) |
| 19 | [Продвинутая архитектура](modules/19-architecture/README.md) | Clean Architecture, слои, Repository, CQRS | [теория](modules/19-architecture/README.md) | [практика](modules/19-architecture/PRACTICE.md) | [разбор](modules/19-architecture/CODE_REVIEW.md) |
| 20 | [Наблюдаемость](modules/20-observability/README.md) | логирование, метрики, трейсинг, OpenTelemetry | [теория](modules/20-observability/README.md) | [практика](modules/20-observability/PRACTICE.md) | [разбор](modules/20-observability/CODE_REVIEW.md) |
| 21 | [Безопасность веб-приложений](modules/21-security/README.md) | JWT, роли, секреты, OWASP Top 10 | [теория](modules/21-security/README.md) | [практика](modules/21-security/PRACTICE.md) | [разбор](modules/21-security/CODE_REVIEW.md) |
| 22 | [Docker и деплой](modules/22-docker/README.md) | контейнеры, Dockerfile, compose, деплой | [теория](modules/22-docker/README.md) | [практика](modules/22-docker/PRACTICE.md) | [разбор](modules/22-docker/CODE_REVIEW.md) |
| 23 | [Собеседование и повторение](modules/23-interview/README.md) | карта повторения, типичные вопросы | [теория](modules/23-interview/README.md) | [практика](modules/23-interview/PRACTICE.md) | [разбор](modules/23-interview/CODE_REVIEW.md) |

## 🏁 Итоговый проект

**Task Tracker** — консольное приложение-трекер задач с архитектурой в слоях
(модели → сервисы → хранилище JSON → CLI), валидацией и юнит-тестами.
Исходники: [`modules/15-final-project/task-tracker`](modules/15-final-project/task-tracker).

```bash
git clone https://github.com/Bezlevv/csharp-trainer-pro.git
cd csharp-trainer-pro/modules/15-final-project/task-tracker
dotnet run --project src/TaskTracker
dotnet test
```

## 📈 Что дальше после курса

- **Docker и DevOps** — контейнеризация, деплой, CI/CD.
- **Микросервисы и сообщения** — RabbitMQ/Kafka, распределённые системы.
- **Производительность .NET** — профилирование, BenchmarkDotNet, Span/Memory.
- **Системный дизайн** — масштабируемость, кэширование, распределённые системы.

Базовый стек уже пройден: C# (01–15), Git (16), ASP.NET Core (17), EF Core (18), архитектура (19), наблюдаемость (20), безопасность (21).

Курс даёт прочную базу: всё из этого списка будет понятным продолжением,
а не стеной.

## Участие

Нашёл ошибку или опечатку — открой issue или pull request. Курс развивается.

## Лицензия

[MIT](LICENSE) — используй для обучения свободно.
