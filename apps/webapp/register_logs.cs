using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using GFC.Data;

namespace GFC.Diagnostics
{
    class Program
    {
        static void Main(string[] args)
        {
            // You might need to set the connection string override here if it's not in configuration
            // For now, I'll try to use the default.
            
            try 
            {
                using var connection = Db.GetConnection();
                connection.Open();
                Console.WriteLine("Connection successful.");

                // Register the new page
                string sql = @"
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/audit-logs')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Administrative Logs', '/admin/audit-logs', 'Management and security audit trails', 'ADMINISTRATION', 1, 1, 10);
    Console.WriteLine('Registered Administrative Logs');
END
ELSE
BEGIN
    UPDATE AppPages 
    SET Category = 'ADMINISTRATION', 
        PageName = 'Administrative Logs',
        IsActive = 1
    WHERE PageRoute = '/admin/audit-logs';
    Console.WriteLine('Updated Administrative Logs');
END";

                using var command = new SqlCommand(sql, connection);
                // Note: I can't easily use Console.WriteLine inside SQL block like that in a script, 
                // but I'll use it to execute the migration.
                
                // Clean version:
                string cleanSql = @"
IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/audit-logs')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Administrative Logs', '/admin/audit-logs', 'Management and security audit trails', 'ADMINISTRATION', 1, 1, 15);
END
ELSE
BEGIN
    UPDATE AppPages 
    SET Category = 'ADMINISTRATION', 
        PageName = 'Administrative Logs',
        IsActive = 1
    WHERE PageRoute = '/admin/audit-logs';
END";
                using var cmd = new SqlCommand(cleanSql, connection);
                cmd.ExecuteNonQuery();
                Console.WriteLine("Migration completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
