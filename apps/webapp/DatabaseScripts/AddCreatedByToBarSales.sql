IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'BarSaleEntries' AND COLUMN_NAME = 'CreatedBy')
BEGIN
    ALTER TABLE BarSaleEntries ADD CreatedBy NVARCHAR(100) NULL;
    PRINT 'Added CreatedBy column to BarSaleEntries table.';
END
ELSE
BEGIN
    PRINT 'CreatedBy column already exists in BarSaleEntries table.';
END
