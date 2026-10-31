# Практика — Модуль 16

> Выполняй в отдельной учебной папке (НЕ в этом репозитории).

## Задача 1: Первый репозиторий

**Сложность:** ★☆☆
**Описание:** Создай проект, проинициализируй git, сделай первый коммит.

**Требования:**
- `git init`.
- Создай файл `README.md`, добавь его (`git add`), закоммить.
- Посмотри `git log --oneline`.

## Задача 2: Ветки

**Сложность:** ★★☆
**Описание:** Создай ветку, внеси изменения, смержи в main.

**Требования:**
- `git switch -c feature/hello`.
- Добавь код/текст, закоммить.
- Вернись в main (`git switch main`) и сделай `git merge feature/hello`.

## Задача 3: Конфликт

**Сложность:** ★★★
**Описание:** Провоцируй конфликт слияния и разреши его.

**Требования:**
- В main и в ветке измени одну и ту же строку файла.
- При merge получишь конфликт: разбери маркеры `<<<<<<<` / `=======` / `>>>>>>>`, выбери правильный вариант, закоммить.

## Задача 4: Откат

**Сложность:** ★★★
**Описание:** Освой `git revert` и `git reset`.

**Требования:**
- Сделай плохой коммит.
- Отмени его через `git revert <sha>`.
- Пойми, почему revert безопаснее reset в публичной истории.

## Задача 5: Conventional Commits

**Сложность:** ★☆☆
**Описание:** Перепиши свои будущие коммиты в формате conventional.

**Требования:**
- Используй `feat:`, `fix:`, `docs:` и т.д.

## Задача 6: Исследуй CI курса

**Сложность:** ★★☆
**Описание:** Прочитай `.github/workflows/build.yml` курса.

**Требования:**
- Пойми, что делает каждый шаг.
- Ответь: зачем `--configuration Release` и `--no-build`?

## Решения

<details>
<summary>Задача 1 — решение</summary>

```bash
mkdir my-first-repo
cd my-first-repo
git init
echo "# Hello" > README.md
git add README.md
git commit -m "docs: add readme"
git log --oneline
```

**Пояснение:** `git add` кладёт файл в индекс (staging), `git commit` делает снимок.

</details>

<details>
<summary>Задача 2 — решение</summary>

```bash
git switch -c feature/hello
echo "hello" > hello.txt
git add hello.txt
git commit -m "feat: add hello file"
git switch main
git merge feature/hello
```

**Пояснение:** Ветка — изолированная линия; merge переносит коммиты в main.

</details>

<details>
<summary>Задача 3 — решение</summary>

```bash
# в main: создай файл и напиши "линия A"
git switch -c branch-b
# измени ту же строку на "линия B", закоммить
git switch main
git merge branch-b
# <<<<<<< HEAD ... ======= ... >>>>>>> branch-b
# оставь нужное, удали маркеры
git add conflicted.txt
git commit -m "merge: resolve conflict"
```

**Пояснение:** Конфликт решается смыслово, а не склейкой строк.

</details>

<details>
<summary>Задача 4 — решение</summary>

```bash
git revert <sha>    # новый коммит, убирающий изменения
git reset --soft HEAD~1  # локальный откат без потери изменений
```

**Пояснение:** `revert` безопасен для публичной истории; `reset` — только для локальных незапушенных коммитов.

</details>

<details>
<summary>Задача 5 — решение</summary>

```bash
git commit -m "feat(auth): add login endpoint"
git commit -m "fix(ui): correct button alignment"
git commit -m "docs: update getting started"
```

**Пояснение:** Согласованный формат упрощает changelog и историю.

</details>

<details>
<summary>Задача 6 — решение</summary>

```yaml
# .github/workflows/build.yml
- name: Build module 13
  run: dotnet build modules/13-unit-testing/xunit-demo/Calculator.slnx --configuration Release
- name: Test module 13
  run: dotnet test modules/13-unit-testing/xunit-demo/Calculator.slnx --configuration Release --no-build
```

**Пояснение:** `--configuration Release` — сборка как в проде; `--no-build` использует уже собранные бинарники, не пересобирая. CI собирает оба проекта и гоняет тесты на каждый push.

</details>
