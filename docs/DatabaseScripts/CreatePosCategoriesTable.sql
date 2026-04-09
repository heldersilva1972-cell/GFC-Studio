IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosCategories')
BEGIN
    CREATE TABLE PosCategories (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        GlobalId UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
        Name NVARCHAR(100) NOT NULL,
        DisplayOrder INT NOT NULL DEFAULT 0,
        IsActive BIT NOT NULL DEFAULT 1,
        IsDeleted BIT NOT NULL DEFAULT 0,
        CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        ModifiedAt DATETIME2 NULL,
        CreatedBy NVARCHAR(200) NULL,
        ModifiedBy NVARCHAR(200) NULL,
        SyncDate DATETIME2 NULL,
        RowVersion ROWVERSION
    );
    
    CREATE INDEX IX_PosCategories_DisplayOrder ON PosCategories (DisplayOrder);
END
GO
