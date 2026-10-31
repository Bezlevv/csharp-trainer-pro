# Разбор кода — Модуль 05

> Формат: рабочий, но плохой код → находим проблемы → рефакторим.

## Пример 1: Наследование ради переиспользования

### Исходный код

```csharp
class FileLogger
{
    public void Log(string message) => Console.WriteLine($"LOG: {message}");
}

class ReportGenerator : FileLogger
{
    public void Generate() => Log("Отчёт сгенерирован");
}
```

### Что не так

- `ReportGenerator` не «является» логгером (IS-A). Связь слабая, но наследование делает её намертво: любой наследник теперь «будет» логгером. Лучше — композиция.

### Как лучше

```csharp
class ReportGenerator
{
    private readonly FileLogger _logger = new();

    public void Generate() => _logger.Log("Отчёт сгенерирован");
}
```

### Вывод

Используй наследование только там, где есть настоящая связь «является». Для инструментов — композиция.

## Пример 2: `switch` по типу вместо виртуального метода

### Исходный код

```csharp
using PrintArea(Shape shape)
{
    if (shape is Circle c) return Math.PI * c.Radius * c.Radius;
    if (shape is Rectangle r) return r.Width * r.Height;
    if (shape is Triangle t) { /* формула Герона */ }
    return 0;
}
```

### Что не так

- Каждая новая фигура → правка `PrintArea`. Логика «размазана» по потребителям.

### Как лучше

```csharp
abstract class Shape
{
    public abstract double Area();
}
```

### Вывод

Классический случай OCP/полиморфизма: новое поведение добавляется новым классом, а не правкой старых.

## Пример 3: Забыт `virtual`/`override`

### Исходный код

```csharp
class Animal { public string Speak() => "..."; }
class Dog : Animal { public new string Speak() => "Гав!"; }

Animal a = new Dog();
Console.WriteLine(a.Speak()); // "..." — метод скрыт, а не переопределён
```

### Что не так

- `new` скрывает метод, но через базовую ссылку вызывает версия базового класса. Полиморфизм не работает.

### Как лучше

```csharp
class Animal { public virtual string Speak() => "..."; }
class Dog : Animal { public override string Speak() => "Гав!"; }
```

### Вывод

Используй `virtual`/`override` — тогда вызов пойдёт к фактической реализации.

## Пример 4: Жирный интерфейс

### Исходный код

```csharp
interface IWorker
{
    void Work();
    void Eat();
    void Sleep();
    void PrintReport();
    void SendEmail();
}
```

### Что не так

- Не все работники отправляют email или спят. Придётся реализовывать «вхолостую» все методы. Нарушение принципа разделения интерфейсов.

### Как лучше

```csharp
interface IWorker { void Work(); }
interface IReportable { void PrintReport(); }
interface INotifier { void SendEmail(); }
```

### Вывод

Интерфейсы дробятся по ролям (подробнее — модуль 14, ISP).

## Пример 5: `Equals` без `GetHashCode`

### Исходный код

```csharp
class Person
{
    public string Name { get; }
    public Person(string name) => Name = name;

    public override bool Equals(object? obj) =>
        obj is Person other && Name == other.Name;
    // GetHashCode не переопределён
}
```

### Что не так

- Объекты, равные по `Equals`, обязаны иметь одинаковый `GetHashCode`. Иначе поведение в `Dictionary`/`HashSet` сломается.

### Как лучше

```csharp
public override int GetHashCode() => Name.GetHashCode();
```

### Вывод

`Equals` и `GetHashCode` переопределяются парой (или просто используй `record` — он сделает это сам).
