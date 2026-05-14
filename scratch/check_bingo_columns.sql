-- Diagnostic: Check Bingo Table Columns
USE ClubMembership;

PRINT 'Checking BingoSessions columns...';
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BingoSessions'
ORDER BY ORDINAL_POSITION;

PRINT 'Checking BingoGameEntries columns...';
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BingoGameEntries'
ORDER BY ORDINAL_POSITION;

PRINT 'Checking BingoAdmissionEntries columns...';
SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'BingoAdmissionEntries'
ORDER BY ORDINAL_POSITION;

PRINT 'Checking Foreign Keys for Bingo tables...';
SELECT 
    f.name AS ForeignKeyName,
    OBJECT_NAME(f.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME (f.referenced_object_id) AS ReferencedTable,
    COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedColumn,
    delete_referential_action_desc AS DeleteAction
FROM sys.foreign_keys AS f
INNER JOIN sys.foreign_keys_columns AS fc 
   ON f.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(f.parent_object_id) IN ('BingoGameEntries', 'BingoAdmissionEntries', 'BingoLotteryTransactions');
