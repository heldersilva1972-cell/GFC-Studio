---
name: Complete Studio V2 Core Functionality
about: Implement missing 75% of Phase 10 Studio features to make the visual editor fully functional
title: "[PHASE 10] Complete Studio V2 Core Functionality"
labels: enhancement, phase-10, studio, high-priority
assignees: jules
---

## 🎯 Overview

The Studio V2 visual page editor is currently only a UI shell (25% complete). This task implements the missing core functionality to make it a fully working editor.

**Current State:** UI loads but preview is empty, can't edit content, can't save changes  
**Target State:** Full visual editor with drag-and-drop, live preview, property editing, and persistence

**Priority:** 🔴 CRITICAL  
**Estimated Effort:** 3-5 days  
**Phase:** 10 - Studio V2 Rebuild

---

## 📋 Problem Statement

### What's Working ✅
- Studio page loads without errors
- UI layout (3-panel design: toolbox, canvas, inspector)
- Top command bar with buttons
- Device toggle buttons (Desktop/Tablet/Mobile)
- Database schema exists (Pages, Sections, Drafts, Templates)
- Next.js preview app running on port 3000

### What's Broken ❌
1. **Preview shows "Empty Page"** - Sections load from DB but don't display in iframe
2. **Can't add sections** - Drag-and-drop from component library doesn't work
3. **Can't select sections** - Clicking sections in preview does nothing
4. **Can't edit content** - Properties panel is always empty
5. **Can't save changes** - Save button doesn't persist to database
6. **Can't manage sections** - No way to delete or reorder sections

---

## 🔧 Technical Issues

### Issue 1: Preview Communication Broken 🔴

**Symptom:** Preview iframe shows "Empty Page" even though sections exist in database

**Root Cause:**
- Blazor loads sections from database ✅
- JavaScript `sendSectionsToPreview()` is called ✅
- But Next.js preview iframe doesn't receive the message ❌
- postMessage communication is broken

**Files Affected:**
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`
- `apps/webapp/GFC.BlazorServer/wwwroot/js/studio-preview.js`
- `apps/studio/app/studio-preview/[slug]/page.tsx`

**Fix Required:**
1. Ensure `studio-preview.js` loads before iframe renders
2. Wait for iframe `PREVIEW_READY` message before sending sections
3. Add retry logic for failed messages
4. Add console logging for debugging
5. Verify Next.js preview is receiving and processing messages

**Acceptance Criteria:**
- [ ] Sections from database appear in preview iframe
- [ ] Console shows "Sent sections to preview: [array]"
- [ ] Console shows "Preview iframe is ready"
- [ ] Hero section with "Welcome to GFC" displays in preview

**Test Data:**
```sql
-- Run this to verify test data exists
SELECT * FROM [Pages] WHERE [Slug] = 'home';
SELECT * FROM [Sections] WHERE [PageId] = 1;
SELECT * FROM [Drafts] WHERE [PageId] = 1;
```

---

### Issue 2: Drag-and-Drop Not Implemented 🔴

**Symptom:** Can't drag components from left panel to canvas

**Root Cause:**
- Components have `draggable="true"` ✅
- But `HandleDrop` method is not implemented ❌

**Files Affected:**
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`
- `apps/webapp/GFC.BlazorServer/Services/StudioService.cs`

**Fix Required:**
1. Implement `HandleComponentDragStart(string componentType)` method
2. Implement `HandleDrop()` method to add section
3. Call `StudioService.AddSectionAsync()` on drop
4. Generate unique `ClientId` for new sections (use `Guid.NewGuid()`)
5. Send `ADD_SECTION` message to preview via postMessage
6. Update Blazor state and re-render

**Code Example:**
```csharp
private async Task HandleDrop()
{
    if (string.IsNullOrEmpty(_draggedComponentType)) return;
    
    var newSection = new StudioSection
    {
        ClientId = Guid.NewGuid().ToString(),
        ComponentType = _draggedComponentType,
        OrderIndex = sections.Count,
        Content = GetDefaultContent(_draggedComponentType),
        StudioPageId = PageId
    };
    
    sections.Add(newSection);
    await SendSectionsToPreview();
    MarkAsDirty();
    
    _draggedComponentType = null;
}
```

**Acceptance Criteria:**
- [ ] Can drag component from left panel
- [ ] Drop zone highlights when dragging over canvas
- [ ] Section appears in preview when dropped
- [ ] Section is added to `sections` list
- [ ] Properties panel shows new section when selected

---

### Issue 3: Click-to-Select Not Working 🔴

**Symptom:** Clicking sections in preview doesn't select them

**Root Cause:**
- Sections render in preview (after Issue 1 fixed) ✅
- But no click handlers exist ❌
- No selection state management ❌

**Files Affected:**
- `apps/studio/app/studio-preview/[slug]/page.tsx`
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`

**Fix Required:**

**In Next.js Preview (`page.tsx`):**
```typescript
function renderSection(section: Section) {
  return (
    <div
      key={section.id}
      className={`studio-section ${selectedId === section.id ? 'selected' : ''}`}
      onClick={() => handleSectionClick(section.id)}
      data-section-id={section.id}
    >
      {/* section content */}
    </div>
  );
}

function handleSectionClick(sectionId: string) {
  window.parent.postMessage({
    type: 'SELECT_SECTION',
    payload: { sectionId }
  }, 'http://localhost:5207');
}
```

**In Blazor (`Studio.razor`):**
```csharp
protected override async Task OnAfterRenderAsync(bool firstRender)
{
    if (firstRender)
    {
        await JSRuntime.InvokeVoidAsync("setupPreviewMessageListener", 
            DotNetObjectReference.Create(this));
    }
}

[JSInvokable]
public void HandleSectionSelected(string sectionId)
{
    selectedSection = sections.FirstOrDefault(s => s.ClientId == sectionId);
    StateHasChanged();
}
```

**Acceptance Criteria:**
- [ ] Clicking section in preview selects it
- [ ] Blue dashed outline appears around selected section
- [ ] Properties panel populates with section data
- [ ] Selection label shows section type (e.g., "Hero Section")

---

### Issue 4: Properties Panel Empty 🟡

**Symptom:** Properties panel always shows "Select a component to see its properties"

**Root Cause:**
- `StudioWizard` component exists ✅
- But it's not receiving `selectedSection` data ❌
- Property editors not implemented for all section types ❌

**Files Affected:**
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`
- `apps/webapp/GFC.BlazorServer/Components/Shared/StudioWizard.razor`

**Fix Required:**

1. Pass `selectedSection` to `StudioWizard`:
```razor
@if (selectedSection != null)
{
    <StudioWizard 
        Section="selectedSection" 
        OnPropertyChanged="HandlePropertyChanged" />
}
```

2. Implement property editors in `StudioWizard.razor`:
```razor
@switch (Section.ComponentType)
{
    case "Hero":
        <div class="property-group">
            <label>Headline</label>
            <input type="text" 
                   @bind="Section.Content.headline" 
                   @oninput="OnPropertyChanged" />
        </div>
        <div class="property-group">
            <label>Subtitle</label>
            <input type="text" 
                   @bind="Section.Content.subtitle" 
                   @oninput="OnPropertyChanged" />
        </div>
        break;
    
    case "RichTextBlock":
        <div class="property-group">
            <label>Content</label>
            <textarea @bind="Section.Content.content" 
                      @oninput="OnPropertyChanged" 
                      rows="10"></textarea>
        </div>
        break;
}
```

3. Send updates to preview:
```csharp
private async Task HandlePropertyChanged()
{
    await JSRuntime.InvokeVoidAsync("sendSectionUpdate", 
        _previewIframe, 
        JsonSerializer.Serialize(selectedSection));
    MarkAsDirty();
}
```

**Acceptance Criteria:**
- [ ] Properties panel shows fields for selected section
- [ ] Editing headline updates preview in real-time
- [ ] Color picker works for background colors
- [ ] Image upload works for background images
- [ ] Changes are reflected immediately in preview

---

### Issue 5: Save/Publish Not Working 🔴

**Symptom:** Clicking "Save Version" doesn't persist changes to database

**Root Cause:**
- `SaveDraft()` method exists but is incomplete ❌
- Sections aren't serialized correctly ❌
- Database insert fails silently ❌

**Files Affected:**
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`
- `apps/webapp/GFC.BlazorServer/Services/StudioService.cs`

**Fix Required:**

```csharp
private async Task SaveDraft()
{
    try
    {
        saveStatus = SaveStatus.Saving;
        saveStatusText = "Saving...";
        StateHasChanged();
        
        // Serialize sections to JSON
        var sectionsJson = JsonSerializer.Serialize(sections);
        
        // Get next version number
        var latestDraft = await StudioService.GetLatestDraftAsync(PageId);
        var nextVersion = (latestDraft?.Version ?? 0) + 1;
        
        // Create new draft
        var draft = new StudioDraft
        {
            PageId = PageId,
            StudioPageId = PageId,
            Version = nextVersion,
            ContentJson = sectionsJson,
            ContentSnapshotJson = sectionsJson,
            CreatedBy = currentUser,
            CreatedAt = DateTime.UtcNow
        };
        
        // Save to database
        await StudioService.SaveDraftAsync(draft);
        
        // Update UI
        latestVersion = nextVersion;
        saveStatus = SaveStatus.Saved;
        saveStatusText = $"Saved v{nextVersion}";
        
        // Reload history
        await LoadHistory();
        
        StateHasChanged();
    }
    catch (Exception ex)
    {
        Logger.LogError(ex, "Failed to save draft");
        saveStatus = SaveStatus.Error;
        saveStatusText = "Save failed";
    }
}
```

**Acceptance Criteria:**
- [ ] Clicking "Save Version" persists changes
- [ ] New draft appears in database
- [ ] Version number increments correctly
- [ ] History dropdown shows new version
- [ ] Can rollback to previous versions
- [ ] Auto-save works after 30 seconds of inactivity

---

### Issue 6: Section Management Missing 🟡

**Symptom:** Can't delete or reorder sections

**Root Cause:**
- No delete button on sections ❌
- No reorder buttons ❌
- No section toolbar ❌

**Files Affected:**
- `apps/webapp/GFC.BlazorServer/Components/Pages/Studio.razor`
- `apps/studio/app/studio-preview/[slug]/page.tsx`

**Fix Required:**

1. Add section toolbar in Next.js preview:
```typescript
<div className="section-toolbar">
  <button onClick={() => handleMoveUp(section.id)}>↑</button>
  <button onClick={() => handleMoveDown(section.id)}>↓</button>
  <button onClick={() => handleDuplicate(section.id)}>📋</button>
  <button onClick={() => handleDelete(section.id)}>🗑️</button>
</div>
```

2. Implement handlers:
```csharp
private async Task DeleteSection(string sectionId)
{
    var section = sections.FirstOrDefault(s => s.ClientId == sectionId);
    if (section != null)
    {
        sections.Remove(section);
        await SendSectionsToPreview();
        MarkAsDirty();
    }
}

private async Task MoveSection(string sectionId, int direction)
{
    var index = sections.FindIndex(s => s.ClientId == sectionId);
    if (index < 0) return;
    
    var newIndex = index + direction;
    if (newIndex < 0 || newIndex >= sections.Count) return;
    
    var section = sections[index];
    sections.RemoveAt(index);
    sections.Insert(newIndex, section);
    
    // Update OrderIndex
    for (int i = 0; i < sections.Count; i++)
    {
        sections[i].OrderIndex = i;
    }
    
    await SendSectionsToPreview();
    MarkAsDirty();
}
```

**Acceptance Criteria:**
- [ ] Delete button removes section
- [ ] Up/down arrows reorder sections
- [ ] Duplicate button creates copy
- [ ] Changes reflect in preview immediately
- [ ] OrderIndex is maintained correctly

---

## 📁 Files to Modify

### Blazor Server (C#)
```
apps/webapp/GFC.BlazorServer/
├── Components/Pages/Studio.razor (main editor - ~1000 lines)
├── Components/Shared/StudioWizard.razor (properties panel - ~200 lines)
├── Services/StudioService.cs (CRUD operations - ~300 lines)
├── wwwroot/js/studio-preview.js (iframe communication - ~50 lines)
```

### Next.js Preview (TypeScript)
```
apps/studio/
├── app/studio-preview/[slug]/page.tsx (preview renderer - ~200 lines)
```

---

## 🧪 Testing Checklist

### Basic Functionality
- [ ] Studio page loads without errors
- [ ] Preview shows existing page content (Issue 1)
- [ ] Device toggles change preview size
- [ ] Can drag component from left panel (Issue 2)
- [ ] Can drop component on canvas (Issue 2)
- [ ] Section appears in preview (Issue 2)
- [ ] Can click section to select it (Issue 3)
- [ ] Properties panel shows section properties (Issue 4)
- [ ] Can edit section properties (Issue 4)
- [ ] Changes appear in preview immediately (Issue 4)
- [ ] Can save changes (Issue 5)
- [ ] Can view version history (Issue 5)
- [ ] Can rollback to previous version (Issue 5)
- [ ] Can delete sections (Issue 6)
- [ ] Can reorder sections (Issue 6)

### Error Handling
- [ ] Shows error if page not found
- [ ] Shows error if save fails
- [ ] Shows error if preview fails to load
- [ ] Handles network errors gracefully

---

## 📚 Reference Documentation

**Phase 10 Specifications:**
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/01_STUDIO_UI_SPECIFICATIONS.md`
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/02_COMPONENT_LIBRARY.md`
- `docs/complete/PHASE_10_STUDIO_V2_REBUILD/03_DATABASE_SCHEMA.md`

**Existing Code to Study:**
- `StudioService.cs` - CRUD patterns
- `StudioAutoSaveService.cs` - Auto-save logic
- `PageService.cs` - Page management
- `AnimationOrchestrator.razor` - SignalR patterns

**Database Schema:**
```sql
-- Pages table
[Pages] (Id, Title, Slug, Status, ...)

-- Sections table (page content)
[Sections] (Id, PageId, StudioPageId, ComponentType, OrderIndex, ContentJson, ...)

-- Drafts table (version history)
[Drafts] (Id, PageId, StudioPageId, Version, ContentJson, ContentSnapshotJson, ...)
```

---

## 🚀 Implementation Plan

### Day 1: Preview Communication
- [ ] Fix Issue 1 (Preview not showing content)
- [ ] Verify sections load from database
- [ ] Verify postMessage communication works
- [ ] Test with existing "Home" page data

### Day 2: Interaction
- [ ] Fix Issue 3 (Click-to-select)
- [ ] Fix Issue 4 (Properties panel)
- [ ] Test editing properties
- [ ] Test real-time preview updates

### Day 3: Content Management
- [ ] Fix Issue 2 (Drag-and-drop)
- [ ] Fix Issue 6 (Section management)
- [ ] Test adding/deleting/reordering sections

### Day 4: Persistence
- [ ] Fix Issue 5 (Save/Publish)
- [ ] Test version history
- [ ] Test rollback functionality
- [ ] Test auto-save

### Day 5: Polish & Testing
- [ ] Run full testing checklist
- [ ] Fix any bugs found
- [ ] Add error handling
- [ ] Add loading states
- [ ] Code cleanup and documentation

---

## 📝 Important Notes

### Database Compatibility
- Tables have BOTH `PageId` and `StudioPageId` columns (legacy compatibility)
- **Always populate BOTH when inserting data**
- C# models use `StudioPageId` but database uses `PageId` as the actual FK
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
- Add `console.log()` statements liberally

---

## ✅ Definition of Done

- [ ] All 6 issues are fixed
- [ ] All testing checklist items pass
- [ ] Code is committed to feature branch `feature/complete-studio-core`
- [ ] Pull request created with detailed description
- [ ] No merge conflicts
- [ ] All tests pass
- [ ] Code review completed
- [ ] Documentation updated
- [ ] Merged to main branch

---

**Assignee:** @jules  
**Reviewer:** @heldersilva  
**Branch:** `feature/complete-studio-core`  
**Labels:** `enhancement`, `phase-10`, `studio`, `high-priority`  
**Milestone:** Phase 10 - Studio V2 Rebuild  
**Estimated Time:** 3-5 days
