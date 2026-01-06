# 🔍 System Migration Wizard Error - Diagnostic Guide

## ❌ **Error Observed:**
"Something went wrong" when clicking "Create & Start" on `/admin/system/migration`

## ✅ **Services ARE Registered:**
Lines 252-253 in Program.cs confirm:
```csharp
builder.Services.AddScoped<GFC.BlazorServer.Services.Migration.IMigrationService, ...>();
builder.Services.AddScoped<GFC.BlazorServer.Services.Migration.IMigrationReportService, ...>();
```

## 🔍 **How to Diagnose:**

### **Step 1: Check Browser Console**
1. Press `F12` to open Developer Tools
2. Go to "Console" tab
3. Look for red error messages
4. Take a screenshot or copy the error text

### **Step 2: Check Application Logs**
Look in the terminal/console where the app is running for error messages.

### **Step 3: Common Causes:**

#### **A. Database Table Missing**
The `MigrationProfiles` table might not exist.

**Fix:**
```powershell
sqlcmd -S .\ClubMembership -d ClubMembership -i "docs\DatabaseScripts\Manual_MigrationWizard_Schema.sql"
```

#### **B. Null Reference in CreateProfile**
The `CreateProfile` method might be failing.

**Check:** Line 351-361 in `MigrationWizard.razor`

#### **C. Missing GatesStatusJson**
The profile creation might fail if GatesStatusJson is null.

#### **D. Authentication Issue**
The user context might not be available.

---

## 🔧 **Quick Fixes to Try:**

### **Fix 1: Restart the Application**
```powershell
# Stop the app (Ctrl+C)
# Then restart
dotnet run
```

### **Fix 2: Clear Browser Cache**
- Press `Ctrl + Shift + Delete`
- Clear "Cached images and files"
- Hard reload: `Ctrl + F5`

### **Fix 3: Check Database**
```sql
-- Check if table exists
SELECT * FROM sys.tables WHERE name = 'MigrationProfiles'

-- Check if any profiles exist
SELECT * FROM MigrationProfiles
```

---

## 📋 **What to Check Next:**

1. **Browser Console Error** - This will tell us exactly what's failing
2. **Application Logs** - Look for exceptions in the terminal
3. **Database State** - Verify the MigrationProfiles table exists

---

## 🎯 **Most Likely Cause:**

Based on the error location (when clicking "Create & Start"), the issue is probably:

1. **Database table doesn't exist** - Run the migration script
2. **Null reference in service** - Check if `MigrationService.CreateProfileAsync` is working
3. **Authentication context missing** - User identity might not be available

---

## 📞 **Next Steps:**

Please provide:
1. Screenshot of browser console (F12 → Console tab)
2. Any error messages from the terminal where the app is running
3. Result of this SQL query:
   ```sql
   SELECT * FROM sys.tables WHERE name = 'MigrationProfiles'
   ```

This will help me pinpoint the exact issue!
