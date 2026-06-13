using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;

namespace GFC.Scratch
{
    public class CheckPullTabs
    {
        public static void Main()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GfcDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;");

            using (var db = new GfcDbContext(optionsBuilder.Options))
            {
                try
                {
                    var games = db.PullTabGameDefinitions.Include(g => g.PrizeOptions).ToList();
                    Console.WriteLine($"Found {games.Count} Pull Tab games in database:");
                    foreach (var g in games)
                    {
                        Console.WriteLine($"- ID: {g.Id}, Name: {g.GameName}, Price: {g.TicketPrice}, Active: {g.IsActive}, Deleted: {g.IsDeleted}");
                        foreach (var o in g.PrizeOptions)
                        {
                            Console.WriteLine($"  * Option ID: {o.Id}, Label: {o.OptionLabel}, Payout: {o.PayoutAmount}, Active: {o.IsActive}, Deleted: {o.IsDeleted}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading database: " + ex.Message);
                }
            }
        }
    }
}
