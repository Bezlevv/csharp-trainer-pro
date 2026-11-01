@echo off
chcp 65001 >nul
cd /d "%~dp0"

where node >nul 2>nul
if errorlevel 1 (
  echo [ОШИБКА] Node.js не найден.
  echo Установи его с https://nodejs.org и повтори.
  pause
  exit /b 1
)

start "" http://localhost:8091/
echo Запускаю песочницу... Открой http://localhost:8091/
node server.js
pause
