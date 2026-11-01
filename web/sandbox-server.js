// Локальный сервер реальной песочницы C#.
// Запуск: node web/sandbox-server.js
// Открой в браузере: http://localhost:8091/
//
// POST /run  { code: "..." } → реально компилирует и выполняет код
// через установленный .NET SDK, возвращает stdout/stderr.

const http = require("http");
const fs = require("fs");
const path = require("path");
const { execFile } = require("child_process");

const WEB = __dirname; // .../web
const PORT = Number(process.env.PORT) || 8091;
const TMP = process.env.SANDBOX_TMP ||
  "C:\\Users\\mnttm\\AppData\\Local\\Temp\\opencode\\sandbox";
const DOTNET = "dotnet";

const EXT_TYPES = {
  ".html": "text/html; charset=utf-8",
  ".css": "text/css; charset=utf-8",
  ".js": "application/javascript; charset=utf-8"
};

function serveStatic(req, res) {
  let p = decodeURIComponent(req.url.split("?")[0]);
  if (p === "/") p = "/sandbox.html";
  const fp = path.join(WEB, p.replace(/^[/\\]+/, ""));
  if (!fp.startsWith(WEB)) { res.writeHead(403); res.end("Forbidden"); return; }
  fs.readFile(fp, (err, data) => {
    if (err) { res.writeHead(404); res.end("Not found: " + p); return; }
    res.writeHead(200, { "Content-Type": EXT_TYPES[path.extname(fp)] || "application/octet-stream" });
    res.end(data);
  });
}

function runCode(code, input, cb) {
  const id = Date.now().toString(36) + Math.random().toString(36).slice(2, 7);
  const dir = path.join(TMP, id);
  const outDir = path.join(dir, "out");
  fs.mkdirSync(dir, { recursive: true });

  fs.writeFileSync(path.join(dir, "Program.cs"), code);
  fs.writeFileSync(path.join(dir, "Run.csproj"),
    '<Project Sdk="Microsoft.NET.Sdk">' +
    "<PropertyGroup>" +
    "<OutputType>Exe</OutputType>" +
    "<TargetFramework>net8.0</TargetFramework>" +
    "<ImplicitUsings>enable</ImplicitUsings>" +
    "<Nullable>enable</Nullable>" +
    "</PropertyGroup></Project>");

  const start = Date.now();

  // 1) Сборка в отдельную папку out/
  execFile(DOTNET, ["build", dir, "-c", "Release", "-o", outDir, "--nologo"],
    { timeout: 45000, cwd: dir, windowsHide: true },
    (err, stdout, stderr) => {
      if (err) {
        fs.rmSync(dir, { recursive: true, force: true });
        return cb({ output: stdout + "\n" + stderr, error: true, ms: Date.now() - start });
      }
      // 2) Запуск exe напрямую — stdin пробрасывается надёжнее
      const exe = path.join(outDir, "Run.exe");
      const runOpts = { timeout: 45000, cwd: dir, windowsHide: true };
      if (input && input.length) {
        runOpts.input = input.endsWith("\n") ? input : input + "\n";
      }
      execFile(exe, [], runOpts, (err2, stdout2, stderr2) => {
        fs.rmSync(dir, { recursive: true, force: true });
        const ms = Date.now() - start;
        if (err2) return cb({ output: stdout2 + "\n" + stderr2, error: true, ms });
        cb({ output: stdout2, error: false, ms });
      });
    });
}

const server = http.createServer((req, res) => {
  if (req.method === "POST" && req.url === "/run") {
    let body = "";
    req.on("data", (c) => { body += c; });
    req.on("end", () => {
      let code = "", input = "";
      try {
        const parsed = JSON.parse(body || "{}");
        code = parsed.code || "";
        input = parsed.input || "";
      } catch { code = ""; }
      if (!code.trim()) {
        res.writeHead(400, { "Content-Type": "application/json" });
        res.end(JSON.stringify({ output: "Код пустой.", error: true }));
        return;
      }
      runCode(code, input, (result) => {
        res.writeHead(200, { "Content-Type": "application/json" });
        res.end(JSON.stringify(result));
      });
    });
    return;
  }
  serveStatic(req, res);
});

server.listen(PORT, () => {
  console.log("Песочница: http://localhost:" + PORT + "/");
});
