-- Add missing DoorOrReader column to ControllerEvents
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('ControllerEvents') AND name = 'DoorOrReader')
BEGIN
    ALTER TABLE ControllerEvents ADD DoorOrReader INT NOT NULL DEFAULT 0;
    PRINT 'Added DoorOrReader column';
END
ELSE
BEGIN
    PRINT 'DoorOrReader column already exists';
END
GO

-- Add performance indices
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_Sync' AND object_id = OBJECT_ID('ControllerEvents'))
BEGIN
    CREATE INDEX IX_ControllerEvents_Sync ON ControllerEvents (ControllerId, RawIndex);
    PRINT 'Added IX_ControllerEvents_Sync index';
END
ELSE
BEGIN
    PRINT 'IX_ControllerEvents_Sync index already exists';
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ControllerEvents_Recent' AND object_id = OBJECT_ID('ControllerEvents'))
BEGIN
    CREATE INDEX IX_ControllerEvents_Recent ON ControllerEvents (ControllerId, TimestampUtc DESC, RawIndex DESC);
    PRINT 'Added IX_ControllerEvents_Recent index';
END
ELSE
BEGIN
    PRINT 'IX_ControllerEvents_Recent index already exists';
END
GO

PRINT 'Schema update complete';
