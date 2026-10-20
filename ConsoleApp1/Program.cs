using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

int page = 1;     
int pageSize = 5;  

var pageOfClients = await db.Clients
    .AsNoTracking()
    .OrderBy(c => c.ClientId)
    .Skip((page - 1) * pageSize) 
    .Take(pageSize)               
    .ToListAsync();

Console.WriteLine($"=== Страница {page} (размер порции: {pageSize}) ===");
foreach (var client in pageOfClients)
{
    Console.WriteLine($"{client.ClientId}: {client.FirstName} {client.LastName}");
}
