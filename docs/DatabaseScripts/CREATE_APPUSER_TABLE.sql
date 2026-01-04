-- =============================================
-- DEEP DIVE FIX: Create AppUsers Table (PLURAL)
-- For GFC Custom Authentication System
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Deep Dive Fix: Creating AppUsers Table';
PRINT '========================================';
PRINT '';

-- Drop the wrong table if it exists (singular)
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUser]') AND type in (N'U'))
BEGIN
    DROP TABLE [dbo].[AppUser];
    PRINT '✓ Dropped old singular AppUser table';
END

-- Create the correct table (PLURAL) as required by UserRepository.cs
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AppUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AppUsers] (
        [UserId] INT IDENTITY(1,1) NOT NULL,
        [Username] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(256) NULL,
        [PasswordHash] NVARCHAR(MAX) NOT NULL,
        [IsAdmin] BIT NOT NULL DEFAULT 0,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [MemberId] INT NULL,
        [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [LastLoginDate] DATETIME2 NULL,
        [CreatedBy] NVARCHAR(100) NULL,
        [Notes] NVARCHAR(500) NULL,
        [PasswordChangeRequired] BIT NOT NULL DEFAULT 0,
        [MfaEnabled] BIT NOT NULL DEFAULT 0,
        [MfaSecretKey] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AppUsers] PRIMARY KEY CLUSTERED ([UserId] ASC)
    );
    
    CREATE UNIQUE NONCLUSTERED INDEX [IX_AppUsers_Username] 
    ON [dbo].[AppUsers] ([Username] ASC);
    
    PRINT '✓ Created CORRECT AppUsers table (plural)';
END
ELSE
BEGIN
    PRINT '✓ AppUsers table already exists';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'AppUsers Table Ready';
PRINT '========================================';
PRINT '';
PRINT 'SUMMARY:';
PRINT 'The application was looking for "AppUsers" (plural) but the previous';
PRINT 'script created "AppUser" (singular). This caused a silent failure.';
PRINT '';
PRINT 'NEXT STEPS:';
PRINT '1. RESTART your application now.';
PRINT '2. Upon startup, it will see the empty table and create the admin user.';
PRINT '3. Login using: admin / Admin123!';
PRINT '========================================';
GO
