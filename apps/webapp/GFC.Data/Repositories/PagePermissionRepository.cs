using System.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.Data;
using Microsoft.Data.SqlClient;

namespace GFC.Data.Repositories;

public class PagePermissionRepository : IPagePermissionRepository
{
    // Page management
    public IEnumerable<AppPage> GetAllPages()
    {
        var pages = new List<AppPage>();
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "SELECT * FROM AppPages ORDER BY Category, DisplayOrder, PageName";
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            pages.Add(MapReaderToAppPage(reader));
        }
        
        return pages;
    }

    public IEnumerable<AppPage> GetActivePages()
    {
        var pages = new List<AppPage>();
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "SELECT * FROM AppPages WHERE IsActive = 1 ORDER BY Category, DisplayOrder, PageName";
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            pages.Add(MapReaderToAppPage(reader));
        }
        
        return pages;
    }

    public IEnumerable<AppPage> GetPagesByCategory(string category)
    {
        var pages = new List<AppPage>();
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "SELECT * FROM AppPages WHERE Category = @Category AND IsActive = 1 ORDER BY DisplayOrder, PageName";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Category", category);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            pages.Add(MapReaderToAppPage(reader));
        }
        
        return pages;
    }

    public AppPage? GetPageById(int pageId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "SELECT * FROM AppPages WHERE PageId = @PageId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PageId", pageId);
        using var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            return MapReaderToAppPage(reader);
        }
        
        return null;
    }

    public AppPage? GetPageByRoute(string route)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        var normalizedRoute = route.TrimStart('/').ToLowerInvariant();
        const string sql = @"SELECT * FROM AppPages 
              WHERE LOWER(LTRIM(PageRoute, '/')) = @Route 
              OR PageRoute = @OriginalRoute";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Route", normalizedRoute);
        command.Parameters.AddWithValue("@OriginalRoute", route);
        using var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            return MapReaderToAppPage(reader);
        }
        
        return null;
    }

    public void AddPage(AppPage page)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
              VALUES (@PageName, @PageRoute, @Description, @Category, @RequiresAdmin, @IsActive, @DisplayOrder)";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PageName", page.PageName);
        command.Parameters.AddWithValue("@PageRoute", page.PageRoute);
        command.Parameters.AddWithValue("@Description", (object?)page.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Category", (object?)page.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("@RequiresAdmin", page.RequiresAdmin);
        command.Parameters.AddWithValue("@IsActive", page.IsActive);
        command.Parameters.AddWithValue("@DisplayOrder", page.DisplayOrder);
        command.ExecuteNonQuery();
    }

    public void UpdatePage(AppPage page)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"UPDATE AppPages SET 
              PageName = @PageName, 
              PageRoute = @PageRoute, 
              Description = @Description, 
              Category = @Category, 
              RequiresAdmin = @RequiresAdmin, 
              IsActive = @IsActive, 
              DisplayOrder = @DisplayOrder
              WHERE PageId = @PageId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PageId", page.PageId);
        command.Parameters.AddWithValue("@PageName", page.PageName);
        command.Parameters.AddWithValue("@PageRoute", page.PageRoute);
        command.Parameters.AddWithValue("@Description", (object?)page.Description ?? DBNull.Value);
        command.Parameters.AddWithValue("@Category", (object?)page.Category ?? DBNull.Value);
        command.Parameters.AddWithValue("@RequiresAdmin", page.RequiresAdmin);
        command.Parameters.AddWithValue("@IsActive", page.IsActive);
        command.Parameters.AddWithValue("@DisplayOrder", page.DisplayOrder);
        command.ExecuteNonQuery();
    }

    // Permission management
    public IEnumerable<UserPagePermission> GetUserPermissions(int userId)
    {
        var permissions = new List<UserPagePermission>();
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"SELECT upp.*, ap.PageName, ap.PageRoute, ap.Category 
              FROM UserPagePermissions upp
              INNER JOIN AppPages ap ON upp.PageId = ap.PageId
              WHERE upp.UserId = @UserId AND upp.CanAccess = 1
              ORDER BY ap.Category, ap.DisplayOrder";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            permissions.Add(MapReaderToUserPagePermission(reader));
        }
        
        return permissions;
    }

    public IEnumerable<UserPagePermission> GetPagePermissions(int pageId)
    {
        var permissions = new List<UserPagePermission>();
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"SELECT upp.*, u.Username 
              FROM UserPagePermissions upp
              INNER JOIN AppUsers u ON upp.UserId = u.UserId
              WHERE upp.PageId = @PageId AND upp.CanAccess = 1";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@PageId", pageId);
        using var reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            permissions.Add(MapReaderToUserPagePermission(reader));
        }
        
        return permissions;
    }

    public UserPagePermission? GetPermission(int userId, int pageId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "SELECT * FROM UserPagePermissions WHERE UserId = @UserId AND PageId = @PageId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PageId", pageId);
        using var reader = command.ExecuteReader();
        
        if (reader.Read())
        {
            return MapReaderToUserPagePermission(reader);
        }
        
        return null;
    }

    public bool HasPermission(int userId, string pageRoute)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        // First check if user is admin
        const string adminSql = "SELECT IsAdmin FROM AppUsers WHERE UserId = @UserId";
        using var adminCommand = new SqlCommand(adminSql, connection);
        adminCommand.Parameters.AddWithValue("@UserId", userId);
        var isAdmin = (bool?)adminCommand.ExecuteScalar();
        
        if (isAdmin == true)
            return true;

        // Check specific permission
        var normalizedRoute = pageRoute.TrimStart('/').ToLowerInvariant();
        const string sql = @"SELECT CAST(CASE WHEN COUNT(*) > 0 THEN 1 ELSE 0 END AS BIT)
              FROM UserPagePermissions upp
              INNER JOIN AppPages ap ON upp.PageId = ap.PageId
              WHERE upp.UserId = @UserId 
              AND upp.CanAccess = 1
              AND (LOWER(LTRIM(ap.PageRoute, '/')) = @Route OR ap.PageRoute = @OriginalRoute)
              AND ap.IsActive = 1";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@Route", normalizedRoute);
        command.Parameters.AddWithValue("@OriginalRoute", pageRoute);
        
        return (bool)command.ExecuteScalar();
    }

    // CRUD operations
    public void GrantPermission(int userId, int pageId, string grantedBy)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"MERGE UserPagePermissions AS target
              USING (SELECT @UserId AS UserId, @PageId AS PageId) AS source
              ON target.UserId = source.UserId AND target.PageId = source.PageId
              WHEN MATCHED THEN
                  UPDATE SET CanAccess = 1, GrantedDate = GETDATE(), GrantedBy = @GrantedBy
              WHEN NOT MATCHED THEN
                  INSERT (UserId, PageId, CanAccess, GrantedDate, GrantedBy)
                  VALUES (@UserId, @PageId, 1, GETDATE(), @GrantedBy);";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PageId", pageId);
        command.Parameters.AddWithValue("@GrantedBy", grantedBy);
        command.ExecuteNonQuery();
    }

    public void RevokePermission(int userId, int pageId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "DELETE FROM UserPagePermissions WHERE UserId = @UserId AND PageId = @PageId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@PageId", pageId);
        command.ExecuteNonQuery();
    }

    public void SetUserPermissions(int userId, IEnumerable<int> pageIds, string grantedBy)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            // Clear existing permissions
            const string deleteSql = "DELETE FROM UserPagePermissions WHERE UserId = @UserId";
            using (var deleteCommand = new SqlCommand(deleteSql, connection, transaction))
            {
                deleteCommand.Parameters.AddWithValue("@UserId", userId);
                deleteCommand.ExecuteNonQuery();
            }

            // Add new permissions
            foreach (var pageId in pageIds)
            {
                const string insertSql = @"INSERT INTO UserPagePermissions (UserId, PageId, CanAccess, GrantedDate, GrantedBy)
                      VALUES (@UserId, @PageId, 1, GETDATE(), @GrantedBy)";
                using var insertCommand = new SqlCommand(insertSql, connection, transaction);
                insertCommand.Parameters.AddWithValue("@UserId", userId);
                insertCommand.Parameters.AddWithValue("@PageId", pageId);
                insertCommand.Parameters.AddWithValue("@GrantedBy", grantedBy);
                insertCommand.ExecuteNonQuery();
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public void ClearUserPermissions(int userId)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = "DELETE FROM UserPagePermissions WHERE UserId = @UserId";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.ExecuteNonQuery();
    }

    // Bulk operations
    public void GrantAllPermissions(int userId, string grantedBy)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        const string sql = @"INSERT INTO UserPagePermissions (UserId, PageId, CanAccess, GrantedDate, GrantedBy)
              SELECT @UserId, PageId, 1, GETDATE(), @GrantedBy
              FROM AppPages
              WHERE IsActive = 1
              AND NOT EXISTS (
                  SELECT 1 FROM UserPagePermissions 
                  WHERE UserId = @UserId AND PageId = AppPages.PageId
              )";
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserId", userId);
        command.Parameters.AddWithValue("@GrantedBy", grantedBy);
        command.ExecuteNonQuery();
    }

    public void CopyPermissions(int sourceUserId, int targetUserId, string grantedBy)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            // Clear target user's permissions
            const string deleteSql = "DELETE FROM UserPagePermissions WHERE UserId = @UserId";
            using (var deleteCommand = new SqlCommand(deleteSql, connection, transaction))
            {
                deleteCommand.Parameters.AddWithValue("@UserId", targetUserId);
                deleteCommand.ExecuteNonQuery();
            }

            // Copy permissions from source user
            const string copySql = @"INSERT INTO UserPagePermissions (UserId, PageId, CanAccess, GrantedDate, GrantedBy)
                  SELECT @TargetUserId, PageId, CanAccess, GETDATE(), @GrantedBy
                  FROM UserPagePermissions
                  WHERE UserId = @SourceUserId";
            using var copyCommand = new SqlCommand(copySql, connection, transaction);
            copyCommand.Parameters.AddWithValue("@SourceUserId", sourceUserId);
            copyCommand.Parameters.AddWithValue("@TargetUserId", targetUserId);
            copyCommand.Parameters.AddWithValue("@GrantedBy", grantedBy);
            copyCommand.ExecuteNonQuery();

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public IEnumerable<int> GetDefaultPageIds()
    {
        var ids = new List<int>();
        try 
        {
            using var connection = Db.GetConnection();
            connection.Open();
            
            // Check if table exists first to avoid crashing
            const string checkSql = "SELECT OBJECT_ID(N'[dbo].[DefaultPermissions]', N'U')";
            using var checkCmd = new SqlCommand(checkSql, connection);
            if (checkCmd.ExecuteScalar() == DBNull.Value) return ids;

            const string sql = "SELECT PageId FROM DefaultPermissions";
            using var command = new SqlCommand(sql, connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                ids.Add((int)reader["PageId"]);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[DB ERROR] Failed to load DefaultPermissions: {ex.Message}");
        }
        return ids;
    }

    public void SetDefaultPageIds(IEnumerable<int> pageIds)
    {
        using var connection = Db.GetConnection();
        connection.Open();
        
        // AUTO-FIX: Ensure table exists before saving
        const string createSql = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DefaultPermissions]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[DefaultPermissions] (
                    [PageId] INT NOT NULL,
                    CONSTRAINT [PK_DefaultPermissions] PRIMARY KEY CLUSTERED ([PageId] ASC),
                    CONSTRAINT [FK_DefaultPermissions_AppPages] FOREIGN KEY ([PageId]) REFERENCES [dbo].[AppPages] ([PageId]) ON DELETE CASCADE
                );
            END";
        using (var createCmd = new SqlCommand(createSql, connection))
        {
            createCmd.ExecuteNonQuery();
        }

        using var transaction = connection.BeginTransaction();
        try
        {
            using (var deleteCmd = new SqlCommand("DELETE FROM DefaultPermissions", connection, transaction))
            {
                deleteCmd.ExecuteNonQuery();
            }

            foreach (var id in pageIds)
            {
                using var insertCmd = new SqlCommand("INSERT INTO DefaultPermissions (PageId) VALUES (@PageId)", connection, transaction);
                insertCmd.Parameters.AddWithValue("@PageId", id);
                insertCmd.ExecuteNonQuery();
            }
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    // Helper methods
    private static AppPage MapReaderToAppPage(SqlDataReader reader)
    {
        return new AppPage
        {
            PageId = reader["PageId"] != DBNull.Value ? (int)reader["PageId"] : 0,
            PageName = reader["PageName"]?.ToString() ?? string.Empty,
            PageRoute = reader["PageRoute"]?.ToString() ?? string.Empty,
            Description = reader["Description"] as string,
            Category = reader["Category"] as string,
            RequiresAdmin = reader["RequiresAdmin"] != DBNull.Value && (bool)reader["RequiresAdmin"],
            IsActive = reader["IsActive"] != DBNull.Value && (bool)reader["IsActive"],
            DisplayOrder = reader["DisplayOrder"] != DBNull.Value ? (int)reader["DisplayOrder"] : 0
        };
    }

    private static UserPagePermission MapReaderToUserPagePermission(SqlDataReader reader)
    {
        return new UserPagePermission
        {
            PermissionId = reader["PermissionId"] != DBNull.Value ? (int)reader["PermissionId"] : 0,
            UserId = reader["UserId"] != DBNull.Value ? (int)reader["UserId"] : 0,
            PageId = reader["PageId"] != DBNull.Value ? (int)reader["PageId"] : 0,
            CanAccess = reader["CanAccess"] != DBNull.Value && (bool)reader["CanAccess"],
            GrantedDate = reader["GrantedDate"] != DBNull.Value ? (DateTime)reader["GrantedDate"] : DateTime.MinValue,
            GrantedBy = reader["GrantedBy"] as string
        };
    }
}
