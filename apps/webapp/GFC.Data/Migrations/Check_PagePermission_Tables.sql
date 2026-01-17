-- Check if AppPages and UserPagePermissions tables exist
SELECT 
    CASE 
        WHEN EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AppPages') 
        THEN 'AppPages table EXISTS' 
        ELSE 'AppPages table DOES NOT EXIST' 
    END AS AppPagesStatus,
    CASE 
        WHEN EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UserPagePermissions') 
        THEN 'UserPagePermissions table EXISTS' 
        ELSE 'UserPagePermissions table DOES NOT EXIST' 
    END AS UserPagePermissionsStatus;

-- If they exist, check row counts
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AppPages')
BEGIN
    SELECT COUNT(*) AS AppPagesCount FROM AppPages;
END

IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UserPagePermissions')
BEGIN
    SELECT COUNT(*) AS UserPagePermissionsCount FROM UserPagePermissions;
END
