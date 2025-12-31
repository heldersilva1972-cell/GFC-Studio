using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

// This is a scratchpad-style script to force update the DB
class Program {
    static void Main() {
        var optionsBuilder = new DbContextOptionsBuilder<GfcDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=ClubMembership;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;");
        
        using var context = new GfcDbContext(optionsBuilder.Options);
        
        var controller = context.Controllers.FirstOrDefault();
        if (controller != null) {
            Console.WriteLine($"Updating controller {controller.Id} from SN {controller.SerialNumber} to 223213880");
            controller.SerialNumber = 223213880;
            controller.IpAddress = "192.168.1.72";
            
            var config = context.ControllerNetworkConfigs.FirstOrDefault(c => c.ControllerId == controller.Id);
            if (config != null) {
                config.IpAddress = "192.168.1.72";
            } else {
                context.ControllerNetworkConfigs.Add(new ControllerNetworkConfig {
                    ControllerId = controller.Id,
                    IpAddress = "192.168.1.72",
                    SubnetMask = "255.255.255.0",
                    Gateway = "192.168.1.1",
                    Port = 60000,
                    CreatedUtc = DateTime.UtcNow
                });
            }
            
            context.SaveChanges();
            Console.WriteLine("Update successful.");
        } else {
            Console.WriteLine("No controller found in DB.");
        }
    }
}
