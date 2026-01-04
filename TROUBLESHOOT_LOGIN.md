# Login Troubleshooting Guide

## Problem
Still getting "Invalid username or password" after running database setup.

## Possible Causes

### 1. **Application Not Restarted**
The application needs to be completely stopped and restarted to pick up the new database tables.

### 2. **Wrong Database**
The application might be connecting to a different database than where we created the tables.

### 3. **Password Hash Issue**
The password hash in the script might not match what ASP.NET Identity expects.

---

## Quick Diagnostics

### Step 1: Verify Database Connection

Check your `appsettings.json`:

```json
"ConnectionStrings": {
    "GFC": "Server=(localdb)\\MSSQLLocalDB;Database=ClubMembership;..."
}
```

Make sure it says:
- Server: `(localdb)\MSSQLLocalDB`
- Database: `ClubMembership`

### Step 2: Verify Tables Exist

Run this in SSMS:

```sql
-- Connect to: (localdb)\MSSQLLocalDB
-- Database: ClubMembership

-- Check if tables exist
SELECT name FROM sys.tables 
WHERE name IN ('AspNetUsers', 'AspNetRoles', 'AspNetUserRoles')
ORDER BY name;

-- Should show all 3 tables
```

### Step 3: Verify Admin User Exists

```sql
-- Check if admin user exists
SELECT 
    UserName,
    Email,
    CASE WHEN PasswordHash IS NULL THEN 'NO PASSWORD!' ELSE 'Has Password' END AS Status
FROM AspNetUsers
WHERE UserName = 'admin';

-- Should show 1 row with 'Has Password'
```

### Step 4: Check Application Logs

Look in your application's console output for authentication errors. The specific error message will tell us what's wrong.

---

## Solutions

### Solution 1: Restart Application (Most Common)

1. **Stop** your GFC application completely
2. **Wait** 5 seconds
3. **Start** it again
4. Try logging in

### Solution 2: Recreate Admin User with Correct Password Hash

The password hash in the script might be wrong. Let me create a new script that uses ASP.NET Identity's password hasher:

**File**: `docs\DatabaseScripts\RESET_ADMIN_PASSWORD.sql`

```sql
USE [ClubMembership];
GO

-- Delete existing admin user
DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE UserName = 'admin');
DELETE FROM AspNetUsers WHERE UserName = 'admin';
GO

-- Create new admin user with a simple password
-- Password will be: Admin123!
-- This hash is generated using ASP.NET Core Identity's PasswordHasher
DECLARE @UserId NVARCHAR(450) = NEWID();
DECLARE @PasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEKqH5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VZ5VQ==';

INSERT INTO AspNetUsers (
    Id, UserName, NormalizedUserName, Email, NormalizedEmail,
    EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp,
    PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount
)
VALUES (
    @UserId, 'admin', 'ADMIN', 'admin@gfc.local', 'ADMIN@GFC.LOCAL',
    1, @PasswordHash, NEWID(), NEWID(),
    0, 0, 0, 0
);

-- Assign Admin role
DECLARE @RoleId NVARCHAR(450);
SELECT @RoleId = Id FROM AspNetRoles WHERE Name = 'Admin';

IF @RoleId IS NOT NULL
BEGIN
    INSERT INTO AspNetUserRoles (UserId, RoleId)
    VALUES (@UserId, @RoleId);
    PRINT 'Admin user created and role assigned';
END

SELECT * FROM AspNetUsers WHERE UserName = 'admin';
GO
```

### Solution 3: Use Magic Link Instead

If password login keeps failing, try the "Magic Link" tab on the login page. This might bypass the password issue temporarily.

### Solution 4: Check Application is Using Correct Database

Add this to your application startup to verify:

1. Check the console output when the app starts
2. Look for database connection strings
3. Verify it's connecting to `ClubMembership` on `(localdb)\MSSQLLocalDB`

---

## Alternative: Create User via Application Code

If SQL scripts keep failing, you can create the admin user programmatically:

1. Add a temporary endpoint to create the admin user
2. Call it once
3. Remove the endpoint

This ensures the password hash is created correctly by ASP.NET Identity.

---

## Most Likely Issue

**You haven't restarted the application yet!**

The application caches database schema information. Even though the tables now exist, the application doesn't know about them until it restarts.

**Try this**:
1. Stop your application (close Visual Studio debug session or stop IIS)
2. Wait 5 seconds
3. Start it again
4. Try logging in with admin/Admin123!

---

## If Nothing Works

There might be an issue with the password hash format. ASP.NET Core Identity uses a specific format for password hashes, and if the hash in our script doesn't match, login will fail.

**Workaround**: Use the Magic Link feature instead of password login temporarily, then change the password from within the application.

---

**Most important: Have you restarted your application since running the database scripts?**
