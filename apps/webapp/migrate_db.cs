using Microsoft.Data.SqlClient;
using System;

class Program
{
    static void Main()
    {
        string connStr = "Server=.\\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;";
        using var conn = new SqlConnection(connStr);
        conn.Open();
        Console.WriteLine("Updating database...");
        
        string sql = @"
            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryCommissionRates') AND name = 'DailySystemFee')
            BEGIN
                ALTER TABLE LotteryCommissionRates ADD DailySystemFee DECIMAL(18,2) NOT NULL DEFAULT 2.00;
                Console.WriteLine('Added DailySystemFee');
            END
            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryCommissionRates') AND name = 'WeeklyBondFee')
            BEGIN
                ALTER TABLE LotteryCommissionRates ADD WeeklyBondFee DECIMAL(18,2) NOT NULL DEFAULT 7.00;
                Console.WriteLine('Added WeeklyBondFee');
            END";
        
        // Wait! I can't put Console.WriteLine in SQL.
        string sqlFixed = @"
            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryCommissionRates') AND name = 'DailySystemFee')
            BEGIN
                ALTER TABLE LotteryCommissionRates ADD DailySystemFee DECIMAL(18,2) NOT NULL DEFAULT 2.00;
            END;
            IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LotteryCommissionRates') AND name = 'WeeklyBondFee')
            BEGIN
                ALTER TABLE LotteryCommissionRates ADD WeeklyBondFee DECIMAL(18,2) NOT NULL DEFAULT 7.00;
            END;";

        using var cmd = new SqlCommand(sqlFixed, conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("Database updated successfully.");
    }
}
