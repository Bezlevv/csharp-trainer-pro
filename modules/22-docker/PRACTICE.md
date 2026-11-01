# Практика — Модуль 22: Docker

## Задача 1: Простой Dockerfile

**Сложность:** ★★☆
**Описание:** Напиши Dockerfile для консольного .NET-приложения.

**Требования:**
- Двухступенчатый: `sdk` → build, `aspnet` (или `runtime`) → run.
- `ENTRYPOINT` на скомпилированную DLL.

## Задача 2: Запуск

**Сложность:** ★☆☆
**Описание:** Собери образ и запусти контейнер.

**Требования:**
- `docker build -t myapp .`
- `docker run myapp`

## Задача 3: Порт

**Сложность:** ★☆☆
**Описание:** Запусти веб-API с пробросом порта.

**Требования:**
- `docker run -p 8080:8080 my-api`
- Проверь `curl http://localhost:8080/health`

## Задача 4: Переменные окружения

**Сложность:** ★★☆
**Описание:** Передай `JWT_SECRET` при запуске.

**Требования:**
- `docker run -e "JWT_SECRET=super-secret" ...`

## Задача 5: docker-compose

**Сложность:** ★★★
**Описание:** Опиши `docker-compose.yml` с API и PostgreSQL.

**Требования:**
- Сервисы `api` и `db`.
- Порты и переменные окружения.

## Решения

<details>
<summary>Задача 1 — решение</summary>

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet publish src/MyApp/MyApp.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyApp.dll"]
```
</details>

<details>
<summary>Задача 2 — решение</summary>

```bash
docker build -t myapp .
docker run myapp
```
</details>

<details>
<summary>Задача 3 — решение</summary>

```bash
docker run -p 8080:8080 my-api
curl http://localhost:8080/health
```
</details>

<details>
<summary>Задача 4 — решение</summary>

```bash
docker run -e "JWT_SECRET=super-secret" my-api
```
</details>

<details>
<summary>Задача 5 — решение</summary>

```yaml
services:
  api:
    build: .
    ports: ["8080:8080"]
    environment:
      JWT_SECRET: super-secret
  db:
    image: postgres:16
    environment:
      POSTGRES_PASSWORD: secret
```
</details>
