# Database Update Instructions - Bartender Schedule

## 📋 **What This Script Does**

This migration script will:
1. ✅ Create the `StaffMembers` table (for bartenders)
2. ✅ Create or update the `StaffShifts` table
3. ✅ Add the `MemberId` column to link bartenders to GFC members
4. ✅ Create performance indexes
5. ✅ Add 3 sample bartenders (optional)

---

## 🚀 **How to Run**

### **Option 1: SQL Server Management Studio (SSMS)**

1. Open **SQL Server Management Studio**
2. Connect to your GFC database server
3. Click **File** → **Open** → **File**
4. Navigate to and select: `Database_Migration_StaffManagement.sql`
5. Make sure the correct database is selected in the dropdown (should be your GFC database)
6. Click **Execute** (or press F5)
7. Check the **Messages** tab for success confirmations

### **Option 2: Command Line (sqlcmd)**

```bash
sqlcmd -S YOUR_SERVER_NAME -d YOUR_DATABASE_NAME -i "Database_Migration_StaffManagement.sql"
```

Replace:
- `YOUR_SERVER_NAME` with your SQL Server instance name
- `YOUR_DATABASE_NAME` with your GFC database name

---

## ✅ **Expected Output**

You should see messages like:
```
Created StaffMembers table
Added FK_StaffShifts_StaffMembers constraint
Created index IX_StaffShifts_Date
Created index IX_StaffShifts_StaffMemberId
Created index IX_StaffMembers_IsActive
Inserted sample bartenders
Database migration completed successfully!
```

If tables already exist, you'll see:
```
StaffMembers table already exists
```

---

## 📊 **What Gets Created**

### **StaffMembers Table**
```
Columns:
- Id (Primary Key)
- Name (Bartender name)
- Role (Always "Bartender")
- MemberId (Link to GFC Members - NULLABLE)
- PhoneNumber
- Email
- HourlyRate
- IsActive
- HireDate
- CreatedAt
- UpdatedAt
- Notes
```

### **StaffShifts Table**
```
Columns:
- Id (Primary Key)
- StaffMemberId (Foreign Key to StaffMembers)
- Date
- ShiftType (1=Day, 2=Night)
- Status
- CreatedAt
- UpdatedAt
```

---

## 🔍 **Verify the Migration**

After running the script, verify it worked:

```sql
-- Check if tables exist
SELECT name FROM sys.tables WHERE name IN ('StaffMembers', 'StaffShifts');

-- Check StaffMembers structure
EXEC sp_help 'StaffMembers';

-- View sample data
SELECT * FROM StaffMembers;
SELECT * FROM StaffShifts;
```

---

## ⚠️ **Important Notes**

1. **Safe to Re-run**: This script is idempotent - it checks if tables exist before creating them
2. **Sample Data**: The script adds 3 sample bartenders only if the table is empty
3. **No Data Loss**: Existing data in StaffShifts will be preserved
4. **Foreign Keys**: The script properly handles updating foreign key constraints

---

## 🗑️ **Optional: Remove Sample Data**

If you don't want the sample bartenders, either:

**Option A**: Comment out lines 108-118 in the SQL script before running

**Option B**: Delete them after running:
```sql
DELETE FROM StaffMembers WHERE Name IN ('John Smith', 'Sarah Johnson', 'Mike Davis');
```

---

## 📝 **Rollback (If Needed)**

If you need to undo the migration:

```sql
-- Drop tables (WARNING: This deletes all data!)
DROP TABLE IF EXISTS StaffShifts;
DROP TABLE IF EXISTS StaffMembers;
```

---

## ✅ **Next Steps After Migration**

1. Build the application: `dotnet build`
2. Run the application
3. Navigate to `/admin/staff-shifts`
4. Test the Bartender Schedule features:
   - Add bartenders
   - Link to existing members
   - Assign shifts
   - Switch between week/month views

---

**File Location**: `Database_Migration_StaffManagement.sql`

**Status**: ✅ Ready to run!
