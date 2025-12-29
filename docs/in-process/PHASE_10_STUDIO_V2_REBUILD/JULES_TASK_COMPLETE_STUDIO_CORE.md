# Jules Task: Complete Studio V2 Core Functionality

**Priority:** 🔴 CRITICAL  
**Estimated Effort:** 3-5 days  
**Phase:** 10 - Studio V2 Rebuild  
**Status:** 🚧 Ready for Implementation  
**Created:** 2025-12-27  

---

## 🎯 Objective

Complete the missing 75% of Phase 10 Studio V2 core functionality to deliver a fully working visual page editor. The current Studio is only a UI shell - it needs the core editing features, preview system, and data persistence to be usable.

---

## 📋 Current State Analysis

### ✅ What's Working
- Studio page loads without errors
- UI layout (3-panel design: toolbox, canvas, inspector)
- Top command bar with buttons
- Device toggle buttons (Desktop/Tablet/Mobile)
- Database schema exists (Pages, Sections, Drafts, Templates)
- Next.js preview app running on port 3000

### ❌ What's Broken
1. **Preview not showing content** - Sections load from DB but don't display in iframe
2. **No component drag-and-drop** - Can't add sections from left panel
3. **No click-to-edit** - Can't select sections in preview
4. **Properties panel empty** - No way to edit section content
5. **Save doesn't work** - Changes don't persist to database
6. **No section management** - Can't add, delete, or reorder sections

---

## 🔧 Technical Issues to Fix

### Issue 1: Preview Communication Broken
**File:** `Studio.razor`, `studio-preview.js`, `studio-preview/[slug]/page.tsx`

**Problem:**
- Blazor loads sections from database (✅)
- JavaScript `sendSectionsToPreview()` is called (✅)
- But Next.js preview iframe shows "Empty Page" (❌)
- postMessage communication not working

**Root Cause:**
- Script `studio-preview.js` may not be loading
- Iframe may not be ready when message is sent
- Next.js preview may not be listening for messages

**Fix Required:**
1. Ensure `studio-preview.js` loads before iframe
2. Wait for iframe `PREVIEW_READY` message before sending sections
3. Add retry logic for failed messages
4. Add console logging for debugging
5. Verify Next.js preview is receiving and processing messages

**Acceptance Criteria:**
- [ ] Sections from database appear in preview iframe
- [ ] Console shows "Sent sections to preview: [array]"
- [ ] Console shows "Preview iframe is ready"
- [ ] Hero section with "Welcome to GFC" displays

---

### Issue 2: Component Drag-and-Drop Not Implemented
**File:** `Studio.razor`, `StudioService.cs`

**Problem:**
- Left panel shows component list (✅)
- Components have `draggable="true"` attribute (✅)
- But dropping does nothing (❌)

**Fix Required:**
1. Implement `HandleComponentDragStart` method
2. Implement `HandleDrop` method to add section
3. Call `StudioService.AddSectionAsync()` on drop
4. Generate unique `ClientId` for new sections
5. Send updated sections to preview via postMessage
6. Update Blazor state and re-render

**Acceptance Criteria:**
- [ ] Can drag component from left panel
- [ ] Drop zone highlights when dragging over canvas
- [ ] Section appears in preview when dropped
- [ ] Section is saved to `sections` list
- [ ] Properties panel shows new section

---

### Issue 3: Click-to-Select Not Working
**File:** `studio-preview/[slug]/page.tsx`, `Studio.razor`

**Problem:**
- Sections render in preview (after Issue 1 fixed)
- But clicking them does nothing
- No selection outline appears

**Fix Required:**
1. Add click handlers to sections in Next.js preview
2. Send `SELECT_SECTION` message to Blazor parent
3. Blazor receives message and sets `selectedSection`
4. Preview receives `SECTION_SELECTED` message back
5. Preview shows blue outline on selected section

**Acceptance Criteria:**
- [ ] Clicking section in preview selects it
- [ ] Blue dashed outline appears around selected section
- [ ] Properties panel populates with section data
- [ ] Selection label shows section type

---

### Issue 4: Properties Panel Empty
**File:** `Studio.razor`, `StudioWizard.razor`

**Problem:**
- Properties panel shows "Select a component to see its properties"
- Even when section is selected (after Issue 3 fixed)
- StudioWizard component exists but isn't rendering

**Fix Required:**
1. Pass `selectedSection` to `StudioWizard` component
2. Implement property editors for each section type:
   - Hero: headline, subtitle, backgroundImage
   - RichTextBlock: content (rich text editor)
   - ButtonCTA: text, link, backgroundColor
   - ImageGallery: images array
3. Bind property changes to section object
4. Call `MarkAsDirty()` on property change
5. Send `UPDATE_SECTION` message to preview

**Acceptance Criteria:**
- [ ] Properties panel shows fields for selected section
- [ ] Editing headline updates preview in real-time
- [ ] Color picker works for background colors
- [ ] Image upload works for background images
- [ ] Changes are reflected immediately in preview

---

### Issue 5: Save/Publish Not Working
**File:** `Studio.razor`, `StudioService.cs`, `StudioAutoSaveService.cs`

**Problem:**
- "Save Version" button exists
- But clicking it doesn't persist changes
- Auto-save service exists but may not be working

**Fix Required:**
1. Implement `SaveDraft()` method properly
2. Serialize `sections` list to JSON
3. Call `StudioService.SaveDraftAsync()`
4. Insert into `Drafts` table with incremented version
5. Show success message
6. Update `latestVersion` display
7. Add to history dropdown

**Acceptance Criteria:**
- [ ] Clicking "Save Version" persists changes
- [ ] New draft appears in database
- [ ] Version number increments
- [ ] History dropdown shows new version
- [ ] Can rollback to previous versions
- [ ] Auto-save works after 30 seconds of inactivity

---

### Issue 6: Section Management Missing
**File:** `Studio.razor`

**Problem:**
- Can't add sections (after drag-drop fixed)
- Can't delete sections
- Can't reorder sections
- No section toolbar

**Fix Required:**
1. Add "Add Section" button functionality
2. Add delete button to section toolbar
3. Implement up/down arrows for reordering
4. Update `OrderIndex` when reordering
5. Send updates to preview
6. Persist changes to database

**Acceptance Criteria:**
- [ ] "Add Section" button adds new section
- [ ] Delete button removes section
- [ ] Up/down arrows reorder sections
- [ ] Changes reflect in preview immediately
- [ ] OrderIndex is maintained correctly

---

## 📁 Files to Modify

### Blazor Server (C#)
```
apps/webapp/GFC.BlazorServer/
├── Components/Pages/Studio.razor (main editor)
├── Components/Shared/StudioWizard.razor (properties panel)
├── Services/StudioService.cs (CRUD operations)
├── Services/StudioAutoSaveService.cs (auto-save logic)
├── wwwroot/js/studio-preview.js (iframe communication)
```

### Next.js Preview (TypeScript)
```
apps/studio/
├── app/studio-preview/[slug]/page.tsx (preview renderer)
```

### Database
```
- Ensure Sections table has data
- Ensure Drafts table has data
- Verify foreign keys are correct
```

---

## 🧪 Testing Checklist

### Basic Functionality
- [ ] Studio page loads without errors
- [ ] Preview shows existing page content
- [ ] Device toggles change preview size
- [ ] Can drag component from left panel
- [ ] Can drop component on canvas
- [ ] Section appears in preview
- [ ] Can click section to select it
- [ ] Properties panel shows section properties
- [ ] Can edit section properties
- [ ] Changes appear in preview immediately
- [ ] Can save changes
- [ ] Can view version history
- [ ] Can rollback to previous version

### Advanced Functionality
- [ ] Auto-save works after 30 seconds
- [ ] Can delete sections
- [ ] Can reorder sections
- [ ] Can duplicate sections
- [ ] Can save section as template
- [ ] Can publish page
- [ ] Can exit to dashboard

### Error Handling
- [ ] Shows error if page not found
- [ ] Shows error if save fails
- [ ] Shows error if preview fails to load
- [ ] Handles network errors gracefully

---

## 📚 Reference Documentation

### Phase 10 Specs
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/01_STUDIO_UI_SPECIFICATIONS.md`
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/02_COMPONENT_LIBRARY.md`
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/03_DATABASE_SCHEMA.md`

### Existing Code
- Study `StudioService.cs` for CRUD patterns
- Study `StudioAutoSaveService.cs` for auto-save logic
- Study `PageService.cs` for page management
- Study `AnimationOrchestrator.razor` for SignalR patterns

### Database Schema
```sql
-- Pages table (main pages)
CREATE TABLE [Pages] (
    [Id] INT PRIMARY KEY IDENTITY,
    [Title] NVARCHAR(200),
    [Slug] NVARCHAR(200),
    [Status] NVARCHAR(20), -- Draft, Published
    ...
);

-- Sections table (page content)
CREATE TABLE [Sections] (
    [Id] INT PRIMARY KEY IDENTITY,
    [PageId] INT FOREIGN KEY REFERENCES [Pages]([Id]),
    [StudioPageId] INT, -- duplicate for compatibility
    [ComponentType] NVARCHAR(100), -- Hero, RichTextBlock, etc.
    [OrderIndex] INT,
    [ContentJson] NVARCHAR(MAX), -- section properties
    ...
);

-- Drafts table (version history)
CREATE TABLE [Drafts] (
    [Id] INT PRIMARY KEY IDENTITY,
    [PageId] INT FOREIGN KEY REFERENCES [Pages]([Id]),
    [StudioPageId] INT, -- duplicate for compatibility
    [Version] INT,
    [ContentJson] NVARCHAR(MAX), -- serialized sections array
    [ContentSnapshotJson] NVARCHAR(MAX), -- duplicate for compatibility
    ...
);
```

---

## 🎯 Success Criteria

### Minimum Viable Product (MVP)
- [ ] Can load existing page in Studio
- [ ] Can see page content in preview
- [ ] Can add new sections
- [ ] Can edit section properties
- [ ] Can save changes
- [ ] Changes persist to database

### Full Feature Set
- [ ] All 6 issues above are fixed
- [ ] All testing checklist items pass
- [ ] No console errors
- [ ] Performance is acceptable (< 1s to load page)
- [ ] Code is clean and well-documented

---

## 🚀 Implementation Order

### Day 1: Preview Communication
1. Fix Issue 1 (Preview not showing content)
2. Verify sections load from database
3. Verify postMessage communication works
4. Test with existing "Home" page data

### Day 2: Interaction
1. Fix Issue 3 (Click-to-select)
2. Fix Issue 4 (Properties panel)
3. Test editing properties
4. Test real-time preview updates

### Day 3: Content Management
1. Fix Issue 2 (Drag-and-drop)
2. Fix Issue 6 (Section management)
3. Test adding/deleting/reordering sections

### Day 4: Persistence
1. Fix Issue 5 (Save/Publish)
2. Test version history
3. Test rollback functionality
4. Test auto-save

### Day 5: Polish & Testing
1. Run full testing checklist
2. Fix any bugs found
3. Add error handling
4. Add loading states
5. Code cleanup and documentation

---

## 📝 Notes for Jules

### Important Context
- The database has BOTH `PageId` and `StudioPageId` columns (legacy compatibility)
- Always populate BOTH when inserting data
- The C# models use `StudioPageId` but database uses `PageId` as the actual FK
- EF Core mappings handle this via `HasColumnName()`

### Known Issues
- `studio-preview.js` script may not be loading (check browser network tab)
- Iframe may not be ready when Blazor tries to send messages
- Next.js preview needs to listen for `UPDATE_SECTIONS` message type

### Debugging Tips
- Use browser console to see postMessage traffic
- Check Network tab for 404s on `studio-preview.js`
- Use React DevTools to inspect Next.js preview state
- Check Blazor Server logs for C# errors

---

## ✅ Definition of Done

- [ ] All 6 issues are fixed
- [ ] All testing checklist items pass
- [ ] Code is committed to feature branch
- [ ] Pull request created with detailed description
- [ ] No merge conflicts
- [ ] All tests pass
- [ ] Code review completed
- [ ] Merged to main branch

---

**Assignee:** Jules  
**Reviewer:** Helder Silva  
**Branch:** `feature/complete-studio-core`  
**Target:** Phase 10 - Studio V2 Rebuild  
**Deadline:** 5 days from start
