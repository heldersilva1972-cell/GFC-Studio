# Database Setup - Complete Guide

## Problem
You're getting "Invalid username or password" because your database is missing:
1. ASP.NET Identity tables (AspNetUsers, AspNetRoles, etc.)
2. Admin user
3. KeyCards tables

## Solution: Run Complete Setup

### ⚡ Quick Fix (Recommended)

**Double-click**: `SETUP_DATABASE_COMPLETE.bat`

This will:
1. ✅ Create all ASP.NET Identity tables
2. ✅ Create admin user (admin/Admin123!)
3. ✅ Create KeyCards tables
4. ✅ Set up all indexes and relationships

**Time**: ~30 seconds

---

## Manual Setup (If Batch File Fails)

### Step 1: Create Identity Schema

Open SSMS and run:
```
docs\DatabaseScripts\CREATE_IDENTITY_SCHEMA.sql
```

### Step 2: Create Admin User

Run:
```
docs\DatabaseScripts\CREATE_ADMIN_USER.sql
```

### Step 3: Create KeyCards Tables

Run:
```
docs\DatabaseScripts\CREATE_KEYCARDS_TABLE.sql
```

---

## After Setup

### 1. Restart Your Application

Stop and start your GFC Web App.

### 2. Login

- **Username**: `admin`
- **Password**: `Admin123!`

### 3. Change Password

**IMPORTANT**: Change the default password immediately for security!

---

## Verification

After running the setup, verify tables exist:

```sql
-- Connect to: (localdb)\MSSQLLocalDB
-- Database: ClubMembership

-- Check Identity tables
SELECT name FROM sys.tables 
WHERE name LIKE 'AspNet%'
ORDER BY name;

-- Should show:
-- AspNetRoleClaims
-- AspNetRoles
-- AspNetUserClaims
-- AspNetUserLogins
-- AspNetUserRoles
-- AspNetUsers
-- AspNetUserTokens

-- Check KeyCards tables
SELECT name FROM sys.tables 
WHERE name LIKE '%KeyCard%'
ORDER BY name;

-- Should show:
-- ControllerSyncQueue
-- KeyCardHistory
-- KeyCards

-- Verify admin user exists
SELECT UserName, Email FROM AspNetUsers;

-- Should show:
-- admin | admin@gfc.local
```

---

## Troubleshooting

### "Invalid object name" errors

Run `SETUP_DATABASE_COMPLETE.bat` again.

### "User already exists" error

The admin user is already created. Try logging in with:
- Username: `admin`
- Password: `Admin123!`

If you forgot the password, you'll need to reset it in the database.

### Connection errors

Check your `appsettings.json`:
```json
"ConnectionStrings": {
    "GFC": "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;..."
}
```

Make sure:
- Server is: `(localdb)\MSSQLLocalDB`
- Database is: `ClubMembership`

---

## Files Created

| File | Purpose |
|------|---------|
| `CREATE_IDENTITY_SCHEMA.sql` | Creates ASP.NET Identity tables |
| `CREATE_ADMIN_USER.sql` | Creates admin user |
| `CREATE_KEYCARDS_TABLE.sql` | Creates KeyCards tables |
| `SETUP_DATABASE_COMPLETE.bat` | Runs all scripts in order |
| `README_DATABASE_SETUP.md` | This file |

---

## Next Steps After Login

1. ✅ Change admin password
2. ✅ Create additional users if needed
3. ✅ Configure roles and permissions
4. ✅ Proceed with Cloudflare Tunnel setup (Issue 1)
5. ✅ Add PWA install banner (Issue 2)

---

**Run `SETUP_DATABASE_COMPLETE.bat` now to fix all database issues!**
