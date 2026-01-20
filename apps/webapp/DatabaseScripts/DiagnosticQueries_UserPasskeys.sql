-- Diagnostic query to check UserPasskeys table and related data
-- Run this to see if there are any issues with the table structure or existing data

-- 1. Check if UserPasskeys table exists and its structure
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'UserPasskeys'
ORDER BY ORDINAL_POSITION;

-- 2. Check existing passkeys
SELECT 
    Id,
    UserId,
    CredentialId,
    FriendlyName,
    CreatedAtUtc,
    LastUsedUtc
FROM UserPasskeys
ORDER BY CreatedAtUtc DESC;

-- 3. Check if there are any orphaned passkeys (user doesn't exist)
SELECT 
    p.Id,
    p.UserId,
    p.CredentialId,
    p.FriendlyName,
    CASE WHEN u.UserId IS NULL THEN 'ORPHANED' ELSE 'OK' END AS Status
FROM UserPasskeys p
LEFT JOIN AppUsers u ON p.UserId = u.UserId;

-- 4. Check foreign key constraints
SELECT 
    fk.name AS ForeignKeyName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fkc.parent_object_id, fkc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    COL_NAME(fkc.referenced_object_id, fkc.referenced_column_id) AS ReferencedColumn
FROM sys.foreign_keys AS fk
INNER JOIN sys.foreign_key_columns AS fkc 
    ON fk.object_id = fkc.constraint_object_id
WHERE OBJECT_NAME(fk.parent_object_id) = 'UserPasskeys';

-- 5. Check recent AppUsers to verify UserId exists
SELECT TOP 5
    UserId,
    Username,
    IsActive,
    CreatedDate
FROM AppUsers
ORDER BY CreatedDate DESC;
