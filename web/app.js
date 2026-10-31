// C# Trainer Pro — интерактивный курс
// Загружает markdown-модули из репозитория и рендерит их (marked + highlight.js).

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

const TABS = [
  { id: "teoria",   label: "Теория",        file: "README.md" },
  { id: "praktika", label: "Практика",      file: "PRACTICE.md" },
  { id: "razbor",   label: "Разбор кода",   file: "CODE_REVIEW.md" }
];

const state = {
  index: null,
  tab: "teoria",
  cache: {} // slug -> { file: html }
};

marked.setOptions({ gfm: true, breaks: true });

const $ = (sel) => document.querySelector(sel);

function renderMenu() {
  const menu = $("#menu");
  menu.innerHTML = "";
  MODULES.forEach((m, i) => {
    const btn = document.createElement("button");
    btn.textContent = m.title;
    btn.addEventListener("click", () => openModule(i));
    btn.dataset.index = i;
    menu.appendChild(btn);
  });
}

function updateMenuActive() {
  document.querySelectorAll("#menu button").forEach((b) => {
    b.classList.toggle("active", Number(b.dataset.index) === state.index);
  });
}

async function loadModule(slug, file) {
  if (state.cache[slug] && state.cache[slug][file]) {
    return state.cache[slug][file];
  }
  const url = `/modules/${slug}/${file}`;
  const res = await fetch(url);
  if (!res.ok) {
    throw new Error(`Не удалось загрузить ${url} (HTTP ${res.status})`);
  }
  const md = await res.text();
  const html = marked.parse(md);
  if (!state.cache[slug]) state.cache[slug] = {};
  state.cache[slug][file] = html;
  return html;
}

async function renderView() {
  const mod = MODULES[state.index];
  const view = $("#view");
  view.innerHTML = `<div class="placeholder">Загрузка…</div>`;

  try {
    const html = await loadModule(mod.slug, TABS.find(t => t.id === state.tab).file);
    const tabsHtml = `<div class="tabs">${TABS.map(t =>
      `<button data-tab="${t.id}" class="${t.id === state.tab ? "active" : ""}">${t.label}</button>`
    ).join("")}</div>`;

    const prev = state.index > 0
      ? `<a href="#" data-nav="${state.index - 1}">← ${MODULES[state.index - 1].title}</a>`
      : `<span></span>`;
    const next = state.index < MODULES.length - 1
      ? `<a href="#" data-nav="${state.index + 1}">${MODULES[state.index + 1].title} →</a>`
      : `<span></span>`;

    view.innerHTML = tabsHtml +
      `<div class="content">${html}</div>` +
      `<div class="nav-bottom">${prev}${next}</div>`;

    // подсветка кода
    view.querySelectorAll("pre code").forEach((el) => {
      try { hljs.highlightElement(el); } catch (_) { /* если hljs недоступен */ }
    });

    // вкладки
    view.querySelectorAll(".tabs button").forEach((b) => {
      b.addEventListener("click", () => {
        state.tab = b.dataset.tab;
        renderView();
      });
    });

    // перехват относительных ссылок на .md внутри модуля → переключение вкладки
    view.querySelectorAll(".content a").forEach((a) => {
      const href = a.getAttribute("href") || "";
      if (/^(https?:|mailto:|#|\/|\.\.)/.test(href)) return;
      const file = href.split("/").pop().split("?")[0];
      const tab = TABS.find((t) => t.file === file);
      if (!tab) return;
      a.addEventListener("click", (e) => {
        e.preventDefault();
        state.tab = tab.id;
        renderView();
      });
    });

    // навигация «предыдущий/следующий»
    view.querySelectorAll("[data-nav]").forEach((a) => {
      a.addEventListener("click", (e) => {
        e.preventDefault();
        openModule(Number(a.dataset.nav));
      });
    });
  } catch (err) {
    view.innerHTML = `<div class="error-box">Ошибка: ${escapeHtml(err.message)}<br><small>Убедись, что сайт открыт через локальный сервер (не file://).</small></div>`;
  }
}

function openModule(index) {
  state.index = index;
  state.tab = "teoria";
  updateMenuActive();
  renderView();
}

function escapeHtml(s) {
  return s.replace(/[&<>"']/g, (c) =>
    ({ "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;" }[c]));
}

renderMenu();
openModule(0);
