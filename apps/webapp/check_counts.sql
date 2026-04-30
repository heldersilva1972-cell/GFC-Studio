
SELECT 'Members' as TableName, COUNT(*) as RecordCount FROM dbo.Members
UNION ALL
SELECT 'Dues' as TableName, COUNT(*) as RecordCount FROM dbo.Dues
UNION ALL
SELECT 'ControllerEvents' as TableName, COUNT(*) as RecordCount FROM dbo.ControllerEvents
UNION ALL
SELECT 'StaffShifts' as TableName, COUNT(*) as RecordCount FROM dbo.StaffShifts
UNION ALL
SELECT 'BarSaleEntries' as TableName, COUNT(*) as RecordCount FROM dbo.BarSaleEntries;
