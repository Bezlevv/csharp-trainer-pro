using TaskTracker.Cli;
using TaskTracker.Services;
using TaskTracker.Storage;

// Точка входа: собираем зависимости вручную (DI-демонстрация из модуля 14).
var storagePath = Path.Combine(Environment.CurrentDirectory, "data", "tasks.json");
var repository = new JsonTaskRepository(storagePath);
var service = new TaskService(repository);
var ui = new ConsoleUI(service);

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
};

await ui.RunAsync(cts.Token);
