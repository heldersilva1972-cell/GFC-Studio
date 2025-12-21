# Phase 2: Visual Interval Builder (Access Schedules)

## 🎯 Objective
Replace the current placeholder alert in the "Time Profiles" section with a professional, graphical interface for defining recurring access time ranges.

## 🏗️ Technical Requirements

### 1. New Component: `TimeIntervalPicker.razor`
*   **Dual Time Inputs**: Start and End time selectors using the browser's native `<input type="time">` but styled to match the GFC design system.
*   **Day Selector**: A row of 7 toggle chips (M, T, W, T, F, S, S) for selecting which days the interval applies to.
*   **Validation**: Ensure End Time is after Start Time and no gaps/overlaps within a single profile if required.

### 2. UI Integration in `CardAccessController.razor`
*   **Profile Detail View**: When a profile is clicked, expand a section to show all its intervals.
*   **Visual Timeline**: (Optional/Stretch) A horizontal 24-hour bar showing shaded regions where access is granted.
*   **Add/Remove Logic**: Allow users to add multiple intervals to a single profile (e.g., "Mon-Fri 09:00-17:00" AND "Sat 10:00-14:00").

### 3. Data persistence
*   Integrate with the `TimeProfileIntervals` table.
*   Map the UI toggles (Mon-Sun) to the bitmask or boolean columns in the database.

## 🚀 UX Goals
*   **Mobile Friendly**: Sliders and toggles must be easy to hit on a touchscreen.
*   **Real-time Feedback**: As times are changed, the summary text (e.g., "Every weekday from 9 AM to 5 PM") should update instantly.
