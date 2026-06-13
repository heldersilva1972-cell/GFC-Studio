-- Migration script to add FinanceLoans and update FinancePayments

-- 1. Create FinanceLoans table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FinanceLoans')
BEGIN
    CREATE TABLE FinanceLoans (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        LenderName NVARCHAR(250) NOT NULL,
        OriginDate DATETIME NOT NULL,
        OriginalBalance DECIMAL(18,2) NOT NULL,
        CurrentBalance DECIMAL(18,2) NOT NULL,
        Notes NVARCHAR(MAX) NULL,
        IsActive BIT NOT NULL DEFAULT 1,
        CreatedAt DATETIME NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Created FinanceLoans table.';
END

-- 2. Alter FinancePayments BillId to be nullable
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FinancePayments_Bills')
BEGIN
    ALTER TABLE FinancePayments DROP CONSTRAINT FK_FinancePayments_Bills;
    PRINT 'Dropped foreign key FK_FinancePayments_Bills.';
END

IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinancePayments') AND name = 'BillId')
BEGIN
    ALTER TABLE FinancePayments ALTER COLUMN BillId INT NULL;
    PRINT 'Altered BillId to be nullable.';
END

-- Recreate foreign key for BillId referencing FinanceBills
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_FinancePayments_FinanceBills_BillId')
BEGIN
    ALTER TABLE FinancePayments ADD CONSTRAINT FK_FinancePayments_FinanceBills_BillId 
    FOREIGN KEY (BillId) REFERENCES FinanceBills(Id) ON DELETE CASCADE;
    PRINT 'Re-created FK_FinancePayments_FinanceBills_BillId constraint.';
END

-- 3. Add LoanId column and foreign key to FinancePayments
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinancePayments') AND name = 'LoanId')
BEGIN
    ALTER TABLE FinancePayments ADD LoanId INT NULL;
    
    ALTER TABLE FinancePayments ADD CONSTRAINT FK_FinancePayments_FinanceLoans_LoanId
    FOREIGN KEY (LoanId) REFERENCES FinanceLoans(Id) ON DELETE SET NULL;
    
    PRINT 'Added LoanId column and constraint FK_FinancePayments_FinanceLoans_LoanId.';
END
