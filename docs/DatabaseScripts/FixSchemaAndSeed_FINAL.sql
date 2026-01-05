-- Ensure we are in the right DB
USE [ClubMembership];
GO

-- 1. Create Controllers table if missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Controllers')
BEGIN
    CREATE TABLE [Controllers] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NOT NULL,
        [SerialNumber] int NOT NULL,
        [IpAddress] nvarchar(50) NOT NULL,
        [Port] int NOT NULL,
        [IsEnabled] bit NOT NULL DEFAULT 1,
        [LastSeen] datetime2 NULL,
        [IsSimulated] bit NOT NULL DEFAULT 0,
        CONSTRAINT [PK_Controllers] PRIMARY KEY ([Id])
    );
    PRINT 'Created Controllers table.';
END
ELSE
BEGIN
    PRINT 'Controllers table already exists.';
END
GO

-- 2. Create Doors table if missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Doors')
BEGIN
    CREATE TABLE [Doors] (
        [Id] int NOT NULL IDENTITY,
        [ControllerId] int NOT NULL,
        [DoorIndex] int NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [IsEnabled] bit NOT NULL DEFAULT 1,
        CONSTRAINT [PK_Doors] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Doors_Controllers_ControllerId] FOREIGN KEY ([ControllerId]) REFERENCES [Controllers] ([Id]) ON DELETE CASCADE
    );
    CREATE INDEX [IX_Doors_ControllerId] ON [Doors] ([ControllerId]);
    PRINT 'Created Doors table.';
END
ELSE
BEGIN
    PRINT 'Doors table already exists.';
END
GO

-- 3. Create ControllerLogs table if missing
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ControllerLogs')
BEGIN
    CREATE TABLE [ControllerLogs] (
        [Id] int NOT NULL IDENTITY,
        [Timestamp] datetime2 NOT NULL,
        [ControllerId] int NULL,
        [EventCode] int NOT NULL,
        [EventDetail] nvarchar(max) NULL,
        [RawData] nvarchar(max) NULL,
        CONSTRAINT [PK_ControllerLogs] PRIMARY KEY ([Id])
    );
    CREATE INDEX [IX_ControllerLogs_ControllerId] ON [ControllerLogs] ([ControllerId]);
    PRINT 'Created ControllerLogs table.';
END
ELSE
BEGIN
    PRINT 'ControllerLogs table already exists.';
END
GO

-- 4. Check AppUsers and Members
DECLARE @AppUserCount int;
SELECT @AppUserCount = COUNT(*) FROM AppUsers;
PRINT 'AppUsers Count: ' + CAST(@AppUserCount AS NVARCHAR(10));

DECLARE @MemberCount int;
SELECT @MemberCount = COUNT(*) FROM Members;
PRINT 'Members Count: ' + CAST(@MemberCount AS NVARCHAR(10));

-- 5. Seed Admin User if missing
IF @AppUserCount = 0
BEGIN
    PRINT 'Seeding default admin user...';
    -- Password hash for 'Admin123!' (You might need to use the actual Hasher in C# code, but for now we insert a placeholder or try to use a known hash if we had one. 
    -- Since we cant easily generate the hash here, we will trust the OnStartup logic in Program.cs to create it IF the table works.
    -- But Program.cs seemingly failed or hasn't run fully.
    -- Let's NOT insert AppUser here to avoid hash mismatch. The app *should* create it on startup if configured correctly.
    PRINT 'Skipping AppUser seed - relying on Program.cs';
END

-- 6. Seed a Test Member if missing (so dashboard isn't 0)
IF @MemberCount = 0
BEGIN
    PRINT 'Seeding test member...';
    INSERT INTO Members (
        FirstName, LastName, Status, 
        ApplicationDate, AcceptedDate, 
        IsNonPortugueseOrigin, 
        Address1, City, State, PostalCode, 
        Email, Phone
    ) VALUES (
        'Test', 'Member', 'REGULAR', 
        GETDATE(), GETDATE(), 
        0, 
        '123 Club Lane', 'MemberCity', 'MA', '02000', 
        'test@example.com', '555-0199'
    );
    PRINT 'Seeded test member.';
END
GO
