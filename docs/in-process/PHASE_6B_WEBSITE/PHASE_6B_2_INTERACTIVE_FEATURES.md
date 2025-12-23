# PHASE 6B-2: INTERACTIVE FEATURES (Widgets)

**Objective:** Build the specific functional components that users interact with on the public site.

## 1. Events Calendar Widget
- [ ] **Front-End**: A React component that displays a responsive grid of "Event Cards" (Date, Title, Image).
- [ ] **Data Source**: Pulls from `StudioSections` of type `EventsWidget`.

## 2. Rental Availability Calendar
- [ ] **Front-End**: A Calendar view (e.g., using `react-calendar`) showing "Booked" vs "Available" dates.
- [ ] **Logic**: Connects to the `HallRentals` API to check status (does NOT show applicant names, just "Booked").

## 3. Contact Form
- [ ] **Form UI**: Name, Email, Subject, Message.
- [ ] **Backend**: secure endpoint (`POST /api/public/contact`) to receive submissions.
- [ ] **Emailer**: Service to forward these submissions to the Club Secretary.
