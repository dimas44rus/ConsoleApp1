using FitnessEf;
using Microsoft.EntityFrameworkCore;



await using var db = new FitnessContext();

int clientId = 2;
var clientExists = await db.Clients.AnyAsync(c => c.ClientId == clientId);

if (!clientExists)
{
    Console.WriteLine("Клиента с таким ID нет.");
    return;
}

db.Visits.Add(new FitnessEf.Visit
{
    ClientId = clientId,
    VisitDate = DateOnly.FromDateTime(DateTime.Today),
    Note = "Первичное Задание"
});


await db.SaveChangesAsync();
Console.WriteLine($"Запись о посещении для клиента с ID = {clientId} успешно сохранена!");
