using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

Console.WriteLine("Запрос клиентов без указанного Email...");

var clientsWithoutEmail = await db.Clients
    .AsNoTracking()
    .Where(c => c.Email == null)
    .OrderBy(c => c.ClientId)
    .ToListAsync();

Console.WriteLine("=== Список клиентов без Email ===");
foreach (var client in clientsWithoutEmail)
{
    Console.WriteLine($"{client.ClientId}: {client.FirstName} {client.LastName} (Телефон: {client.Phone ?? "не указан"})");
}
