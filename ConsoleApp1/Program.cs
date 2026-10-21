using FitnessEf;

await using var db = new FitnessContext();

Console.WriteLine("Подготовка пакета действий перед сохранением...");

db.Clients.Add(new Client
{
    FirstName = "Анна",
    LastName = "Смирнова",
    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
    Status = "active"
});

var existing = await db.Clients.FindAsync(2);
if (existing is not null)
{
    existing.Status = "inactive";
    Console.WriteLine($"Статус существующего клиента с ID = {existing.ClientId} изменен в памяти на 'inactive'.");
}

Console.WriteLine("Отправка пакета изменений в базу данных...");
int affectedRows = await db.SaveChangesAsync();

Console.WriteLine($"Пакет успешно применен! Количество измененных строк в БД: {affectedRows}");
