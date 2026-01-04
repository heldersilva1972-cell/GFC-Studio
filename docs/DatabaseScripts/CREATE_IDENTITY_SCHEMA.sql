-- =============================================
-- Create ASP.NET Identity Tables
-- Database: ClubMembership
-- =============================================

USE [ClubMembership];
GO

PRINT '========================================';
PRINT 'Creating ASP.NET Identity Schema';
PRINT '========================================';
PRINT '';

-- Create AspNetRoles table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoles] (
        [Id] NVARCHAR(450) NOT NULL,
        [Name] NVARCHAR(256) NULL,
        [NormalizedName] NVARCHAR(256) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    CREATE UNIQUE NONCLUSTERED INDEX [RoleNameIndex] 
    ON [dbo].[AspNetRoles] ([NormalizedName] ASC) 
    WHERE ([NormalizedName] IS NOT NULL);
    
    PRINT '✓ Created AspNetRoles table';
END
ELSE
    PRINT '✓ AspNetRoles table already exists';
GO

-- Create AspNetUsers table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUsers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUsers] (
        [Id] NVARCHAR(450) NOT NULL,
        [UserName] NVARCHAR(256) NULL,
        [NormalizedUserName] NVARCHAR(256) NULL,
        [Email] NVARCHAR(256) NULL,
        [NormalizedEmail] NVARCHAR(256) NULL,
        [EmailConfirmed] BIT NOT NULL,
        [PasswordHash] NVARCHAR(MAX) NULL,
        [SecurityStamp] NVARCHAR(MAX) NULL,
        [ConcurrencyStamp] NVARCHAR(MAX) NULL,
        [PhoneNumber] NVARCHAR(MAX) NULL,
        [PhoneNumberConfirmed] BIT NOT NULL,
        [TwoFactorEnabled] BIT NOT NULL,
        [LockoutEnd] DATETIMEOFFSET(7) NULL,
        [LockoutEnabled] BIT NOT NULL,
        [AccessFailedCount] INT NOT NULL,
        CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    CREATE NONCLUSTERED INDEX [EmailIndex] 
    ON [dbo].[AspNetUsers] ([NormalizedEmail] ASC);
    
    CREATE UNIQUE NONCLUSTERED INDEX [UserNameIndex] 
    ON [dbo].[AspNetUsers] ([NormalizedUserName] ASC) 
    WHERE ([NormalizedUserName] IS NOT NULL);
    
    PRINT '✓ Created AspNetUsers table';
END
ELSE
    PRINT '✓ AspNetUsers table already exists';
GO

-- Create AspNetUserRoles table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserRoles]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserRoles] (
        [UserId] NVARCHAR(450) NOT NULL,
        [RoleId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
        CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) 
            REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) 
            REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE NONCLUSTERED INDEX [IX_AspNetUserRoles_RoleId] 
    ON [dbo].[AspNetUserRoles] ([RoleId] ASC);
    
    PRINT '✓ Created AspNetUserRoles table';
END
ELSE
    PRINT '✓ AspNetUserRoles table already exists';
GO

-- Create AspNetUserClaims table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [UserId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) 
            REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE NONCLUSTERED INDEX [IX_AspNetUserClaims_UserId] 
    ON [dbo].[AspNetUserClaims] ([UserId] ASC);
    
    PRINT '✓ Created AspNetUserClaims table';
END
ELSE
    PRINT '✓ AspNetUserClaims table already exists';
GO

-- Create AspNetUserLogins table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserLogins]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserLogins] (
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [ProviderKey] NVARCHAR(450) NOT NULL,
        [ProviderDisplayName] NVARCHAR(MAX) NULL,
        [UserId] NVARCHAR(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC),
        CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) 
            REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    CREATE NONCLUSTERED INDEX [IX_AspNetUserLogins_UserId] 
    ON [dbo].[AspNetUserLogins] ([UserId] ASC);
    
    PRINT '✓ Created AspNetUserLogins table';
END
ELSE
    PRINT '✓ AspNetUserLogins table already exists';
GO

-- Create AspNetUserTokens table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetUserTokens]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetUserTokens] (
        [UserId] NVARCHAR(450) NOT NULL,
        [LoginProvider] NVARCHAR(450) NOT NULL,
        [Name] NVARCHAR(450) NOT NULL,
        [Value] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED ([UserId] ASC, [LoginProvider] ASC, [Name] ASC),
        CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) 
            REFERENCES [dbo].[AspNetUsers] ([Id]) ON DELETE CASCADE
    );
    
    PRINT '✓ Created AspNetUserTokens table';
END
ELSE
    PRINT '✓ AspNetUserTokens table already exists';
GO

-- Create AspNetRoleClaims table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AspNetRoleClaims]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AspNetRoleClaims] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [RoleId] NVARCHAR(450) NOT NULL,
        [ClaimType] NVARCHAR(MAX) NULL,
        [ClaimValue] NVARCHAR(MAX) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) 
            REFERENCES [dbo].[AspNetRoles] ([Id]) ON DELETE CASCADE
    );
    
    CREATE NONCLUSTERED INDEX [IX_AspNetRoleClaims_RoleId] 
    ON [dbo].[AspNetRoleClaims] ([RoleId] ASC);
    
    PRINT '✓ Created AspNetRoleClaims table';
END
ELSE
    PRINT '✓ AspNetRoleClaims table already exists';
GO

PRINT '';
PRINT '========================================';
PRINT 'ASP.NET Identity Schema Created!';
PRINT '========================================';
PRINT '';
PRINT 'Tables Created:';
PRINT '  ✓ AspNetRoles';
PRINT '  ✓ AspNetUsers';
PRINT '  ✓ AspNetUserRoles';
PRINT '  ✓ AspNetUserClaims';
PRINT '  ✓ AspNetUserLogins';
PRINT '  ✓ AspNetUserTokens';
PRINT '  ✓ AspNetRoleClaims';
PRINT '';
PRINT 'NEXT STEP: Run CREATE_ADMIN_USER.sql to create admin user';
PRINT '========================================';
GO
