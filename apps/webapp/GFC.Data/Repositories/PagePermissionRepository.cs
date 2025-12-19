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
              INNER JOIN Users u ON upp.UserId = u.UserId
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
        const string adminSql = "SELECT IsAdmin FROM Users WHERE UserId = @UserId";
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

    // Helper methods
    private static AppPage MapReaderToAppPage(SqlDataReader reader)
    {
        return new AppPage
        {
            PageId = (int)reader["PageId"],
            PageName = reader["PageName"].ToString() ?? string.Empty,
            PageRoute = reader["PageRoute"].ToString() ?? string.Empty,
            Description = reader["Description"] as string,
            Category = reader["Category"] as string,
            RequiresAdmin = (bool)reader["RequiresAdmin"],
            IsActive = (bool)reader["IsActive"],
            DisplayOrder = (int)reader["DisplayOrder"]
        };
    }

    private static UserPagePermission MapReaderToUserPagePermission(SqlDataReader reader)
    {
        return new UserPagePermission
        {
            PermissionId = (int)reader["PermissionId"],
            UserId = (int)reader["UserId"],
            PageId = (int)reader["PageId"],
            CanAccess = (bool)reader["CanAccess"],
            GrantedDate = (DateTime)reader["GrantedDate"],
            GrantedBy = reader["GrantedBy"] as string
        };
    }
}
