# 🔧 QUICK FIX - Bartender Schedule Error

## ❌ Error You're Seeing:
```
Error loading shifts: Invalid column name 'MemberId'.
```

## ✅ Solution:

### **Run This Script:**
**`FIX_DATABASE_NOW.sql`**

---

## 🚀 How to Run:

### **Option 1: SQL Server Management Studio**
1. Open **SSMS**
2. Connect to your database
3. Open file: **`FIX_DATABASE_NOW.sql`**
4. Select your GFC database
5. Click **Execute** (F5)

### **Option 2: Command Line**
```bash
sqlcmd -S YOUR_SERVER -d YOUR_DATABASE -i "FIX_DATABASE_NOW.sql"
```

---

## 📋 What This Script Does:

✅ Adds the missing `MemberId` column to `StaffMembers` table  
✅ Verifies all columns exist  
✅ Shows you the table structure  
✅ Shows record counts  

---

## ⏱️ Expected Output:

```
========================================
Starting Bartender Schedule Database Fix
========================================

✓ Added MemberId column to StaffMembers table

Verifying StaffMembers table structure:
----------------------------------------
ColumnName      DataType    MaxLength   Nullable
Id              int         NULL        NO
Name            nvarchar    100         NO
Role            nvarchar    50          YES
MemberId        int         NULL        YES    ← NEW!
PhoneNumber     nvarchar    20          YES
Email           nvarchar    100         YES
...

Current record counts:
----------------------------------------
TableName       RecordCount
StaffMembers    3
StaffShifts     0

========================================
Database fix completed!
You can now refresh the Bartender Schedule page
========================================
```

---

## 🔄 After Running the Script:

1. **Refresh** the Bartender Schedule page in your browser
2. The error should be **gone**
3. You should see the bartender roster and schedule

---

## 🆘 If You Still See Errors:

The script will tell you if tables are missing. If so:
1. Run **`RUN_THIS_DATABASE_SCRIPT.sql`** first
2. Then run **`FIX_DATABASE_NOW.sql`**

---

**File to run**: `FIX_DATABASE_NOW.sql`

**Status**: ✅ Ready to fix the error!
