using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

var client = new Client
{
    FirstName = "Тест",
    LastName = "Ошибки",
    RegistrationDate = DateOnly.FromDateTime(DateTime.Today),
    Status = "very_long_status_name_that_exceeds_twenty_characters"
};

try
{
    db.Clients.Add(client);
    await db.SaveChangesAsync();
    Console.WriteLine($"Клиент добавлен. ID: {client.ClientId}");
}
catch (DbUpdateException ex)
{
    Console.WriteLine("Не удалось сохранить клиента. Проверьте данные и ограничения БД.");
    Console.WriteLine($"Детали ошибки: {ex.GetBaseException().Message}");
}
