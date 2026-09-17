-- ==============================================================================
-- Migration Script: Register 'Hall Rentals' Page in AppPages
-- Description: Registers the /hall-rentals page under the 'HALL RENTALS' category
--              so it appears in User Management -> Page Access Permissions.
-- ==============================================================================

SET NOCOUNT ON;

PRINT 'Registering Hall Rentals page in AppPages...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[AppPages] WHERE [PageRoute] = '/hall-rentals')
BEGIN
    INSERT INTO [dbo].[AppPages] (
        [PageName],
        [PageRoute],
        [Description],
        [Category],
        [RequiresAdmin],
        [IsActive],
        [DisplayOrder]
    )
    VALUES (
        'Hall Rentals',
        '/hall-rentals',
        'Hall rentals overview, calendar synchronization, and public booking events',
        'HALL RENTALS',
        0,
        1,
        1
    );
    PRINT '✓ Successfully inserted Hall Rentals page into AppPages.';
END
ELSE
BEGIN
    UPDATE [dbo].[AppPages]
    SET [Category] = 'HALL RENTALS',
        [PageName] = 'Hall Rentals',
        [IsActive] = 1
    WHERE [PageRoute] = '/hall-rentals';
    PRINT '✓ Hall Rentals page already registered. Updated category and active state.';
END
GO
