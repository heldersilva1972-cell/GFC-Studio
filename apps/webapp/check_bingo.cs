using System;
using System.Linq;
using System.Threading.Tasks;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) => {
        services.AddDbContext<GfcDbContext>(options =>
            options.UseSqlServer("Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"));
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GfcDbContext>();
    var count = await db.BingoSessions.CountAsync();
    Console.WriteLine($"Total Bingo Sessions: {count}");
    
    var latest = await db.BingoSessions.OrderByDescending(s => s.SessionDate).Take(5).ToListAsync();
    foreach (var s in latest)
    {
        Console.WriteLine($"ID: {s.Id}, Date: {s.SessionDate:yyyy-MM-dd}, Status: {s.Status}, Deleted: {s.IsDeleted}");
    }
}
