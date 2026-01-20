-- Create UserPasskeys table for biometric authentication
-- Run this script on your SQL Server database

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserPasskeys')
BEGIN
    CREATE TABLE UserPasskeys (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        UserId INT NOT NULL,
        CredentialId NVARCHAR(500) NOT NULL UNIQUE,
        PublicKey NVARCHAR(MAX) NOT NULL,
        FriendlyName NVARCHAR(200) NULL,
        UserHandle NVARCHAR(500) NULL,
        SignatureCounter BIGINT NOT NULL DEFAULT 0,
        AttestationFormat NVARCHAR(50) NULL,
        AAGUID UNIQUEIDENTIFIER NULL,
        CreatedAtUtc DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        LastUsedUtc DATETIME2 NULL,
        CONSTRAINT FK_UserPasskeys_Users FOREIGN KEY (UserId) REFERENCES AppUsers(UserId) ON DELETE CASCADE
    );

    CREATE INDEX IX_UserPasskeys_UserId ON UserPasskeys(UserId);
    CREATE INDEX IX_UserPasskeys_CredentialId ON UserPasskeys(CredentialId);

    PRINT 'UserPasskeys table created successfully';
END
ELSE
BEGIN
    PRINT 'UserPasskeys table already exists';
END
GO
