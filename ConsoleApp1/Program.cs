using FitnessEf;

await using var db = new FitnessContext();

int id = 1;
var client = await db.Clients.FindAsync(id);

if (client is null)
{
    Console.WriteLine("Клиент не найден.");
    return;
}

db.Clients.Remove(client);

await db.SaveChangesAsync();
Console.WriteLine("Клиент успешно удалён из базы данных.");
