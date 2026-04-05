IF NOT EXISTS (SELECT 1 FROM AppPages WHERE PageRoute = '/admin/pos-terminal')
BEGIN
    INSERT INTO AppPages (PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder)
    VALUES ('Pos Terminal', '/admin/pos-terminal', 'POS Terminal interface', 'POS SYSTEM', 1, 1, 99);
END
ELSE
BEGIN
    UPDATE AppPages 
    SET Category = 'POS SYSTEM', IsActive = 1
    WHERE PageRoute = '/admin/pos-terminal';
END
GO
