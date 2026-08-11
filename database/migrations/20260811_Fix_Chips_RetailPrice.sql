-- Fix RetailPrice for Chips in dbo.LiquorItems
IF EXISTS (SELECT 1 FROM dbo.LiquorItems WHERE Id = 41 AND Name = 'Chips')
BEGIN
    UPDATE dbo.LiquorItems
    SET RetailPrice = 1.75
    WHERE Id = 41 AND Name = 'Chips';
    PRINT 'Updated RetailPrice for Chips to 1.75';
END
ELSE
BEGIN
    UPDATE dbo.LiquorItems
    SET RetailPrice = 1.75
    WHERE Name = 'Chips';
    PRINT 'Updated RetailPrice for Chips by Name';
END
