# Shift Assignment Debugging Guide

## 🔧 Changes Made to Help Debug

### **1. Added Status Field**
Shifts now get a default status of "Scheduled" when created:
```csharp
newShift.Status = "Scheduled";
```

### **2. Added Console Logging**
The application now logs to the browser console:

**When saving a shift:**
- Success message shows date
- Errors are logged to console

**When loading shifts:**
- Shows count of shifts loaded
- Lists each shift with details
- Logs any errors

### **3. Better Error Messages**
- "Selected bartender not found" if staff lookup fails
- "Please select a bartender" if none selected
- Full exception details in console

---

## 🔍 How to Debug

### **Step 1: Open Browser Console**
1. Press **F12** in your browser
2. Click the **Console** tab
3. Keep it open while testing

### **Step 2: Assign a Shift**
1. Click a "+ Assign" button
2. Select a bartender
3. Click "Assign Shift"
4. **Watch the console** for messages

### **Step 3: Check What's Logged**

**You should see:**
```
Loaded X shifts for week starting Dec 24
  - Shift 1: John Smith on Dec 24 (Day)
  - Shift 2: Sarah Johnson on Dec 25 (Night)
```

**If you see 0 shifts:**
- The shift might not be saving
- Check for error messages in console
- Check database directly

---

## 🗄️ **Check Database Directly**

Run this SQL query to see if shifts are being saved:

```sql
-- Check if shifts exist
SELECT 
    ss.Id,
    ss.Date,
    ss.ShiftType,
    ss.Status,
    ss.StaffMemberId,
    sm.Name as StaffName
FROM StaffShifts ss
LEFT JOIN StaffMembers sm ON ss.StaffMemberId = sm.Id
ORDER BY ss.Date DESC;
```

**What to look for:**
- ✅ Rows exist = Shifts are being saved
- ✅ StaffMemberId matches a real StaffMember
- ✅ StaffName appears in the join
- ❌ No rows = Shifts aren't saving (check error messages)
- ❌ StaffMemberId doesn't match = Foreign key issue

---

## 🐛 **Common Issues & Solutions**

### **Issue 1: Shift saves but doesn't appear**
**Cause:** Week/month view is showing wrong date range

**Solution:** 
- Check the console log for "Loaded X shifts"
- Make sure the date range includes your shift
- Try navigating to next/previous week

### **Issue 2: "Error assigning shift" message**
**Cause:** Database constraint violation or connection issue

**Solution:**
- Check console for full error
- Verify StaffMembers table exists
- Verify foreign key constraint exists
- Check database connection

### **Issue 3: Shift appears but shows "Unassigned"**
**Cause:** StaffMember relationship not loading

**Solution:**
- Check if StaffMember exists in database
- Verify foreign key is correct
- Check `GetShiftsForWeekAsync` is including StaffMember

### **Issue 4: Dropdown shows but bartender list is empty**
**Cause:** No active members or filter too strict

**Solution:**
- Check console when page loads
- Verify members exist with status != INACTIVE
- Check `LoadActiveMembers` method

---

## ✅ **Expected Behavior**

1. **Assign shift** → Toast: "Shift assigned to John Smith on Dec 24"
2. **Console logs** → "Loaded 1 shifts for week..."
3. **Calendar updates** → Shows "John Smith" in the day/night cell
4. **Refresh page** → Shift still appears (persisted to database)

---

## 📝 **Test Checklist**

- [ ] Open browser console (F12)
- [ ] Assign a shift
- [ ] See success toast message
- [ ] See console log "Loaded X shifts"
- [ ] See bartender name on calendar
- [ ] Refresh page
- [ ] Shift still appears
- [ ] Run SQL query to verify database

---

**Status**: ✅ Debugging tools added - Check console for details!
