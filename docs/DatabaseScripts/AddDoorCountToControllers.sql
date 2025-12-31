IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'Controllers' AND COLUMN_NAME = 'DoorCount')
BEGIN
    ALTER TABLE Controllers ADD DoorCount INT NOT NULL DEFAULT 4;
    PRINT 'Added DoorCount column to Controllers table.';
END
ELSE
BEGIN
    PRINT 'DoorCount column already exists in Controllers table.';
END
GO
