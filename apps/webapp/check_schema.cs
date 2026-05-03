
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

var connectionString = "Server=localhost\\SQLEXPRESS;Database=GFC;Trusted_Connection=True;TrustServerCertificate=True;";
var optionsBuilder = new DbContextOptionsBuilder<GfcDbContext>();
optionsBuilder.UseSqlServer(connectionString);

using var db = new GfcDbContext(optionsBuilder.Options);

try {
    Console.WriteLine("Checking SystemSettings table...");
    var columns = db.Model.FindEntityType(typeof(GFC.Core.Models.SystemSettings))?
        .GetProperties().Select(p => p.GetColumnName()).ToList();
    
    if (columns != null) {
        Console.WriteLine("Columns in SystemSettings:");
        foreach (var col in columns) {
            Console.WriteLine($"- {col}");
        }
    } else {
        Console.WriteLine("SystemSettings entity not found in model.");
    }
} catch (Exception ex) {
    Console.WriteLine($"Error: {ex.Message}");
}
