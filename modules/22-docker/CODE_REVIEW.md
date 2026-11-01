# Разбор кода — Модуль 22: Docker

## Пример 1: Огромный образ

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
COPY . .
RUN dotnet publish -c Release -o /app
ENTRYPOINT ["dotnet", "/app/TaskApi.dll"]
```

**Проблема:** SDK-образ весит гигабайты — в боевом образе он не нужен.
**Как надо:** многоступенчатая сборка: `sdk` для сборки, `aspnet/runtime` для запуска.

## Пример 2: Секреты в Dockerfile

```dockerfile
ENV JWT_SECRET=hardcoded-secret
```

**Проблема:** секрет попадёт в образ и историю.
**Как надо:** передавать при запуске: `docker run -e "JWT_SECRET=..."`.

## Пример 3: Забыли EXPOSE

**Проблема:** `-p 8080:8080` не пробросит порт, если он не задекларирован.
**Как надо:** указать `EXPOSE 8080` в Dockerfile.

## Пример 4: Порядок COPY и кэш

```dockerfile
COPY . .
COPY src/MyApp/*.csproj .
RUN dotnet restore
```

**Проблема:** копируем всё до restore → кэш не работает, restore на каждом билде.
**Как надо:** сначала скопировать `.csproj`, сделать restore, потом остальное — кэш слоёв работает.

## Вывод
Docker — про небольшие образы (многоступенчатость), безопасность (секреты вне образа) и правильный порядок для кэша.
