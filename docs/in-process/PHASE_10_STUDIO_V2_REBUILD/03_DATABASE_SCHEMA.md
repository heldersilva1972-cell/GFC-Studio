# GFC Studio - Database Schema

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Persistent Data Structure

---

## 📊 Tables

### Pages
Stores top-level page metadata.
- `Id` (INT)
- `Title` (STRING)
- `Slug` (STRING)
- `Status` (Draft, Published, Archived)
- `ThemeConfig` (JSON)

### Sections
Stores the actual component blocks on a page.
- `Id` (INT)
- `PageId` (FK)
- `Type` (Hero, Text, Card, etc.)
- `OrderIndex` (INT)
- `Data` (JSON - Props like colors, text, image URLs)

### Drafts
Stores auto-saved versions for the "Undo" system.
- `Id` (GUID)
- `PageId` (FK)
- `Payload` (JSON - The full state of the canvas)
- `CreatedAt` (DATETIME)

### PublicReviews (GFC Specific)
- `DisplayName` (STRING)
- `Content` (TEXT)
- `IsApproved` (BIT)
- `IsFeatured` (BIT)

---

## 🔄 Relationships
- 1 **Page** → Multiple **Sections**.
- 1 **Page** → Multiple **Drafts**.
