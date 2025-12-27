# 📋 ISSUE TEMPLATE FOR JULES - IMPROVED FORMAT

## How to Use This Template

When creating issues for Jules, follow this format **EXACTLY**. This ensures Jules has:
1. ✅ **Exact code to copy/paste** - No interpretation needed
2. ✅ **Clear file locations** - Knows exactly where to make changes
3. ✅ **Before/After examples** - Can verify changes are correct
4. ✅ **Testing checklist** - Knows when the work is complete

---

# Issue Title: [Feature/Bug] - [Brief Description]

**Priority:** [High/Medium/Low]  
**Estimated Time:** [X hours]  
**Files to Modify:** [List exact file paths]

---

## 📝 Summary

[1-2 sentence description of what needs to be done]

---

## 🎯 Acceptance Criteria

- [ ] [Specific, testable criterion 1]
- [ ] [Specific, testable criterion 2]
- [ ] [Specific, testable criterion 3]

---

## 📂 Files to Modify

### File 1: `[Exact file path]`
**Lines:** [Approximate line numbers if known]

### File 2: `[Exact file path]`
**Lines:** [Approximate line numbers if known]

---

## 🔧 Implementation Details

### Change 1: [Description]

**File:** `[Exact path]`  
**Location:** [Line numbers or search term]

**FIND THIS CODE:**
```[language]
[EXACT code to find - copy/pasteable]
```

**REPLACE WITH THIS CODE:**
```[language]
[EXACT replacement code - copy/pasteable]
```

**Why:** [Brief explanation of why this change is needed]

---

### Change 2: [Description]

**File:** `[Exact path]`  
**Location:** [Line numbers or search term]

**FIND THIS CODE:**
```[language]
[EXACT code to find]
```

**REPLACE WITH THIS CODE:**
```[language]
[EXACT replacement code]
```

**Why:** [Brief explanation]

---

### Change 3: [Add New Code]

**File:** `[Exact path]`  
**Location:** [Where to add - e.g., "After line 500" or "Before @code section"]

**ADD THIS CODE:**
```[language]
[EXACT code to add - copy/pasteable]
```

**Why:** [Brief explanation]

---

## 🧪 Testing Checklist

After implementing, verify:

- [ ] [Specific test 1 - e.g., "Dark mode displays correctly"]
- [ ] [Specific test 2 - e.g., "Clicking stat card opens filtered list"]
- [ ] [Specific test 3 - e.g., "Cannot add duplicate events"]
- [ ] No console errors
- [ ] No compilation errors
- [ ] Application runs successfully

---

## 📸 Expected Result

**Before:**
[Description or screenshot of current state]

**After:**
[Description or screenshot of expected state]

---

## ⚠️ Important Notes

- [Any gotchas or special considerations]
- [Dependencies or prerequisites]
- [Breaking changes to be aware of]

---

## 🔗 Related Issues

- [Link to related issues if any]

---

## 💡 Example

Here's a complete example of how to format a change:

### Change: Add Dark Mode Support

**File:** `Components/Pages/Admin/Dashboard.razor.css`  
**Location:** After line 25 (after `:root` closing brace)

**ADD THIS CODE:**
```css
/* Dark Mode Support */
@media (prefers-color-scheme: dark) {
    :root {
        --gray-50: #1f1f1f;
        --gray-100: #2d2d2d;
        --gray-900: #ffffff;
    }
    
    .dashboard {
        background: linear-gradient(135deg, #1a1a1a 0%, #2d2d2d 100%);
    }
}
```

**Why:** Provides proper contrast and visibility in dark mode

**Test:** 
1. Enable dark mode in browser/OS
2. Navigate to dashboard
3. Verify all text is readable
4. Verify proper contrast ratios

---

## ✅ Definition of Done

- [ ] All code changes implemented exactly as specified
- [ ] All tests in checklist pass
- [ ] No new errors or warnings
- [ ] Code follows project conventions
- [ ] Changes committed with clear message
- [ ] Pull request created with reference to this issue

---

## 📌 CRITICAL RULES FOR JULES

1. **COPY CODE EXACTLY** - Do not modify, interpret, or "improve" the provided code
2. **TEST EVERYTHING** - Complete the entire testing checklist before submitting
3. **ASK IF UNCLEAR** - If anything is ambiguous, ask for clarification
4. **VERIFY FILE PATHS** - Make sure you're editing the correct files
5. **CHECK LINE NUMBERS** - Use search if line numbers don't match
6. **COMMIT PROPERLY** - Use clear commit messages referencing the issue number

---

## 🎓 Why This Format Works

- **No Ambiguity:** Exact code means no interpretation errors
- **Easy to Verify:** Before/After makes it clear what changed
- **Complete:** Nothing is left to assumption
- **Testable:** Clear criteria for success
- **Traceable:** Easy to review and audit

---

# TEMPLATE END

**Instructions for Helder:**
1. Copy this template
2. Fill in ALL sections with EXACT code
3. Include COMPLETE code blocks (not snippets)
4. Provide EXACT file paths
5. Add SPECIFIC test cases
6. Review before posting to ensure Jules can copy/paste everything

**Instructions for Jules:**
1. Read the ENTIRE issue first
2. Copy code EXACTLY as provided
3. Test EVERY item in the checklist
4. Ask questions if ANYTHING is unclear
5. Do NOT submit until ALL tests pass
