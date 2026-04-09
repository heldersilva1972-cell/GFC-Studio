-- Create PosTokens table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PosTokens')
BEGIN
    CREATE TABLE [PosTokens] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(50) NOT NULL,
        [SalePrice] DECIMAL(18,2) NOT NULL,
        [ColorHex] NVARCHAR(10) NOT NULL DEFAULT '#3b82f6',
        [IsActive] BIT NOT NULL DEFAULT 1,
        [EligibleItemIds] NVARCHAR(MAX) NOT NULL DEFAULT '',
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_PosTokens] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    PRINT 'PosTokens table created.';
END
ELSE
BEGIN
    PRINT 'PosTokens table already exists.';
END
GO
