# PHASE 9-1: DATA MIGRATION

**Objective:** Import historical data so the new system isn't empty.

## 1. Member Data
- [ ] **Extraction**: Export data from the legacy FoxPro/Excel/Access database.
- [ ] **Transformation**: Map legacy fields to new `Person` schema.
- [ ] **Loading**: Script to insert into `GfcDbContext`.

## 2. Financial History
- [ ] **Dues History**: Import past years of payment records to ensure "Life Member" calculations are accurate.
