using FitnessEf;
using Microsoft.EntityFrameworkCore;

try
{
    await using var db = new FitnessContext();

    Console.WriteLine("=== Диагностика SQL-запроса (ToQueryString) ===");

    var query = db.Clients
        .Where(c => c.Status == "active")
        .OrderBy(c => c.LastName);

    Console.WriteLine(query.ToQueryString());
    Console.WriteLine();

    Console.WriteLine("=== Выполнение асинхронного нетрекаемого запроса ===");

    var activeClients = await query.AsNoTracking().ToListAsync();
    Console.WriteLine($"Успешно загружено активных клиентов: {activeClients.Count}");

    foreach (var client in activeClients)
    {
        Console.WriteLine($"- {client.FirstName} {client.LastName} (ID: {client.ClientId})");
    }
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Ошибка конфигурации контекста: {ex.Message}");
}
catch (DbUpdateException ex)
{
    Console.WriteLine("Ошибка обновления или сохранения данных в PostgreSQL.");
    Console.WriteLine($"Детали СУБД: {ex.GetBaseException().Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"Произошла непредвиденная системная ошибка: {ex.Message}");
}

Console.ReadLine();
