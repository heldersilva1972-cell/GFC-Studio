-- Migration script to add DefaultCategoryId and Priority to FinanceVendors table

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinanceVendors') AND name = 'DefaultCategoryId')
BEGIN
    ALTER TABLE FinanceVendors ADD DefaultCategoryId INT NULL;
    
    ALTER TABLE FinanceVendors 
    ADD CONSTRAINT FK_FinanceVendors_FinanceCategories FOREIGN KEY (DefaultCategoryId) 
    REFERENCES FinanceCategories (Id) ON DELETE SET NULL;
    
    PRINT 'Added DefaultCategoryId column and FK constraint to FinanceVendors.';
END

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('FinanceVendors') AND name = 'Priority')
BEGIN
    ALTER TABLE FinanceVendors ADD Priority INT NOT NULL DEFAULT 3;
    PRINT 'Added Priority column with default value 3 (Low) to FinanceVendors.';
END
