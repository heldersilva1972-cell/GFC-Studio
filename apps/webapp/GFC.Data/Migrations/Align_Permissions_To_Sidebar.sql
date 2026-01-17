-- 1. Reset all categories to match sidebar exactly (UPPERCASE as requested)
UPDATE AppPages SET Category = 'MEMBERSHIP' WHERE Category IN ('Members', 'Membership', 'Membership Section', 'MEMBERS');
UPDATE AppPages SET Category = 'CONTROLLERS' WHERE Category IN ('Controllers');
UPDATE AppPages SET Category = 'FINANCE' WHERE Category IN ('Finance', 'Finance & Ops');
UPDATE AppPages SET Category = 'ADMINISTRATION' WHERE Category IN ('Administration', 'Admin');
UPDATE AppPages SET Category = 'WEBSITE' WHERE Category IN ('Website');
UPDATE AppPages SET Category = 'HALL RENTALS' WHERE Category IN ('Hall Rentals', 'HallRentals');
UPDATE AppPages SET Category = 'SYSTEM' WHERE Category IN ('System');
UPDATE AppPages SET Category = 'CAMERA SYSTEM' WHERE Category IN ('Cameras', 'Camera System');
UPDATE AppPages SET Category = 'GFC STUDIO' WHERE Category IN ('Studio', 'GFC Studio');

-- 2. Target specific page names to match sidebar labels exactly
UPDATE AppPages SET PageName = 'Members Info' WHERE PageRoute = '/members';
UPDATE AppPages SET PageName = 'Dues/Payments' WHERE PageRoute = '/dues';
UPDATE AppPages SET PageName = 'Door Key Cards' WHERE PageRoute = '/keycards';
UPDATE AppPages SET PageName = 'Non-Portuguese Queue' WHERE PageRoute = '/np-queue';
UPDATE AppPages SET PageName = 'Club Bylaws' WHERE PageRoute = '/bylaws';
UPDATE AppPages SET PageName = 'Access Controllers' WHERE PageRoute = '/controllers';
UPDATE AppPages SET PageName = 'Search Controller' WHERE PageRoute = '/controllers/discovery';
UPDATE AppPages SET PageName = 'Access Holidays' WHERE PageRoute = '/controllers/schedules/holidays';
UPDATE AppPages SET PageName = 'Manage Users' WHERE PageRoute = '/users';
UPDATE AppPages SET PageName = 'Security & Policy' WHERE PageRoute = '/admin/security-settings';
UPDATE AppPages SET PageName = 'Legal & Documents' WHERE PageRoute = '/admin/documents';
UPDATE AppPages SET PageName = 'Data Export' WHERE PageRoute = '/export';
UPDATE AppPages SET PageName = 'Infrastructure Hub' WHERE PageRoute = '/admin/operations';
UPDATE AppPages SET PageName = 'Migration Wizard' WHERE PageRoute = '/admin/system/migration';
UPDATE AppPages SET PageName = 'Visual Editor' WHERE PageRoute = '/studio';

-- 3. Deactivate simulation and any pages that don't match the standard sidebar list
UPDATE AppPages SET IsActive = 0 WHERE Category = 'Simulation' OR Category = 'DASHBOARD';
UPDATE AppPages SET IsActive = 1 WHERE Category IN ('MEMBERSHIP', 'CONTROLLERS', 'FINANCE', 'ADMINISTRATION', 'WEBSITE', 'HALL RENTALS', 'SYSTEM', 'CAMERA SYSTEM', 'GFC STUDIO');

-- 4. Move "Life Eligibility" to MEMBERSHIP if it wandered off
UPDATE AppPages SET Category = 'MEMBERSHIP' WHERE PageName = 'Life Eligibility';

-- Ensure "Dashboard" still exists for reference but categorized correctly
UPDATE AppPages SET Category = 'GENERAL' WHERE PageName = 'Dashboard';
