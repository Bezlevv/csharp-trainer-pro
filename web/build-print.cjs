// Сборка печатной HTML-версии курса: web/course-print.html
// Запуск: node build-print.cjs
const fs = require("fs");
const path = require("path");

const REPO = "D:\\OpenCode\\C#_trainer_PRO";
const TMP = "C:\\Users\\mnttm\\AppData\\Local\\Temp\\opencode";
const WEB = path.join(REPO, "web");

const markedLib = require(path.join(TMP, "marked.min.js"));
const parse = markedLib.parse.bind(markedLib);
const renderMd = (md) => parse(md, { gfm: true, breaks: true });

const MODULES = [
  "00-intro","01-basics","02-control-flow","03-methods","04-oop-basics",
  "05-inheritance","06-advanced-types","07-generics-collections","08-exceptions",
  "09-delegates-events","10-linq","11-async","12-files-json","13-unit-testing",
  "14-solid-patterns","15-final-project","16-git","17-aspnet","18-databases",
  "19-architecture","20-observability","21-security"
];
const LEVELS = ["level0","level1","level2","level3","level4"];

const out = [];

out.push("<html><head><meta charset='utf-8'><title>Курс C# — печатная версия</title>");
out.push(`<style>
@page{margin:18mm}
body{font-family:'Segoe UI',system-ui,Roboto,sans-serif;font-size:11pt;color:#111;line-height:1.5;max-width:190mm;margin:0 auto}
h1{font-size:19pt;color:#1e3a8a;border-bottom:2px solid #1e3a8a;padding-bottom:4px;page-break-before:always}
h1:first-child{page-break-before:avoid}
h2{font-size:15pt;color:#1d4ed8;margin-top:18px}
h3{font-size:13pt}
pre{background:#f1f5f9;padding:8px;border-radius:6px;font-family:Consolas,monospace;font-size:9.5pt;overflow-wrap:break-word;white-space:pre-wrap}
code{background:#f1f5f9;padding:1px 4px;border-radius:4px;font-family:Consolas,monospace}
table{border-collapse:collapse;width:100%;margin:8px 0}
th,td{border:1px solid #cbd5e1;padding:5px 7px;text-align:left;font-size:10pt}
th{background:#eef2ff}
blockquote{border-left:4px solid #bfdbfe;margin:6px 0;padding:2px 12px;color:#334155;background:#f0f6ff}
details{background:#f8fafc;border:1px solid #e2e8f0;border-radius:6px;padding:6px 10px;margin:6px 0;page-break-inside:avoid}
summary{font-weight:600;cursor:pointer}
details[open] summary{margin-bottom:6px}
.pagebreak{page-break-before:always}
</style></head><body>`);

function addMd(md) {
  const html = renderMd(md);
  out.push(html);
}

// 1) План
out.push("<h1>План развития: от 0 до Senior</h1>");
addMd(fs.readFileSync(path.join(REPO, "ROADMAP.md"), "utf8"));

// 2) Уроки уровней
LEVELS.forEach((lv) => {
  const dir = path.join(REPO, "lessons", lv);
  let files = [];
  try { files = fs.readdirSync(dir).filter((f) => f.endsWith(".md") && f !== "README.md"); } catch { files = []; }
  files.sort();
  files.forEach((f) => {
    const md = fs.readFileSync(path.join(dir, f), "utf8");
    // вставляем разрыв страницы перед каждым уроком
    out.push('<div class="pagebreak"></div>');
    addMd(md);
  });
});

// 3) Модули (теория + практика + разбор)
MODULES.forEach((slug) => {
  const base = path.join(REPO, "modules", slug);
  const read = (f) => {
    try { return fs.readFileSync(path.join(base, f), "utf8"); } catch { return ""; }
  };
  out.push('<div class="pagebreak"></div>');
  const theory = read("README.md");
  if (theory) addMd(theory);
  const practice = read("PRACTICE.md");
  if (practice) {
    out.push('<h1>Практика</h1>');
    addMd(practice);
  }
  const review = read("CODE_REVIEW.md");
  if (review) {
    out.push('<h1>Разбор кода</h1>');
    addMd(review);
  }
});

out.push("</body></html>");

const fp = path.join(WEB, "course-print.html");
fs.writeFileSync(fp, out.join("\n"), "utf8");
console.log("OK: " + fp);
console.log("Размер: " + (fs.statSync(fp).size / 1024).toFixed(0) + " КБ");
