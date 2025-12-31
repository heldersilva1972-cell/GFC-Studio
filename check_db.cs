using System;
using System.Linq;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("GFC");

var serviceCollection = new ServiceCollection();
serviceCollection.AddDbContext<GfcDbContext>(options =>
    options.UseSqlServer(connectionString));

var serviceProvider = serviceCollection.BuildServiceProvider();

using var scope = serviceProvider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<GfcDbContext>();

Console.WriteLine("Fetching Controllers from DB...");
var controllers = db.Controllers.ToList();

if (!controllers.Any())
{
    Console.WriteLine("No controllers found in database.");
}
else
{
    foreach (var c in controllers)
    {
        Console.WriteLine($"ID: {c.Id}, Name: {c.Name}, SN: {c.SerialNumber}, IP: {c.IpAddress}, Enabled: {c.IsEnabled}");
    }
}
