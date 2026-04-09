IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('LiquorItems') AND name = 'ShowInPos')
BEGIN
    ALTER TABLE LiquorItems ADD ShowInPos BIT NOT NULL DEFAULT 1;
END
GO
