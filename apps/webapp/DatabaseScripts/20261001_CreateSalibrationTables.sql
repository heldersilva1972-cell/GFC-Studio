-- Migration: Create Salibration tables and register mobile permissions
-- Added by Antigravity for GFC Studio

-- 1. Create Salibration Participants Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SalibrationParticipants')
BEGIN
    CREATE TABLE SalibrationParticipants (
        Id INT PRIMARY KEY IDENTITY(1,1),
        FullName NVARCHAR(200) NOT NULL,
        PhoneNumber NVARCHAR(50) NOT NULL,
        Email NVARCHAR(200) NULL,
        Notes NVARCHAR(MAX) NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        CreatedBy NVARCHAR(100) NULL
    );
END
GO

-- 2. Create Salibration Tickets Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SalibrationTickets')
BEGIN
    CREATE TABLE SalibrationTickets (
        Id INT PRIMARY KEY IDENTITY(1,1),
        ParticipantId INT NOT NULL,
        TicketNumber NVARCHAR(50) NOT NULL,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
        IsWinner BIT NOT NULL DEFAULT 0,
        WonAt DATETIME NULL,
        CONSTRAINT FK_SalibrationTickets_Participant FOREIGN KEY (ParticipantId) REFERENCES SalibrationParticipants(Id) ON DELETE CASCADE
    );

    CREATE INDEX IX_SalibrationTickets_TicketNumber ON SalibrationTickets(TicketNumber);
END
GO

-- 3. Create Salibration Settings Table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SalibrationSettings')
BEGIN
    CREATE TABLE SalibrationSettings (
        Id INT PRIMARY KEY IDENTITY(1,1),
        PageTitle NVARCHAR(100) NOT NULL DEFAULT 'Sal-ibration',
        TicketPrefix NVARCHAR(50) NOT NULL DEFAULT '1987',
        VariableDigitCount INT NOT NULL DEFAULT 3,
        ThankYouMessage NVARCHAR(MAX) NOT NULL DEFAULT 'Thank you so much for your generosity and incredible support! Your contribution makes a real difference in our community fundraiser. We’ve saved your numbers — good luck in the drawing! 🌟',
        ThankYouDisplaySeconds INT NOT NULL DEFAULT 5,
        UpdatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );

    INSERT INTO SalibrationSettings (PageTitle, TicketPrefix, VariableDigitCount, ThankYouMessage, ThankYouDisplaySeconds)
    VALUES ('Sal-ibration', '1987', 3, 'Thank you so much for your generosity and incredible support! Your contribution makes a real difference in our community fundraiser. We’ve saved your numbers — good luck in the drawing! 🌟', 5);
END
ELSE IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('SalibrationSettings') AND name = 'ThankYouDisplaySeconds')
BEGIN
    ALTER TABLE SalibrationSettings ADD ThankYouDisplaySeconds INT NOT NULL DEFAULT 5;
END
GO

-- 4. Register Sal-ibration in AppPages for Permissions Management
IF NOT EXISTS (SELECT * FROM AppPages WHERE PageRoute = '/mobile/salibration' OR PageName = 'Sal-ibration (Mobile)')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Sal-ibration (Mobile)', '/mobile/salibration', 'Raffle ticket registration, auto-advancing tiles & winner lookup', 'Mobile', 0, 1, 140);
END
GO
