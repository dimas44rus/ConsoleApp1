using FitnessEf;

await using var db = new FitnessContext();

var client = new Client
{
    FirstName = "Дмитрий",
    LastName = "Соколов",
    BirthDate = new DateOnly(2000, 5, 15),
    Phone = "79995554433",
    Email = "sokolov@example.com",
    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
    Status = "active"
};

db.Clients.Add(client);

int saved = await db.SaveChangesAsync();

Console.WriteLine($"Сохранено изменений: {saved}");
Console.WriteLine($"ID нового клиента: {client.ClientId}");
