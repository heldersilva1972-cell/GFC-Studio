-- =============================================
-- Make AllowedPcIp nullable so EF can write NULL safely
-- =============================================
-- Run this script once against the ClubMembership database

IF COL_LENGTH('dbo.ControllerNetworkConfigs', 'AllowedPcIp') IS NOT NULL
BEGIN
    -- Drop default constraint if it exists (name may differ, so we resolve it first)
    DECLARE @dfName sysname;
    DECLARE @sql nvarchar(max);

    SELECT @dfName = df.name
    FROM sys.default_constraints df
    JOIN sys.columns c
        ON df.parent_object_id = c.object_id
       AND df.parent_column_id = c.column_id
    WHERE df.parent_object_id = OBJECT_ID('dbo.ControllerNetworkConfigs')
      AND c.name = 'AllowedPcIp';

    IF @dfName IS NOT NULL
    BEGIN
        SET @sql = N'ALTER TABLE [dbo].[ControllerNetworkConfigs] DROP CONSTRAINT [' + @dfName + N'];';
        EXEC sp_executesql @sql;
    END;

    ALTER TABLE [dbo].[ControllerNetworkConfigs]
        ALTER COLUMN [AllowedPcIp] NVARCHAR(45) NULL;
END;
GO

PRINT 'ControllerNetworkConfigs.AllowedPcIp is now nullable (NVARCHAR(45) NULL).';
