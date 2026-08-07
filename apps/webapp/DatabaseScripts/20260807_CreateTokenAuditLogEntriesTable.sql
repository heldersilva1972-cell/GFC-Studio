IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TokenAuditLogEntries]') AND type = N'U')
BEGIN
    CREATE TABLE [dbo].[TokenAuditLogEntries] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Timestamp] DATETIME NOT NULL,
        [TokenName] NVARCHAR(100) NOT NULL,
        [ExpectedStock] INT NOT NULL,
        [ActualCount] INT NOT NULL,
        [Variance] INT NOT NULL,
        [Status] NVARCHAR(50) NOT NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [PerformedBy] NVARCHAR(100) NULL
    );
    PRINT 'Created TokenAuditLogEntries table.';
END
ELSE
BEGIN
    PRINT 'TokenAuditLogEntries table already exists.';
END
GO
