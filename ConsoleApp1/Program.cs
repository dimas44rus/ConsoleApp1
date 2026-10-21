using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

int id = 1;
var client = await db.Clients.FirstOrDefaultAsync(c => c.ClientId == id);

if (client is null)
{
    Console.WriteLine("Клиент не найден.");
    return;
}

client.Phone = "79990001122";

await db.SaveChangesAsync();
Console.WriteLine("Телефон успешно изменён в базе данных PostgreSQL.");
