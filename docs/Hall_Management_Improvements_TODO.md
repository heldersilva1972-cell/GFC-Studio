# Hall Management Dashboard - Improvements TODO

## Issues to Fix

### 1. ✅ Dark Mode Visibility
**Problem:** In dark mode, everything is difficult to see
**Solution:** Add dark mode specific CSS with better contrast

### 2. ✅ Hover States
**Problem:** When hovering over a date, it is difficult to see certain things
**Solution:** Improve hover states with better contrast and visibility

### 3. ✅ Scrolling Issues
**Problem:** When scrolling the upcoming events, the whole screen also scrolls
**Solution:** Fix overflow on sidebar lists to prevent body scroll

### 4. ✅ Modal Design
**Problem:** Rental request details look too plain, close button needs improvement
**Solution:** Modernize modal with gradients, better spacing, pill-style close button

### 5. ✅ Multiple Events Per Day
**Problem:** Should not allow more than one event per day
**Solution:** Add validation to check for existing events before allowing new ones

### 6. ✅ Click on Day
**Problem:** Should show rental request details when clicking on day, not just event
**Solution:** Update click handler to show all events for that day

### 7. ✅ Add Club Event Form
**Problem:** Form looks plain and outdated
**Solution:** Modernize with better styling, icons, and layout

### 8. ✅ Clickable Stat Cards
**Problem:** Tiles should be clickable to show filtered lists
**Solution:** Make stat cards clickable and show filtered modal

### 9. ❓ CD Command Issue
**Problem:** `cd apps/website` goes to studio directory
**Solution:** This is a terminal/shell issue, not related to the dashboard code

## Implementation Plan

### Phase 1: CSS Fixes (Dark Mode & Visibility)
- Add dark mode CSS variables
- Improve hover states
- Fix scrolling containers
- Better contrast for all elements

### Phase 2: Component Updates
- Modernize request details modal
- Modernize add club event modal
- Make stat cards clickable
- Add event validation

### Phase 3: Functionality
- Prevent multiple events per day
- Update day click handler
- Add filtered event lists

## Files to Modify

1. `HallManagementDashboard.razor.css` - Dark mode, hover states, scrolling
2. `HallManagementDashboard.razor` - Click handlers, validation, modals
3. Navigation/routing - Not related to dashboard

## Priority Order

1. **HIGH**: Dark mode visibility (affects usability)
2. **HIGH**: Scrolling issues (affects UX)
3. **MEDIUM**: Modal modernization (affects perception)
4. **MEDIUM**: Clickable stat cards (adds functionality)
5. **MEDIUM**: Multiple events validation (prevents errors)
6. **LOW**: CD command (terminal issue, not code issue)
