using FitnessEf;
using Microsoft.EntityFrameworkCore;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("=== УПРАВЛЕНИЕ КЛИЕНТАМИ ФИТНЕС-КЛУБА ===");
    Console.WriteLine("1. Список клиентов");
    Console.WriteLine("2. Найти клиента по ID или Фамилии");
    Console.WriteLine("3. Добавить клиента");
    Console.WriteLine("4. Изменить телефон или статус");
    Console.WriteLine("5. Удалить клиента");
    Console.WriteLine("6. Статистика");
    Console.WriteLine("0. Выход");
    Console.Write("Выбор: ");

    string choice = Console.ReadLine() ?? string.Empty;
    if (choice == "0") break;

    try
    {
        switch (choice)
        {
            case "1": await ListClientsAsync(); break;
            case "2": await FindClientAsync(); break;
            case "3": await AddClientAsync(); break;
            case "4": await UpdateClientAsync(); break;
            case "5": await DeleteClientAsync(); break;
            case "6": await ShowStatisticsAsync(); break;
            default: Console.WriteLine("Неизвестный пункт меню."); break;
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Критический сбой при выполнении операции: {ex.Message}");
    }
}

async Task ListClientsAsync()
{
    await using var db = new FitnessContext();
    var clients = await db.Clients
        .AsNoTracking()
        .OrderBy(c => c.LastName)
        .ThenBy(c => c.FirstName)
        .ToListAsync();

    Console.WriteLine("\n--- Список клиентов ---");
    if (!clients.Any()) Console.WriteLine("База данных пуста.");

    foreach (var c in clients)
    {
        Console.WriteLine($"ID: {c.ClientId} | {c.LastName} {c.FirstName} | Тел: {c.Phone ?? "не указан"} | Статус: {c.Status}");
    }
}

async Task FindClientAsync()
{
    await using var db = new FitnessContext();
    Console.WriteLine("\nВыберите тип поиска: 1 - По ID, 2 - По фрагменту фамилии");
    Console.Write("Выбор: ");
    string searchType = Console.ReadLine() ?? string.Empty;

    if (searchType == "1")
    {
        Console.Write("Введите ID клиента: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            Console.WriteLine("Ошибка: Неверный формат ID.");
            return;
        }

        var client = await db.Clients.AsNoTracking().FirstOrDefaultAsync(c => c.ClientId == id);
        if (client is null)
            Console.WriteLine("Клиент с таким ID не найден.");
        else
            Console.WriteLine($"Найден: ID {client.ClientId} - {client.LastName} {client.FirstName} (Статус: {client.Status})");
    }
    else if (searchType == "2")
    {
        Console.Write("Введите фрагмент фамилии: ");
        string term = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(term))
        {
            Console.WriteLine("Ошибка: Строка поиска не должна быть пустой.");
            return;
        }

        var found = await db.Clients
            .AsNoTracking()
            .Where(c => c.LastName.Contains(term))
            .OrderBy(c => c.LastName)
            .ToListAsync();

        Console.WriteLine("--- Результаты поиска ---");
        if (!found.Any()) Console.WriteLine("Совпадений не найдено.");
        foreach (var c in found) Console.WriteLine($"ID: {c.ClientId} - {c.LastName} {c.FirstName}");
    }
    else
    {
        Console.WriteLine("Неверный выбор.");
    }
}

async Task AddClientAsync()
{
    await using var db = new FitnessContext();
    Console.Write("\nВведите имя: ");
    string firstName = Console.ReadLine()?.Trim() ?? string.Empty;
    Console.Write("Введите фамилию: ");
    string lastName = Console.ReadLine()?.Trim() ?? string.Empty;
    Console.Write("Введите телефон (необязательно): ");
    string? phone = Console.ReadLine()?.Trim();
    if (string.IsNullOrEmpty(phone)) phone = null;

    if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
    {
        Console.WriteLine("Ошибка: Имя и фамилия являются обязательными для заполнения!");
        return;
    }

    var client = new Client
    {
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
        Status = "active"
    };

    db.Clients.Add(client);
    await db.SaveChangesAsync();
    Console.WriteLine($"Клиент успешно добавлен! Присвоен ID: {client.ClientId}");
}

async Task UpdateClientAsync()
{
    await using var db = new FitnessContext();
    Console.Write("\nВведите ID клиента для редактирования: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: Неверный формат ID.");
        return;
    }

    var client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId == id);
    if (client is null)
    {
        Console.WriteLine("Клиент не найден.");
        return;
    }

    Console.WriteLine($"Редактируем: {client.LastName} {client.FirstName}");
    Console.WriteLine("Что изменить? 1 - Телефон, 2 - Статус");
    string op = Console.ReadLine() ?? string.Empty;

    if (op == "1")
    {
        Console.Write("Новый телефон: ");
        client.Phone = Console.ReadLine()?.Trim();
    }
    else if (op == "2")
    {
        Console.Write("Новый статус (например, active/inactive): ");
        string status = Console.ReadLine()?.Trim() ?? string.Empty;
        if (!string.IsNullOrWhiteSpace(status)) client.Status = status;
    }
    else
    {
        Console.WriteLine("Неверная операция.");
        return;
    }

    await db.SaveChangesAsync();
    Console.WriteLine("Данные клиента успешно обновлены в базе.");
}

async Task DeleteClientAsync()
{
    await using var db = new FitnessContext();
    Console.Write("\nВведите ID клиента для удаления: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Ошибка: Неверный формат ID.");
        return;
    }

    var client = await db.Clients.FindAsync(id);
    if (client is null)
    {
        Console.WriteLine("Клиент не найден.");
        return;
    }

    try
    {
        db.Clients.Remove(client);
        await db.SaveChangesAsync();
        Console.WriteLine("Клиент успешно удален из базы данных.");
    }
    catch (DbUpdateException ex)
    {
        Console.WriteLine("\n[Ошибка удаления]: Невозможно удалить клиента.");
        Console.WriteLine("Причина: У клиента есть связанные посещения (сработало ограничение базы данных).");
        Console.WriteLine($"Детали СУБД: {ex.GetBaseException().Message}");
    }
}

async Task ShowStatisticsAsync()
{
    await using var db = new FitnessContext();
    int total = await db.Clients.CountAsync();
    int activeCount = await db.Clients.CountAsync(c => c.Status == "active");

    Console.WriteLine("\n--- Статистика базы данных ---");
    Console.WriteLine($"Общее количество клиентов: {total}");
    Console.WriteLine($"Количество активных клиентов: {activeCount}");
}
