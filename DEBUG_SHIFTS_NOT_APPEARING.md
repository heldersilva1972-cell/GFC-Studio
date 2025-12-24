# Debugging: Shifts Not Appearing on Calendar

## 🔍 **How to Debug**

### **Step 1: Open Browser Console**
1. Press **F12** in your browser
2. Click the **Console** tab
3. Clear the console (click the 🚫 icon)

### **Step 2: Assign a Shift**
1. Click an "Assign" button on the calendar
2. Select a bartender from the dropdown
3. Click "Assign Shift"
4. **Watch the console output**

---

## 📊 **What You Should See in Console**

### **When Assigning:**
```
=== SaveShiftAssignment called ===
newShift is null: false
selectedStaffId: 1
Selected staff: John Smith
Saving shift:
  - Date: 2025-12-24
  - ShiftType: 1 (Day)
  - StaffMemberId: 1
  - StaffName: John Smith
  - Status: Scheduled
Shift saved successfully!
Reloading shifts...
Loaded 1 shifts for week starting Dec 22
  - Shift 1: John Smith on Dec 24 (Day)
Shifts reloaded. Total in weeklyShifts: 1
```

### **When Page Loads:**
```
Loaded X shifts for week starting Dec 22
  - Shift 1: John Smith on Dec 24 (Day)
  - Shift 2: Sarah Johnson on Dec 25 (Night)
```

---

## 🐛 **Common Issues & What Console Will Show**

### **Issue 1: Shift saves but doesn't appear**

**Console shows:**
```
Shift saved successfully!
Reloading shifts...
Loaded 0 shifts for week starting Dec 22  ← PROBLEM!
```

**Cause:** Date mismatch - shift saved for different week

**Solution:** Check the `currentDate` value and the shift's `Date`

---

### **Issue 2: Shift doesn't save**

**Console shows:**
```
SaveShiftAssignment Error: [error message]
```

**Cause:** Database error or constraint violation

**Solution:** Check the error message, verify database tables exist

---

### **Issue 3: StaffName is null/empty**

**Console shows:**
```
Loaded 1 shifts for week starting Dec 22
  - Shift 1: Unassigned on Dec 24 (Day)  ← PROBLEM!
```

**Cause:** StaffMember not loading or Name is null

**Solution:** Check if StaffMember exists in database with correct ID

---

### **Issue 4: Wrong week displayed**

**Console shows:**
```
Loaded 0 shifts for week starting Dec 15  ← Wrong week!
```

**Cause:** `currentDate` doesn't match the week you're viewing

**Solution:** Use Previous/Next buttons to navigate to correct week

---

## 🗄️ **Verify in Database**

After assigning a shift, run this SQL:

```sql
-- Check if shift was saved
SELECT TOP 10
    ss.Id,
    ss.Date,
    ss.ShiftType,
    ss.Status,
    ss.StaffMemberId,
    sm.Name as StaffName,
    sm.IsActive
FROM StaffShifts ss
LEFT JOIN StaffMembers sm ON ss.StaffMemberId = sm.Id
ORDER BY ss.Date DESC, ss.Id DESC;
```

**What to check:**
- ✅ Row exists = Shift was saved
- ✅ StaffMemberId matches a real StaffMember
- ✅ Date is correct
- ✅ ShiftType is 1 (Day) or 2 (Night)
- ✅ StaffMember.Name is not NULL

---

## ✅ **Expected Flow**

1. **Click Assign** → Modal opens
2. **Select bartender** → selectedStaffId set
3. **Click "Assign Shift"** → SaveShiftAssignment called
4. **Console logs** → Shows all shift details
5. **Shift saved** → Database INSERT
6. **Shifts reloaded** → LoadShifts called
7. **Console logs** → Shows shift in list
8. **Calendar updates** → Name appears in cell
9. **Refresh page** → Shift still there

---

## 📝 **Test Checklist**

- [ ] Open console (F12)
- [ ] Clear console
- [ ] Assign a shift
- [ ] See "SaveShiftAssignment called" in console
- [ ] See "Shift saved successfully!" in console
- [ ] See "Loaded X shifts" with your shift listed
- [ ] See name appear on calendar
- [ ] Refresh page
- [ ] Shift still appears
- [ ] Run SQL query to verify database

---

## 🔧 **Next Steps**

1. **Rebuild** the application
2. **Open console** (F12)
3. **Assign a shift**
4. **Copy all console output** and share it

The console logs will tell us exactly what's happening!

---

**Status**: ✅ Comprehensive logging added - Check console for details!
