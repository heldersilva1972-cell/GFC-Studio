# Page-Level Access Control Implementation

## Overview
Implemented a comprehensive page-level access control system that allows administrators to manage individual user permissions for each page in the GFC Studio V2 application.

## Features Implemented

### 1. Database Layer
- **New Tables:**
  - `AppPages`: Stores all application pages/routes with metadata
  - `UserPagePermissions`: Manages user-specific page access permissions
  
- **Migration Script:** `Database/Migrations/AddPagePermissions.sql`
  - Creates tables with proper foreign keys and indexes
  - Seeds 35+ default application pages organized by category
  - Includes pages for Members, Access Control, Controllers, Financial, Administration, and Simulation

### 2. Data Models
- **`AppPage`**: Represents a page/route in the application
  - Properties: PageId, PageName, PageRoute, Description, Category, RequiresAdmin, IsActive, DisplayOrder
  
- **`UserPagePermission`**: Represents user's access to a specific page
  - Properties: PermissionId, UserId, PageId, CanAccess, GrantedDate, GrantedBy

### 3. Repository Layer
- **`IPagePermissionRepository`** & **`PagePermissionRepository`**
  - Page management methods (GetAllPages, GetActivePages, GetPagesByCategory, etc.)
  - Permission management (GetUserPermissions, HasPermission, etc.)
  - CRUD operations (GrantPermission, RevokePermission, SetUserPermissions)
  - Bulk operations (GrantAllPermissions, CopyPermissions)

### 4. Service Layer
- **Extended `IUserManagementService`** with page permission methods:
  - `GetAllPages()`: Get all pages
  - `GetActivePages()`: Get active pages only
  - `GetUserPagePermissions(userId)`: Get user's permissions
  - `UserHasPageAccess(userId, pageRoute)`: Check access
  - `SetUserPagePermissions(userId, pageIds, grantedBy)`: Set permissions
  - `GrantAllPagePermissions(userId, grantedBy)`: Grant all
  - `CopyUserPermissions(sourceUserId, targetUserId, grantedBy)`: Copy from another user

### 5. UI Components

#### UserPagePermissionsModal.razor
A dedicated modal component for managing page permissions:
- **Category-based organization**: Pages grouped by category (Members, Controllers, Financial, etc.)
- **Bulk actions**: Select All / Clear All buttons
- **Visual feedback**: Selected pages highlighted with background color
- **Admin protection**: Admin-only pages clearly marked and disabled for non-admin users
- **Admin bypass**: Administrators automatically have access to all pages
- **Responsive design**: Grid layout with 2 columns on medium+ screens

#### UserManagement.razor Updates
- Added "Page Access" button to each user's action menu
- Integrated UserPagePermissionsModal component
- State management for modal visibility and selected user
- Callback handling for save operations

### 6. Modern UI Enhancements
Created `user-management-animations.css` with:

#### Animations
- **fadeInUp**: Smooth page load animation
- **slideInRight**: Table row entrance animation
- **scaleIn**: Card pop-in effect
- **modalSlideIn**: Modal entrance animation
- **pulse**: Icon hover effect
- **shimmer**: Loading skeleton animation

#### Interactive Effects
- **Button hover**: Lift effect with shadow
- **Button click**: Ripple effect
- **Table row hover**: Highlight and slide effect
- **Badge hover**: Scale and shadow
- **Tab hover**: Underline animation
- **Form focus**: Scale and glow effect
- **Checkbox check**: Bounce animation

#### Accessibility
- Respects `prefers-reduced-motion` for users who need reduced animations
- Enhanced focus states with visible outlines
- Smooth transitions for all state changes

## Page Categories Seeded

1. **Main**
   - Dashboard
   - Simulation Dashboard

2. **Members**
   - Members, Member Detail, Directors, Life Eligibility

3. **Access Control**
   - Key Cards, Physical Keys, Member Access Control

4. **Controllers**
   - Controllers Dashboard, Status, Events, Access Admin, Door Config, Network Config, Schedules, Auto Open, Advanced Modes

5. **Financial**
   - Dues, Lottery, NP Queue, Reimbursements (multiple pages)

6. **Administration** (Admin-only)
   - User Management, Data Export, Settings, Audit Logs, Notification Preferences, System Status

7. **Simulation**
   - Card Reader Emulator, Controller Visualizer

## Security Features

1. **Admin Bypass**: Administrators automatically have access to all pages
2. **Admin-Only Pages**: Certain pages (marked with `RequiresAdmin`) can only be accessed by admins
3. **Granular Control**: Non-admin users can be granted access to specific pages
4. **Audit Trail**: Tracks who granted permissions and when
5. **Cascade Delete**: Permissions are automatically removed when users are deleted

## Usage

### For Administrators
1. Navigate to User Management (`/users`)
2. Click the "Page Access" button next to any user
3. Select/deselect pages the user should have access to
4. Use "Select All" or "Clear All" for bulk operations
5. Click "Save Permissions" to apply changes

### Permission Checking (for future implementation)
```csharp
// Check if user has access to a page
bool hasAccess = UserService.UserHasPageAccess(userId, "/members");

// In a Blazor component
var hasAccess = await UserService.UserHasPageAccess(currentUserId, Navigation.Uri);
```

## Files Created/Modified

### New Files
- `GFC.Core/Models/PagePermission.cs`
- `GFC.Core/Interfaces/IPagePermissionRepository.cs`
- `GFC.Core/DTOs/PagePermissionDto.cs`
- `GFC.Data/Repositories/PagePermissionRepository.cs`
- `Database/Migrations/AddPagePermissions.sql`
- `GFC.BlazorServer/Components/Pages/UserPagePermissionsModal.razor`
- `GFC.BlazorServer/wwwroot/css/user-management-animations.css`

### Modified Files
- `GFC.Core/Interfaces/IUserManagementService.cs`
- `GFC.Core/Services/UserManagementService.cs`
- `GFC.BlazorServer/Program.cs`
- `GFC.BlazorServer/Components/Pages/UserManagement.razor`

## Next Steps (Optional Enhancements)

1. **Authorization Middleware**: Create a custom authorization handler to enforce page permissions
2. **Navigation Guard**: Automatically redirect users if they don't have access to a page
3. **Permission Templates**: Create role-based templates (e.g., "Director", "Treasurer") for quick permission assignment
4. **Permission History**: Track permission changes over time
5. **Bulk User Management**: Apply permissions to multiple users at once
6. **Page Usage Analytics**: Track which pages users access most frequently

## Testing Checklist

- [ ] Run database migration
- [ ] Verify tables are created with seed data
- [ ] Test opening Page Access modal for a user
- [ ] Test selecting/deselecting pages
- [ ] Test "Select All" and "Clear All" buttons
- [ ] Test saving permissions
- [ ] Verify admin users show "Admin" badge and can't have permissions restricted
- [ ] Test animations and hover effects
- [ ] Verify responsive design on different screen sizes
- [ ] Test with reduced motion preference enabled
