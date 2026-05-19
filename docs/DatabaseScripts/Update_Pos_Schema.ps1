$connectionString = "Server=.\SQLEXPRESS;Database=ClubMembership;Integrated Security=True;TrustServerCertificate=True;Encrypt=False;"
$sql = @"
-- 1. Create the PosMenuProfiles table
IF OBJECT_ID('dbo.PosMenuProfiles', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PosMenuProfiles (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME2 NULL,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        RowVersion ROWVERSION NOT NULL
    );
END;

-- 2. Create the PosMenuOverrides table
IF OBJECT_ID('dbo.PosMenuOverrides', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PosMenuOverrides (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        MenuProfileId INT NOT NULL,
        LiquorItemId INT NOT NULL,
        OverridePrice DECIMAL(18,2) NULL,
        OverrideCategory NVARCHAR(100) NULL,
        IsVisible BIT NULL,
        DisplayOrder INT NULL,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME2 NULL,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT FK_PosMenuOverrides_PosMenuProfiles FOREIGN KEY (MenuProfileId) REFERENCES dbo.PosMenuProfiles(Id) ON DELETE CASCADE,
        CONSTRAINT FK_PosMenuOverrides_LiquorItems FOREIGN KEY (LiquorItemId) REFERENCES dbo.LiquorItems(Id) ON DELETE CASCADE
    );
END;

-- 3. Create the PosTerminals table
IF OBJECT_ID('dbo.PosTerminals', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.PosTerminals (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TerminalName NVARCHAR(100) NOT NULL,
        MenuProfileId INT NULL,
        LastSeenAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(MAX) NULL,
        ModifiedBy NVARCHAR(MAX) NULL,
        SyncDate DATETIME2 NULL,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        RowVersion ROWVERSION NOT NULL,
        CONSTRAINT FK_PosTerminals_PosMenuProfiles FOREIGN KEY (MenuProfileId) REFERENCES dbo.PosMenuProfiles(Id) ON DELETE SET NULL
    );
    
    CREATE UNIQUE INDEX IX_PosTerminals_TerminalName ON dbo.PosTerminals (TerminalName);
END;

-- 4. Add the MenuProfileId column to the PosCategories table
IF NOT EXISTS (
    SELECT 1 FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.PosCategories') 
    AND name = 'MenuProfileId'
)
BEGIN
    ALTER TABLE dbo.PosCategories 
    ADD MenuProfileId INT NULL;

    ALTER TABLE dbo.PosCategories
    ADD CONSTRAINT FK_PosCategories_PosMenuProfiles 
    FOREIGN KEY (MenuProfileId) REFERENCES dbo.PosMenuProfiles(Id) ON DELETE SET NULL;
END;
"@

Write-Output "Connecting to local SQL Express Database: ClubMembership..."
$connection = New-Object System.Data.SqlClient.SqlConnection($connectionString)
try {
    $connection.Open()
    $command = $connection.CreateCommand()
    $command.CommandText = $sql
    $command.ExecuteNonQuery()
    Write-Output "Success: Database schema updated successfully!"
} catch {
    Write-Error "Database Migration Failed: $_"
} finally {
    $connection.Close()
}
