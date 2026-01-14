using System;
using System.Data;
using Microsoft.Data.SqlClient;

string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;Trusted_Connection=True;MultipleActiveResultSets=true;";
using var conn = new SqlConnection(connStr);
conn.Open();

void PrintSchema(string table) {
    Console.WriteLine($"--- Schema: {table} ---");
    using var cmd = new SqlCommand($"SELECT TOP 0 * FROM [{table}]", conn);
    using var reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
    var tableSchema = reader.GetSchemaTable();
    foreach (DataRow row in tableSchema.Rows) {
        Console.WriteLine($"{row["ColumnName"]} ({row["DataType"]})");
    }
    reader.Close();
}

PrintSchema("Waivers");
PrintSchema("DuesWaiverPeriods");
PrintSchema("DuesPayments");
PrintSchema("Members");
