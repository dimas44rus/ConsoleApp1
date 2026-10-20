using FitnessEf;

await using var db = new FitnessContext();

Console.Write("ID клиента: ");
if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
{
    var client = await db.Clients.FindAsync(id);

    if (client is null)
        Console.WriteLine("Клиент не найден.");
    else
        Console.WriteLine($"{client.ClientId}: {client.FirstName} {client.LastName}");
}
else
{
    Console.WriteLine("ID должен быть положительным целым числом.");
}
