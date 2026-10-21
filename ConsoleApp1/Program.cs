using FitnessEf;

await using var db = new FitnessContext();

Console.Write("Имя: ");
string firstName = Console.ReadLine()?.Trim() ?? string.Empty;

Console.Write("Фамилия: ");
string lastName = Console.ReadLine()?.Trim() ?? string.Empty;

if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
{
    Console.WriteLine("Ошибка: Имя и фамилия обязательны.");
    return; 
}

var client = new Client
{
    FirstName = firstName,
    LastName = lastName,
    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
    Status = "active"
};

db.Clients.Add(client);
await db.SaveChangesAsync();

Console.WriteLine($"Клиент успешно добавлен! Присвоен ID: {client.ClientId}");
