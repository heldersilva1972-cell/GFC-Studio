# Phase 6: Camera Experience Revamp

**Objective:** Consolidate multiple fragmented camera pages into two powerful, premium interfaces: **Camera Configuration** (Setup) and **View Cameras** (Monitoring). This modernization focuses on "Single Pane of Glass" usability, dynamic layouts, and persistent state.

## 1. Cleanup Strategy (The Purge)

We will remove the following obsolete or redundant pages/components to clean the workspace:
*   [DELETE] `LiveCameras.razor`
*   [DELETE] `CameraGrid.razor`
*   [DELETE] `Recordings.razor`
*   [DELETE] `CameraPermissions.razor` (Not currently needed)
*   [DELETE] `CameraAnalytics.razor` (Storage stats moving to Config page)

## 2. New Page: "Camera Configuration" (Setup)

**Purpose:** The single source of truth for managing the network inventory.
**Merges:** Current `CameraDiscovery` + `CameraManagement` logic.

### Key Features:
1.  **Unified Inventory List:**
    *   Displays **Saved Cameras** at the top (Managed).
    *   Displays **Discovered Cameras** below (Unmanaged).
    *   **Persistence:** Discovery results remain visible even after navigating away and back.
2.  **Smart Tiles:**
    *   **Status Dot:** Real-time Ping/Heartbeat indicator (Green/Red).
    *   **Details:** IP Address, Name, Manufacturer.
    *   **Actions (Saved):** Edit Settings, Remove (Delete from DB).
    *   **Actions (New):** "Add to System" button.
3.  **Video Agent Sync:**
    *   Automatically syncs configuration to `VideoAgent` on Add/Remove (Already implemented).

## 3. New Page: "View Cameras" (Monitoring)

**Purpose:** A high-performance, dynamic video matrix for live monitoring and forensic review.

### Layout Architecture:
*   **Sidebar / Top Bar:** "Camera Pills"
    *   List of all cameras with distinct "Connected/Offline" status dots.
    *   Clicking a pill toggles the camera's visibility in the matrix.
*   **Main Stage:** Dynamic Matrix Engine.
    *   **1 Camera:** Full Screen (100%).
    *   **2 Cameras:** Split Screen (50/50).
    *   **3+ Cameras:** Auto-calculated Grid (2x2, 3x3, etc.).
    *   **Responsiveness:** Maximizes viewing area to fill the browser window.

### Core Features:
1.  **Pagination / Group Cycling:**
    *   **Grid Size Selector:** Option to show "2", "4", "9", or "Custom" cameras at once.
    *   **Cycle Controls:** "Next Group" / "Previous Group" buttons to rotate through the camera list (e.g., viewing Cams 1-4 -> Click Next -> View Cams 5-8).
2.  **Tabs:** "Live" and "Recorded".
    *   **State Persistence:** Selected cameras remain selected when switching between tabs.

### "Live" Tab Capabilities:
*   **Digital Zoom:** Mouse-wheel support to zoom in/out of the video canvas (Client-side).
*   **Audio Control:** Individual Mute/Unmute toggle per camera (Default: Mute).
*   **Snapshot:** "Camera Icon" button to instantly download a JPG frame.
*   **Instant Replay:** "Rewind 30s" button to popup a quick review window.

### "Recorded" Tab Capabilities:
*   **Single-Focus:** Selects the *primary* active camera for detailed timeline review.
*   **Date Picker:** Calendar selector to choose the archive day.
*   **Timeline:** Visual scrubber bar.
*   **Transport Controls:** Play, Pause, Fast Forward (2x/4x), Rewind.

## 4. Implementation Steps

- [x] 1.  **Execute Cleanup:** Delete the 5 obsolete razor files and update `NavMenu`.
- [x] 2.  **Build Configuration Page:** Refactor `CameraDiscovery.razor` into the new master `CameraConfiguration.razor`.
- [x] 3.  **Build View Page:** Create the new `ViewCameras.razor` with the `DynamicMatrix` component.
- [ ] 4.  **Polish:** Implement Digital Zoom and standard styling (Dark Mode default for View page).

## Status: In Progress
- Cleanup: **Complete**
- Configuration Page: **Complete**
- View Page: **Complete**
- Polish: **Pending**
