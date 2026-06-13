-- Migration script to add AccountNumber to FinanceVendors table

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinanceVendors') AND name = 'AccountNumber')
BEGIN
    ALTER TABLE FinanceVendors ADD AccountNumber NVARCHAR(100) NULL;
    PRINT 'Added AccountNumber column to FinanceVendors.';
END
