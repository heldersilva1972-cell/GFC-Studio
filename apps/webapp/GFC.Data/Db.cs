using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data;

/// <summary>
/// Central database connection helper class.
/// Reads connection string from whichever application hosts the data layer (e.g., GFC.WinForms App.config or Blazor appsettings).
/// </summary>
public static class Db
{
    private static string? _configConnectionString;
    private static string? _connectionStringOverride;

    /// <summary>
    /// Optional override for hosts without App.config (e.g., Blazor).
    /// If set, GetConnection() uses this value.
    /// </summary>
    public static string? ConnectionStringOverride
    {
        get => _connectionStringOverride;
        set => _connectionStringOverride = string.IsNullOrWhiteSpace(value) ? null : value;
    }

    /// <summary>
    /// Legacy helper for code that already called Configure.
    /// Kept for backward compatibility; sets <see cref="ConnectionStringOverride"/>.
    /// </summary>
    public static void Configure(string connectionString)
    {
        ConnectionStringOverride = connectionString;
    }

    private static string ResolveConnectionString()
    {
        if (!string.IsNullOrWhiteSpace(ConnectionStringOverride))
        {
            return ConnectionStringOverride!;
        }

        if (_configConnectionString == null)
        {
            // Connection strings live in the startup project (WinForms App.config or Blazor appsettings.json); the data layer simply resolves them by name.
            var connectionString = ConfigurationManager.ConnectionStrings["GFC"]?.ConnectionString;
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'GFC' is not configured. For WinForms, define it in App.config. For Blazor, set Db.ConnectionStringOverride (see Program.cs).");
            }

            _configConnectionString = connectionString;
        }

        return _configConnectionString;
    }

    /// <summary>
    /// Creates and returns a new SqlConnection instance.
    /// </summary>
    public static SqlConnection GetConnection()
    {
        // Connection strings are resolved from the startup project configuration so both WinForms and Blazor can share GFC.Data without duplicating settings.
        return new SqlConnection(ResolveConnectionString());
    }

    /// <summary>
    /// Tests the database connection.
    /// </summary>
    public static bool TestConnection()
    {
        try
        {
            using var connection = GetConnection();
            connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }
}


