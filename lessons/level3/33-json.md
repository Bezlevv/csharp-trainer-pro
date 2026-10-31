# Урок 33. JSON

## Цель
Понять, как превращать объекты в текст (для сохранения/передачи) и обратно.

## Словарик новых слов
- **JSON** — текстовый формат для хранения данных. Читается и людьми, и программами.
- **Сериализация** — превращение объекта в текст (JSON).
- **Десериализация** — превращение текста (JSON) обратно в объект.
- **`JsonSerializer`** — встроенный в .NET помощник для этого.

## Теория «с нуля»

### Аналогия: анкета
Анкета в бумажном виде — это «JSON»: набор полей и значений. Ты заполняешь анкету (объект), потом её можно отправить по факсу (сериализация в текст), а получатель расшифрует (десериализация) и снова увидит аккуратные поля.

### Как выглядит JSON
```json
{
  "name": "Иван",
  "age": 30
}
```

### Сериализация объекта в JSON
```csharp
record Person(string Name, int Age);

Person p = new Person("Иван", 30);
string json = JsonSerializer.Serialize(p);
Console.WriteLine(json);   // {"Name":"Иван","Age":30}
```

- `record` — компактный тип для данных (из урока про продвинутые типы).
- `JsonSerializer.Serialize(obj)` — объект → строка.

### Десериализация JSON в объект
```csharp
string json = "{\"Name\":\"Иван\",\"Age\":30}";
Person? p = JsonSerializer.Deserialize<Person>(json);
Console.WriteLine(p!.Name);   // Иван
```

- `Deserialize<Person>(json)` — строка → объект типа Person.
- `?` и `!` — про возможность null (модуль 04).

### Список объектов
```csharp
List<Person> people = new List<Person> { new("Иван", 30), new("Мария", 28) };
string json = JsonSerializer.Serialize(people);          // в строку
List<Person>? back = JsonSerializer.Deserialize<List<Person>>(json);
```

### Красивый вывод
```csharp
string pretty = JsonSerializer.Serialize(p, new JsonSerializerOptions { WriteIndented = true });
Console.WriteLine(pretty);
```
`WriteIndented = true` — делает JSON читаемым («с отступами»).

### JSON + файлы (связка с уроком 32)
```csharp
// сохранить объект в файл
File.WriteAllText("person.json", JsonSerializer.Serialize(p));

// прочитать из файла
Person? loaded = JsonSerializer.Deserialize<Person>(File.ReadAllText("person.json"));
```

## Примеры

```csharp
record Product(string Name, decimal Price);

Product phone = new Product("Phone", 499.99m);
File.WriteAllText("product.json", JsonSerializer.Serialize(phone, new JsonSerializerOptions { WriteIndented = true }));

Product? loaded = JsonSerializer.Deserialize<Product>(File.ReadAllText("product.json"));
Console.WriteLine(loaded!.Name);
```

## Сделай сам (задачи с подсказками)

1. Создай `record Student(string Name, int Age)` и сериализуй объект в строку.
   *Подсказка: `JsonSerializer.Serialize(student)`.*
2. Десериализуй строку `{"Name":"Аня","Age":20}` обратно в Student.
   *Подсказка: `JsonSerializer.Deserialize<Student>(json)`.*
3. Сохрани список студентов в файл и прочитай обратно.
   *Подсказка: `WriteAllText` + `Deserialize<List<Student>>`.*
4. Выведи JSON красиво (с отступами).
   *Подсказка: `WriteIndented = true`.*

## Разбор типичных ошибок

| Ошибка | Что не так | Как правильно |
|---|---|---|
| `Deserialize<Person>` без проверки на null | может вернуть null | используй `?` и проверяй |
| JSON без кавычек у полей | невалидный JSON | поля в кавычках |
| Забыл `using System.Text.Json;` | JsonSerializer не найден | добавь using |
| Чтение несуществующего файла JSON | исключение | проверяй File.Exists |

## Тест — проверь себя

**Вопрос 1.** Что такое сериализация?
- A) Превращение объекта в текст (JSON).
- B) Превращение текста в объект.
- C) Удаление объекта.

✅ **Верно: A.**
Почему: сериализация — объект → текст.
Почему неверны остальные:
- B — Это десериализация.
- C — Это не удаление.

**Вопрос 2.** Что делает `JsonSerializer.Deserialize<Person>(json)`?
- A) Превращает Person в строку.
- B) Превращает строку JSON в объект Person.
- C) Печатает JSON.

✅ **Верно: B.**
Почему: Deserialize — текст → объект.
Почему неверны остальные:
- A — Это Serialize.
- C — Печать — не задача Deserialize.

**Вопрос 3.** Зачем `WriteIndented = true`?
- A) Чтобы JSON стал меньше.
- B) Чтобы JSON был читаемым, с отступами.
- C) Чтобы JSON ускорился.

✅ **Верно: B.**
Почему: отступы делают JSON наглядным.
Почему неверны остальные:
- A — Отступы, наоборот, увеличивают размер, а не уменьшают.
- C — Это не про скорость.

**Вопрос 4.** Как сохранить объект в файл как JSON?
- A) `File.WriteAllText("p.json", JsonSerializer.Serialize(p));`
- B) `File.ReadAllText("p.json");`
- C) `JsonSerializer.Deserialize<Person>(json);`

✅ **Верно: A.**
Почему: сериализуем и записываем текст в файл.
Почему неверны остальные:
- B — ReadAllText — чтение.
- C — Deserialize — разбор строки, а не запись файла.

## Итог урока
- JSON — текстовый формат данных.
- `Serialize` — объект → JSON; `Deserialize` — JSON → объект.
- `WriteIndented` — красивый вывод.
- JSON легко сочетается с файлами.
