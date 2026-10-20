using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

int total = await db.Clients.CountAsync();

bool hasInactive = await db.Clients.AnyAsync(c => c.Status == "inactive");

int activeCount = await db.Clients.CountAsync(c => c.Status == "active");

Console.WriteLine($"Всего клиентов: {total}");
Console.WriteLine($"Активных клиентов: {activeCount}");
Console.WriteLine(hasInactive
    ? "Есть неактивные клиенты."
    : "Неактивные клиенты отсутствуют.");
