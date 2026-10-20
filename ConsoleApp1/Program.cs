using FitnessEf;
using Microsoft.EntityFrameworkCore;

await using var db = new FitnessContext();

Console.Write("Фрагмент фамилии: ");
string term = Console.ReadLine()?.Trim() ?? string.Empty;

if (term.Length == 0)
{
    Console.WriteLine("Строка поиска не задана.");
}
else
{
    var found = await db.Clients
        .AsNoTracking()
        .Where(c => c.LastName.Contains(term))
        .OrderBy(c => c.LastName)
        .ToListAsync();

    Console.WriteLine("=== Результаты поиска ===");
    foreach (var c in found)
        Console.WriteLine($"{c.ClientId}: {c.FirstName} {c.LastName}");
}
