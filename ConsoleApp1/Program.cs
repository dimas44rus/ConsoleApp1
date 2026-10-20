using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

var activeClients = await db.Clients
    .AsNoTracking()
    .Where(c => c.Status == "active")
    .OrderBy(c => c.LastName)
    .ToListAsync();

Console.WriteLine("=== Активные клиенты ===");
foreach (var client in activeClients)
{
    Console.WriteLine($"{client.ClientId}: {client.FirstName} {client.LastName} (Статус: {client.Status})");
}
