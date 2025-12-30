# Key Card vs Fob Tracking Implementation

## Overview
This feature allows administrators to specify whether they're assigning a **Key Card** or **Key Fob** during the assignment process. The system now tracks this information throughout the entire lifecycle of the access device.

## Changes Made

### 1. Database Schema
- **File**: `docs/database/migrations/add_keytype_to_keyhistory.sql`
- Added `KeyType` column (NVARCHAR(50)) to the `KeyHistory` table
- Existing records are defaulted to 'Card' for backward compatibility

### 2. Core Models & Interfaces
- **IKeyCardRepository** (`GFC.Core/Interfaces/IKeyCardRepository.cs`)
  - Updated `Create` method signature to accept `cardType` parameter (defaults to "Card")
  
- **KeyHistory Entity** (`GFC.BlazorServer/Data/Entities/KeyHistory.cs`)
  - Added `KeyType` property with `[MaxLength(50)]` attribute

### 3. Repository Layer
- **KeyCardRepository** (`GFC.Data/Repositories/KeyCardRepository.cs`)
  - Modified `Create` method to insert the specified card type into the database
  - Updated SQL INSERT statement to include `@CardType` parameter

### 4. Service Layer
- **KeyHistoryService** (`GFC.BlazorServer/Services/KeyHistoryService.cs`)
  - Updated `LogAssignmentAsync` to accept and store `keyType` parameter (defaults to "Card")
  - Updated `LogRevocationAsync` to accept and store `keyType` parameter
  - Both methods now persist the device type in the history logs

### 5. User Interface
- **KeyCards.razor** (`GFC.BlazorServer/Components/Pages/KeyCards.razor`)
  - Added **Step 3: Select Card Type** in the assignment workflow
  - Visual selection interface with Card and Fob options
  - Updated confirmation summary (Step 4) to display selected device type
  - Modified final feedback step to Step 5
  - Added `_selectedCardType` variable (defaults to "Card")
  - Updated `ConfirmAssignment` method to pass card type to repository and history service
  - Added `HandleProceedToCardType` helper method
  - Updated `HandleBackFromConfirm` to navigate to card type selection
  - Reset card type selection when opening new assignment modal

- **KeyCards.razor.css** (`GFC.BlazorServer/Components/Pages/KeyCards.razor.css`)
  - Added comprehensive styling for card type selection interface
  - Responsive grid layout for Card/Fob options
  - Hover effects and selection states
  - Smooth transitions and visual feedback

## User Workflow

### Assignment Process (5 Steps)
1. **Scan Card**: Administrator scans the physical card/fob number
2. **Select Member**: Choose which member to assign the device to
3. **Select Device Type**: Choose between "Key Card" or "Key Fob" *(NEW)*
4. **Confirm**: Review assignment details including device type
5. **Complete**: Confirmation and sync status

### Visual Features
- **Card Option**: Credit card icon with "Standard access card" description
- **Fob Option**: Key icon with "Keychain access fob" description
- Selected option displays with:
  - Highlighted border (purple gradient)
  - Background tint
  - Check mark indicator
  - Elevated shadow effect

## Database Migration

Run the following script on your `ClubMembership` database:

```sql
sqlcmd -S . -d ClubMembership -i "docs/database/migrations/add_keytype_to_keyhistory.sql"
```

Or execute directly:
```sql
USE ClubMembership;
GO

IF NOT EXISTS (
    SELECT * FROM sys.columns 
    WHERE object_id = OBJECT_ID('dbo.KeyHistory') 
    AND name = 'KeyType'
)
BEGIN
    ALTER TABLE dbo.KeyHistory ADD KeyType NVARCHAR(50) NULL;
END
GO

UPDATE dbo.KeyHistory SET KeyType = 'Card' WHERE KeyType IS NULL;
GO
```

## Benefits

1. **Accurate Inventory**: Track exactly how many cards vs fobs are in circulation
2. **Better Reporting**: Generate reports showing device type distribution
3. **Audit Trail**: Complete history of what type of device was assigned to each member
4. **Replacement Tracking**: Know what type of device to replace when needed
5. **Hardware Planning**: Better understand which device types are preferred

## Future Enhancements

Potential future improvements:
- Display device type in the Card Assignments tab
- Filter/search by device type
- Reporting dashboard showing Card vs Fob statistics
- Bulk device type updates
- Device type preferences per member

## Testing Checklist

- [ ] Database migration runs successfully
- [ ] New card assignments show device type selection
- [ ] Device type is saved correctly in database
- [ ] Device type appears in confirmation summary
- [ ] History logs show correct device type
- [ ] Existing assignments default to "Card"
- [ ] UI is responsive and visually appealing
- [ ] Back/forward navigation works correctly
- [ ] Card replacement flow works with device types

## Notes

- Default device type is "Card" for backward compatibility
- The system supports any string value for KeyType, but UI currently offers "Card" and "Fob"
- Device type is tracked in both `KeyCards` table (via CardType column) and `KeyHistory` table
- All historical assignments are automatically set to "Card" type during migration
