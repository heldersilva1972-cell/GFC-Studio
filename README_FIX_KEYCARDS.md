# FIX: Missing KeyCards Table

## Problem
Your application is crashing with: `Invalid object name 'dbo.KeyCards'`

## Root Cause
The KeyCards table doesn't exist in your LocalDB database.

## Solution (Choose ONE method)

### ⚡ METHOD 1: PowerShell Script (EASIEST - RECOMMENDED)

**Right-click** `FIX_KEYCARDS_LOCALDB.ps1` → **Run with PowerShell**

OR from PowerShell:
```powershell
.\FIX_KEYCARDS_LOCALDB.ps1
```

### 🗄️ METHOD 2: SQL Server Management Studio

1. Open **SSMS**
2. Connect to: `(localdb)\MSSQLLocalDB`
3. Open file: `docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql`
4. Press **F5** to execute

### 💻 METHOD 3: Command Line

Double-click: `CREATE_KEYCARDS_NOW.bat`

## After Running

1. **RESTART** your GFC application (stop and start it)
2. The error will be **GONE**
3. Test your application

## What Gets Created

✅ KeyCards table (9 columns)
✅ KeyCardHistory table (9 columns)  
✅ ControllerSyncQueue table (10 columns)
✅ All necessary indexes

## Your Database Connection

- **Server**: `(localdb)\MSSQLLocalDB`
- **Database**: `ClubMembership`
- **Type**: LocalDB (not SQL Server Express)

---

**TIP**: Use METHOD 1 (PowerShell script) - it's the fastest and shows clear output!
