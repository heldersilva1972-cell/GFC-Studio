-- =============================================
-- SEED DEMO DATA SCRIPT
-- Populates the GFC Database with sample data for testing the Dashboard.
-- =============================================

USE [ClubMembership];
GO

PRINT 'Seeding Demo Data...';

-- 1. Members
IF NOT EXISTS (SELECT 1 FROM Members)
BEGIN
    INSERT INTO Members (FirstName, LastName, Status, Email, Phone, ApplicationDate, AcceptedDate, IsNonPortugueseOrigin)
    VALUES 
    ('John', 'Doe', 'REGULAR', 'john.doe@example.com', '555-0101', '2023-01-15', '2023-02-01', 0),
    ('Jane', 'Smith', 'LIFE', 'jane.smith@example.com', '555-0102', '2010-05-20', '2010-06-01', 0),
    ('Bob', 'Johnson', 'REGULAR', 'bob.j@example.com', '555-0103', '2024-01-10', '2024-01-25', 1),
    ('Alice', 'Williams', 'REGULAR', 'alice.w@example.com', '555-0104', '2022-11-05', '2022-12-01', 0),
    ('Charlie', 'Brown', 'REGULAR', 'charlie.b@example.com', '555-0105', '2023-08-15', '2023-09-01', 0);
    PRINT '✓ Added 5 Members';
END

-- 2. Dues Payments (Current Year)
DECLARE @Year INT = YEAR(GETDATE());
DECLARE @Member1 INT = (SELECT TOP 1 MemberID FROM Members WHERE Email = 'john.doe@example.com');
DECLARE @Member2 INT = (SELECT TOP 1 MemberID FROM Members WHERE Email = 'jane.smith@example.com');

IF NOT EXISTS (SELECT 1 FROM DuesPayments WHERE Year = @Year)
BEGIN
    INSERT INTO DuesPayments (MemberID, Year, Amount, PaidDate, PaymentType, Notes)
    VALUES
    (@Member1, @Year, 150.00, GETDATE(), 'Cash', 'Paid at bar'),
    (@Member2, @Year, 150.00, GETDATE(), 'Check', 'Annual dues');
    PRINT '✓ Added Dues Payments';
END

-- 3. Board Assignments
DECLARE @PresPos INT = (SELECT PositionID FROM BoardPositions WHERE PositionName = 'President');
DECLARE @Member3 INT = (SELECT TOP 1 MemberID FROM Members WHERE Email = 'bob.j@example.com');

IF @PresPos IS NOT NULL AND @Member3 IS NOT NULL
BEGIN
    INSERT INTO BoardAssignments (MemberID, PositionID, TermYear, StartDate)
    VALUES (@Member3, @PresPos, @Year, GETDATE());
    PRINT '✓ Added Board Assignment';
END

-- 4. Key Cards
IF NOT EXISTS (SELECT 1 FROM KeyCards)
BEGIN
    INSERT INTO KeyCards (MemberID, CardNumber, IsActive, CreatedDate)
    VALUES 
    (@Member1, '10001', 1, GETDATE()),
    (@Member2, '10002', 1, GETDATE());
    PRINT '✓ Added Key Cards';
END

-- 5. Bar Sales (Mock)
IF NOT EXISTS (SELECT 1 FROM BarSaleEntries)
BEGIN
    INSERT INTO BarSaleEntries (SaleDate, TotalSales, TotalItemsSold)
    VALUES 
    (GETDATE(), 1250.50, 85),
    (DATEADD(day, -1, GETDATE()), 980.00, 62),
    (DATEADD(day, -2, GETDATE()), 1500.75, 110);
    PRINT '✓ Added Bar Sales History';
END

PRINT 'Demo Data Seeding Complete!';
