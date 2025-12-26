# TASK FOR JULES: GFC Ecosystem Merger - Phase 1 (Foundation)

## 🎯 OBJECTIVE
Implement the Database Schema and Core Services for the GFC Ecosystem Merger, covering Website Management, Hall Rentals, and Staff Operations.

**Status**: INITIALIZING (Phase 1 of 5)
**Roadmap Pivot**: Mobile App features moved to Phase 5.
**Reference**: `docs/GFC_MASTER_OPERATIONAL_SPEC.md`

---

## 🏛️ ARCHITECTURAL CONTEXT
- **Shared Data**: All modules (Studio, Web App, Mobile App, Website) share the `GfcDbContext`.
- **Infrastructure**: Use Entity Framework Core with SQL Server.
- **Location**: Models in `apps/webapp/GFC.Core/Models/`, Services in `apps/webapp/GFC.BlazorServer/Services/`.

---

## 📋 TASKS TO COMPLETE

### **TASK 1: Studio & Content Models** 🏗️
Create the following models to support the Visual Studio logic:
- `StudioPage`: (Id, Title, Slug, IsPublished, CreatedAt, UpdatedAt)
- `StudioSection`: (Id, PageId, Type, OrderIndex, ContentJson, DesktopLayout, MobileLayout)
- `StudioDraft`: (Id, PageId, ContentSnapshotJson, CreatedBy, CreatedAt)

### **TASK 2: Operational Models (Rentals & Staff)** 📅
Create the following models for the "Mission Control" features:
- `HallRental`: (Id, ApplicantName, ContactInfo, EventDate, Status [Pending/Approved/Denied], GuestCount, KitchenUsed, TotalPrice, InternalNotes)
- `StaffShift`: (Id, StaffMemberId, Date, ShiftType [1=Day, 2=Night], Status)
- `ShiftReport`: (Id, ShiftId, BartenderId, BarSales, LottoSales, TotalDeposit, SubmittedAt, IsLate)

### **TASK 3: Database Registration** 💾
- Add `DbSet` properties for all new models to `GfcDbContext`.
- Configure any necessary relationships (e.g., `StudioPage` has many `StudioSections`).
- **ACTUAL ACTION**: Create a new EF Core Migration: `AddGfcEcosystemFoundation`.

### **TASK 4: Core Services (CRUD)** 🛠️
Implement the basic Service layer (Interface + Implementation) for:
1. `IRentalService`: `GetRequestsAsync()`, `CreateRequestAsync()`, `UpdateStatusAsync()`.
2. `IShiftService`: `GetScheduleAsync()`, `SubmitShiftReportAsync()`.
3. `IStudioService`: `GetPageConfigAsync()`, `SaveDraftAsync()`.

---

## ⚠️ CRITICAL RULES
1. **Model Hygiene**: Use `System.ComponentModel.DataAnnotations` ( [Required], [Key], [MaxLength] ).
2. **Context Usage**: Use `GfcDbContext` directly (per user's previous preference in Phase 5).
3. **Draft Mode Logic**: Ensure `StudioDraft` can store a JSON blob of the entire page for rollback.
4. **Validation**: Hall Rental logic must eventually reflect rules from the original site (Capacity <= 180).

---

## ✅ SUCCESS CRITERIA
- [ ] Database schema updated via Migration.
- [ ] Models follow the naming conventions in `GFC_STUDIO_LIVING_SPEC.md`.
- [ ] Services injected and registered in `Program.cs`.
- [ ] Project builds successfully without errors.

---

**Task Created**: 2025-12-22
**Assigned To**: JULES 🚀
