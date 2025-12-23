# PHASE 6A-1: STUDIO CORE ENGINE (Drafts & Publishing)

**Objective:** Implement the backend capability to save work-in-progress content without affecting the live site.

## 1. Database Schema
- [ ] Create `StudioDrafts` table:
    - `Id` (int)
    - `PageId` (FK to StudioPages)
    - `ContentJson` (nvarchar max - stores the serialized block data)
    - `Version` (int)
    - `CreatedAt` (datetime)
    - `CreatedBy` (string - username)

## 2. API / Services
- [ ] **IStudioService.SaveDraftAsync(pageId, content)**: Logic to insert a new row into `StudioDrafts`.
- [ ] **IStudioService.GetDraftsAsync(pageId)**: Retrieve history.
- [ ] **IStudioService.PublishDraftAsync(draftId)**:
    1. Find the draft.
    2. Overwrite `StudioPage.Content` with `draft.ContentJson`.
    3. Mark `StudioPage.LastPublishedAt`.

## 3. UI Implementation
- [ ] **Save Button**: Change behavior to "Save Draft" (doesn't impact live site).
- [ ] **Publish Button**: New distinct button (e.g., "Go Live") that triggers the publish logic.
- [ ] **History Dropdown**: A simple dropdown in the Toolbar to load previous draft versions into the canvas.
