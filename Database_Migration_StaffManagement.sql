-- =============================================
-- GFC Studio Phase 6A-3: Staff Management Tables
-- Created: 2025-12-23
-- Description: Adds StaffMembers table and updates StaffShifts to support proper staff management
-- =============================================

-- Step 1: Create StaffMembers table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffMembers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StaffMembers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        [Role] NVARCHAR(50) NULL,
        [MemberId] INT NULL, -- Optional link to existing GFC Member
        [PhoneNumber] NVARCHAR(20) NULL,
        [Email] NVARCHAR(100) NULL,
        [HourlyRate] DECIMAL(10,2) NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [HireDate] DATETIME2 NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NULL,
        [Notes] NVARCHAR(500) NULL,
        CONSTRAINT [PK_StaffMembers] PRIMARY KEY CLUSTERED ([Id] ASC)
    );
    
    PRINT 'Created StaffMembers table';
END
ELSE
BEGIN
    PRINT 'StaffMembers table already exists';
END
GO

-- Step 2: Check if StaffShifts table exists
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND type in (N'U'))
BEGIN
    -- Check if StaffMemberId column exists and update foreign key if needed
    IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[StaffShifts]') AND name = 'StaffMemberId')
    BEGIN
        -- Drop existing foreign key constraint if it exists
        IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_StaffShifts_AppUsers')
        BEGIN
            ALTER TABLE [dbo].[StaffShifts] DROP CONSTRAINT [FK_StaffShifts_AppUsers];
            PRINT 'Dropped old FK_StaffShifts_AppUsers constraint';
        END
        
        -- Add new foreign key to StaffMembers
        IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_StaffShifts_StaffMembers')
        BEGIN
            ALTER TABLE [dbo].[StaffShifts]
            ADD CONSTRAINT [FK_StaffShifts_StaffMembers] 
            FOREIGN KEY ([StaffMemberId]) REFERENCES [dbo].[StaffMembers]([Id])
            ON DELETE CASCADE;
            
            PRINT 'Added FK_StaffShifts_StaffMembers constraint';
        END
    END
    ELSE
    BEGIN
        PRINT 'StaffMemberId column does not exist in StaffShifts - may need to add it';
    END
END
ELSE
BEGIN
    -- Create StaffShifts table if it doesn't exist
    CREATE TABLE [dbo].[StaffShifts] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [StaffMemberId] INT NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [ShiftType] INT NOT NULL, -- 1=Day, 2=Night
        [Status] NVARCHAR(50) NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NULL,
        CONSTRAINT [PK_StaffShifts] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_StaffShifts_StaffMembers] FOREIGN KEY ([StaffMemberId]) 
            REFERENCES [dbo].[StaffMembers]([Id]) ON DELETE CASCADE
    );
    
    PRINT 'Created StaffShifts table';
END
GO

-- Step 3: Create indexes for better performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_StaffShifts_Date' AND object_id = OBJECT_ID('StaffShifts'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_StaffShifts_Date] 
    ON [dbo].[StaffShifts] ([Date] ASC);
    PRINT 'Created index IX_StaffShifts_Date';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_StaffShifts_StaffMemberId' AND object_id = OBJECT_ID('StaffShifts'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_StaffShifts_StaffMemberId] 
    ON [dbo].[StaffShifts] ([StaffMemberId] ASC);
    PRINT 'Created index IX_StaffShifts_StaffMemberId';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_StaffMembers_IsActive' AND object_id = OBJECT_ID('StaffMembers'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_StaffMembers_IsActive] 
    ON [dbo].[StaffMembers] ([IsActive] ASC);
    PRINT 'Created index IX_StaffMembers_IsActive';
END
GO

-- Step 4: Insert sample bartenders (optional - remove if not needed)
IF NOT EXISTS (SELECT * FROM [dbo].[StaffMembers])
BEGIN
    INSERT INTO [dbo].[StaffMembers] ([Name], [Role], [IsActive], [HireDate])
    VALUES 
        ('John Smith', 'Bartender', 1, GETUTCDATE()),
        ('Sarah Johnson', 'Bartender', 1, GETUTCDATE()),
        ('Mike Davis', 'Bartender', 1, GETUTCDATE());
    
    PRINT 'Inserted sample bartenders';
END
GO

-- Step 5: Verify the changes
SELECT 
    'StaffMembers' AS TableName,
    COUNT(*) AS RecordCount
FROM [dbo].[StaffMembers]
UNION ALL
SELECT 
    'StaffShifts' AS TableName,
    COUNT(*) AS RecordCount
FROM [dbo].[StaffShifts];
GO

PRINT 'Database migration completed successfully!';
GO
