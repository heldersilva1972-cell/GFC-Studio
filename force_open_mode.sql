UPDATE [dbo].[SystemSettings]
SET [AccessMode] = 'Open'
WHERE [Id] = 1;
GO

SELECT Id, AccessMode, PrimaryDomain FROM [dbo].[SystemSettings] WHERE Id = 1;
GO
