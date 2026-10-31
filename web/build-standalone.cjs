// Пересборка автономной версии курса: web/course-standalone.html
// Запуск: node web/build-standalone.cjs  (из корня репозитория)
// Требуется только Node.js (без интернета). Библиотеки — в web/vendor.

const fs = require("fs");
const path = require("path");

const WEB = __dirname;                       // .../web
const REPO = path.join(WEB, "..");           // корень репозитория
const VENDOR = path.join(WEB, "vendor");

const MODULES = [
  { slug: "00-intro",               title: "00 · Введение в C# и .NET" },
  { slug: "01-basics",              title: "01 · Основы синтаксиса" },
  { slug: "02-control-flow",        title: "02 · Управление потоком" },
  { slug: "03-methods",             title: "03 · Методы" },
  { slug: "04-oop-basics",          title: "04 · ООП: классы и инкапсуляция" },
  { slug: "05-inheritance",         title: "05 · Наследование и полиморфизм" },
  { slug: "06-advanced-types",      title: "06 · Продвинутые типы" },
  { slug: "07-generics-collections",title: "07 · Обобщения и коллекции" },
  { slug: "08-exceptions",          title: "08 · Исключения" },
  { slug: "09-delegates-events",    title: "09 · Делегаты, лямбды, события" },
  { slug: "10-linq",                title: "10 · LINQ" },
  { slug: "11-async",               title: "11 · Асинхронность" },
  { slug: "12-files-json",          title: "12 · Файлы, даты, JSON" },
  { slug: "13-unit-testing",        title: "13 · Юнит-тесты" },
  { slug: "14-solid-patterns",      title: "14 · SOLID и паттерны" },
  { slug: "15-final-project",       title: "15 · Итоговый проект" },
  { slug: "16-git",                 title: "16 · Git и рабочий процесс" },
  { slug: "17-aspnet",              title: "17 · Введение в ASP.NET Core" },
  { slug: "18-databases",           title: "18 · Базы данных и EF Core" },
  { slug: "19-architecture",        title: "19 · Продвинутая архитектура" },
  { slug: "20-observability",       title: "20 · Наблюдаемость" },
  { slug: "21-security",            title: "21 · Безопасность веб-приложений" }
];

const FILES = { teoria: "README.md", praktika: "PRACTICE.md", razbor: "CODE_REVIEW.md" };

const markedLib = require(path.join(VENDOR, "marked.min.js"));
const parse = markedLib.parse.bind(markedLib);

const data = {};
for (const m of MODULES) {
  data[m.slug] = {};
  for (const key of Object.keys(FILES)) {
    const fp = path.join(REPO, "modules", m.slug, FILES[key]);
    let md = "";
    try { md = fs.readFileSync(fp, "utf8"); } catch { md = `# Материал не найден\n\nОтсутствует \`${FILES[key]}\` для модуля ${m.slug}.`; }
    data[m.slug][key] = parse(md, { gfm: true, breaks: true });
  }
}

const css = fs.readFileSync(path.join(WEB, "style.css"), "utf8");
const themeCss = fs.readFileSync(path.join(VENDOR, "theme-github.css"), "utf8");
const hljsJs = fs.readFileSync(path.join(VENDOR, "highlight.min.js"), "utf8");
const appJs = fs.readFileSync(path.join(WEB, "app-inline.js"), "utf8");

const dataB64 = Buffer.from(JSON.stringify(data), "utf8").toString("base64");
const modulesJson = JSON.stringify(MODULES);

const html = `<!DOCTYPE html>
<html lang="ru">
<head>
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" />
<title>C# Trainer Pro — автономная версия курса</title>
<style>${css}

${themeCss}</style>
<script>${hljsJs}</script>
</head>
<body>
<header class="topbar"><div class="brand">🚀 C# Trainer Pro</div><div class="top-actions"><span class="muted">автономная версия · ${MODULES.length} модуля</span></div></header>
<div class="layout">
  <aside class="sidebar"><div class="sidebar-title">Модули</div><nav id="menu"></nav></aside>
  <main class="main"><div id="view" class="view"><div class="placeholder">Загрузка…</div></div></main>
</div>
<script>
window.COURSE_MODULES = ${modulesJson};
window.COURSE_DATA = (function () {
  var b64 = ${JSON.stringify(dataB64)};
  var bin = atob(b64);
  var bytes = new Uint8Array(bin.length);
  for (var i = 0; i < bin.length; i++) bytes[i] = bin.charCodeAt(i);
  return JSON.parse(new TextDecoder("utf-8").decode(bytes));
})();
</script>
<script>${appJs}</script>
</body>
</html>
`;

const out = path.join(WEB, "course-standalone.html");
fs.writeFileSync(out, html, "utf8");
console.log("OK: " + out);
console.log("Размер: " + (fs.statSync(out).size / 1024).toFixed(1) + " КБ");
