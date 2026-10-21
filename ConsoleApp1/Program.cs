using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

var clients = await db.Clients
    .AsNoTracking()
    .Include(c => c.Visits)
    .OrderBy(c => c.ClientId)
    .ToListAsync();

Console.WriteLine("=== Список клиентов и их посещений ===");
foreach (var client in clients)
{
    Console.WriteLine($"{client.FirstName} {client.LastName} (ID: {client.ClientId})");

    foreach (var visit in client.Visits)
    {
        Console.WriteLine($"  -> {visit.VisitDate}: {visit.Note}");
    }
}
