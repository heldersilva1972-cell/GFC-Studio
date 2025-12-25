# GFC Studio - Database Schema

**Version:** 2.0.0  
**Date:** December 24, 2024  
**Document:** Complete Database Design

---

## 🗄️ Overview

This document defines the complete database schema for GFC Studio, including all tables, relationships, indexes, and data flows.

**Database:** SQL Server (existing GFC database)  
**ORM:** Entity Framework Core 8  
**Schema:** `dbo` (default)

---

## 📊 Entity Relationship Diagram

```
┌─────────────┐
│   Users     │
└──────┬──────┘
       │
       │ (CreatedBy, UpdatedBy)
       │
       ├──────────────────────────────────┐
       │                                  │
       ▼                                  ▼
┌─────────────┐                    ┌─────────────┐
│   Pages     │◄───────────────────┤  Templates  │
└──────┬──────┘                    └─────────────┘
       │
       │ (1:N)
       │
       ▼
┌─────────────┐
│  Sections   │
└──────┬──────┘
       │
       │ (1:N)
       │
       ├──────────────┬──────────────┐
       │              │              │
       ▼              ▼              ▼
┌─────────────┐ ┌─────────────┐ ┌─────────────┐
│   Styles    │ │ Animations  │ │   Media     │
└─────────────┘ └─────────────┘ └──────┬──────┘
                                       │
                                       │
                                       ▼
                                ┌─────────────┐
                                │MediaFolders │
                                └─────────────┘

┌─────────────┐
│   Drafts    │ (Version History)
└──────┬──────┘
       │
       │ (N:1)
       │
       ▼
┌─────────────┐
│   Pages     │
└─────────────┘

┌─────────────┐
│   Forms     │
└──────┬──────┘
       │
       │ (1:N)
       │
       ▼
┌─────────────┐
│ FormFields  │
└─────────────┘

┌─────────────┐
│  Settings   │ (Global Theme)
└─────────────┘
```

---

## 📋 Table Definitions

### 1. Pages

**Purpose:** Store all website pages

```sql
CREATE TABLE Pages (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(200) NOT NULL,
    Slug NVARCHAR(200) NOT NULL UNIQUE,
    MetaTitle NVARCHAR(200) NULL,
    MetaDescription NVARCHAR(500) NULL,
    OgImage NVARCHAR(500) NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Draft',
    -- Draft, Published, Archived
    PublishedAt DATETIME2 NULL,
    PublishedBy NVARCHAR(100) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    DeletedBy NVARCHAR(100) NULL,
    
    -- Indexes
    INDEX IX_Pages_Slug (Slug),
    INDEX IX_Pages_Status (Status),
    INDEX IX_Pages_CreatedAt (CreatedAt),
    INDEX IX_Pages_IsDeleted (IsDeleted)
);
```

**Sample Data:**
```sql
INSERT INTO Pages (Title, Slug, MetaTitle, MetaDescription, Status, CreatedBy, UpdatedBy)
VALUES 
('Home', '/', 'Gloucester Fraternity Club - Since 1923', 'Building community, friendship, and tradition since 1923', 'Published', 'admin', 'admin'),
('Hall Rentals', '/hall-rentals', 'Hall Rentals - GFC', 'Rent our beautiful halls for your special events', 'Published', 'admin', 'admin'),
('Events', '/events', 'Upcoming Events - GFC', 'Join us for exciting community events', 'Draft', 'admin', 'admin'),
('Contact', '/contact', 'Contact Us - GFC', 'Get in touch with the Gloucester Fraternity Club', 'Published', 'admin', 'admin');
```

---

### 2. Sections

**Purpose:** Store page sections (components)

```sql
CREATE TABLE Sections (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PageId INT NOT NULL,
    ComponentType NVARCHAR(100) NOT NULL,
    -- Hero, TextBlock, Image, Button, Card, etc.
    OrderIndex INT NOT NULL,
    -- Display order on page
    ContentJson NVARCHAR(MAX) NOT NULL,
    -- JSON blob with component properties
    StylesJson NVARCHAR(MAX) NULL,
    -- Custom styles override
    AnimationJson NVARCHAR(MAX) NULL,
    -- Animation settings
    ResponsiveJson NVARCHAR(MAX) NULL,
    -- Device-specific settings
    IsVisible BIT NOT NULL DEFAULT 1,
    VisibleOnDesktop BIT NOT NULL DEFAULT 1,
    VisibleOnTablet BIT NOT NULL DEFAULT 1,
    VisibleOnMobile BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100) NOT NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_Sections_Pages FOREIGN KEY (PageId) 
        REFERENCES Pages(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_Sections_PageId (PageId),
    INDEX IX_Sections_OrderIndex (OrderIndex),
    INDEX IX_Sections_ComponentType (ComponentType)
);
```

**Sample Data:**
```sql
INSERT INTO Sections (PageId, ComponentType, OrderIndex, ContentJson, CreatedBy, UpdatedBy)
VALUES 
(1, 'Hero', 0, '{"title":"Welcome to GFC","subtitle":"Since 1923","backgroundImage":"/images/hero.jpg","buttonText":"Learn More","buttonLink":"/about"}', 'admin', 'admin'),
(1, 'FeatureGrid', 1, '{"title":"What We Offer","features":[{"icon":"🏛️","title":"Hall Rentals","description":"Perfect venue","link":"/hall-rentals"}]}', 'admin', 'admin'),
(1, 'ContactSection', 2, '{"title":"Visit Us","address":"27 Webster Street","phone":"(978) 283-2889"}', 'admin', 'admin');
```

---

### 3. Drafts

**Purpose:** Version history for pages

```sql
CREATE TABLE Drafts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PageId INT NOT NULL,
    Version INT NOT NULL,
    ContentJson NVARCHAR(MAX) NOT NULL,
    -- Serialized sections array
    ChangeDescription NVARCHAR(500) NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_Drafts_Pages FOREIGN KEY (PageId) 
        REFERENCES Pages(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_Drafts_PageId_Version (PageId, Version DESC),
    INDEX IX_Drafts_CreatedAt (CreatedAt DESC)
);
```

**Sample Data:**
```sql
INSERT INTO Drafts (PageId, Version, ContentJson, ChangeDescription, CreatedBy)
VALUES 
(1, 1, '[{"type":"Hero","content":{...}}]', 'Initial version', 'admin'),
(1, 2, '[{"type":"Hero","content":{...}}]', 'Updated hero text', 'admin'),
(1, 3, '[{"type":"Hero","content":{...}}]', 'Added new section', 'admin');
```

---

### 4. Templates

**Purpose:** Reusable component templates

```sql
CREATE TABLE Templates (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    -- Layout, Content, Card, Interactive, etc.
    Description NVARCHAR(500) NULL,
    ThumbnailUrl NVARCHAR(500) NULL,
    ComponentType NVARCHAR(100) NOT NULL,
    ContentJson NVARCHAR(MAX) NOT NULL,
    -- Template configuration
    IsPublic BIT NOT NULL DEFAULT 0,
    -- Share with other users
    UsageCount INT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100) NOT NULL,
    
    -- Indexes
    INDEX IX_Templates_Category (Category),
    INDEX IX_Templates_CreatedBy (CreatedBy),
    INDEX IX_Templates_UsageCount (UsageCount DESC)
);
```

**Sample Data:**
```sql
INSERT INTO Templates (Name, Category, Description, ComponentType, ContentJson, IsPublic, CreatedBy, UpdatedBy)
VALUES 
('Modern Hero', 'Layout', 'Full-width hero with gradient', 'Hero', '{"title":"","subtitle":"","backgroundType":"gradient"}', 1, 'admin', 'admin'),
('Pricing Table', 'Content', 'GFC hall rental pricing', 'PricingTable', '{"columns":["Room","Member","Non-Member"]}', 1, 'admin', 'admin');
```

---

### 5. Media

**Purpose:** Asset library (images, videos, files)

```sql
CREATE TABLE Media (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FileName NVARCHAR(255) NOT NULL,
    OriginalFileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    FileSize BIGINT NOT NULL,
    -- Bytes
    MimeType NVARCHAR(100) NOT NULL,
    -- image/jpeg, video/mp4, etc.
    MediaType NVARCHAR(50) NOT NULL,
    -- Image, Video, Document
    Width INT NULL,
    -- For images/videos
    Height INT NULL,
    -- For images/videos
    Duration INT NULL,
    -- For videos (seconds)
    AltText NVARCHAR(500) NULL,
    -- Accessibility
    FolderId INT NULL,
    -- Organization
    UsageCount INT NOT NULL DEFAULT 0,
    -- How many times used
    UploadedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UploadedBy NVARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    DeletedAt DATETIME2 NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_Media_MediaFolders FOREIGN KEY (FolderId) 
        REFERENCES MediaFolders(Id) ON DELETE SET NULL,
    
    -- Indexes
    INDEX IX_Media_MediaType (MediaType),
    INDEX IX_Media_FolderId (FolderId),
    INDEX IX_Media_UploadedAt (UploadedAt DESC),
    INDEX IX_Media_IsDeleted (IsDeleted)
);
```

**Sample Data:**
```sql
INSERT INTO Media (FileName, OriginalFileName, FilePath, FileSize, MimeType, MediaType, Width, Height, AltText, UploadedBy)
VALUES 
('hero-20241224.jpg', 'gfc-building.jpg', '/uploads/2024/12/hero-20241224.jpg', 2048000, 'image/jpeg', 'Image', 1920, 1080, 'GFC Building Exterior', 'admin'),
('hall-interior.jpg', 'main-hall.jpg', '/uploads/2024/12/hall-interior.jpg', 1536000, 'image/jpeg', 'Image', 1600, 900, 'Main Hall Interior', 'admin');
```

---

### 6. MediaFolders

**Purpose:** Organize media library

```sql
CREATE TABLE MediaFolders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    ParentId INT NULL,
    -- For nested folders
    Path NVARCHAR(500) NOT NULL,
    -- Full path for quick lookup
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    
    -- Foreign Keys
    CONSTRAINT FK_MediaFolders_Parent FOREIGN KEY (ParentId) 
        REFERENCES MediaFolders(Id) ON DELETE NO ACTION,
    
    -- Indexes
    INDEX IX_MediaFolders_ParentId (ParentId),
    INDEX IX_MediaFolders_Path (Path)
);
```

**Sample Data:**
```sql
INSERT INTO MediaFolders (Name, ParentId, Path, CreatedBy)
VALUES 
('Hall Photos', NULL, '/Hall Photos', 'admin'),
('Events', NULL, '/Events', 'admin'),
('Logos', NULL, '/Logos', 'admin'),
('Christmas 2024', 2, '/Events/Christmas 2024', 'admin');
```

---

### 7. Forms

**Purpose:** Custom forms (contact, rental, etc.)

```sql
CREATE TABLE Forms (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(200) NOT NULL,
    FormType NVARCHAR(100) NOT NULL,
    -- Contact, HallRental, Newsletter, Custom
    SubmitToEmail NVARCHAR(500) NULL,
    -- Comma-separated emails
    SuccessMessage NVARCHAR(500) NULL,
    RedirectUrl NVARCHAR(500) NULL,
    EnableRecaptcha BIT NOT NULL DEFAULT 1,
    SaveToDatabase BIT NOT NULL DEFAULT 1,
    SettingsJson NVARCHAR(MAX) NULL,
    -- Additional settings
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy NVARCHAR(100) NOT NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100) NOT NULL,
    
    -- Indexes
    INDEX IX_Forms_FormType (FormType)
);
```

**Sample Data:**
```sql
INSERT INTO Forms (Name, FormType, SubmitToEmail, SuccessMessage, EnableRecaptcha, CreatedBy, UpdatedBy)
VALUES 
('Contact Form', 'Contact', 'admin@gfc.com', 'Thank you for contacting us!', 1, 'admin', 'admin'),
('Hall Rental Application', 'HallRental', 'rentals@gfc.com', 'Your application has been received.', 1, 'admin', 'admin');
```

---

### 8. FormFields

**Purpose:** Define form field structure

```sql
CREATE TABLE FormFields (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormId INT NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    FieldLabel NVARCHAR(200) NOT NULL,
    FieldType NVARCHAR(50) NOT NULL,
    -- Text, Email, Tel, Textarea, Select, Checkbox, etc.
    Placeholder NVARCHAR(200) NULL,
    DefaultValue NVARCHAR(500) NULL,
    IsRequired BIT NOT NULL DEFAULT 0,
    OrderIndex INT NOT NULL,
    ValidationRules NVARCHAR(MAX) NULL,
    -- JSON validation rules
    OptionsJson NVARCHAR(MAX) NULL,
    -- For select/radio/checkbox
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Foreign Keys
    CONSTRAINT FK_FormFields_Forms FOREIGN KEY (FormId) 
        REFERENCES Forms(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_FormFields_FormId_OrderIndex (FormId, OrderIndex)
);
```

**Sample Data:**
```sql
INSERT INTO FormFields (FormId, FieldName, FieldLabel, FieldType, IsRequired, OrderIndex)
VALUES 
(1, 'name', 'Your Name', 'Text', 1, 0),
(1, 'email', 'Email Address', 'Email', 1, 1),
(1, 'message', 'Message', 'Textarea', 1, 2),
(2, 'eventDate', 'Event Date', 'Date', 1, 0),
(2, 'eventType', 'Event Type', 'Select', 1, 1);
```

---

### 9. FormSubmissions

**Purpose:** Store form submissions

```sql
CREATE TABLE FormSubmissions (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FormId INT NOT NULL,
    DataJson NVARCHAR(MAX) NOT NULL,
    -- Submitted form data
    IpAddress NVARCHAR(50) NULL,
    UserAgent NVARCHAR(500) NULL,
    IsRead BIT NOT NULL DEFAULT 0,
    SubmittedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Foreign Keys
    CONSTRAINT FK_FormSubmissions_Forms FOREIGN KEY (FormId) 
        REFERENCES Forms(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_FormSubmissions_FormId_SubmittedAt (FormId, SubmittedAt DESC),
    INDEX IX_FormSubmissions_IsRead (IsRead)
);
```

---

### 10. Settings

**Purpose:** Global theme and site settings

```sql
CREATE TABLE Settings (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SettingKey NVARCHAR(100) NOT NULL UNIQUE,
    SettingValue NVARCHAR(MAX) NOT NULL,
    SettingType NVARCHAR(50) NOT NULL,
    -- Theme, SEO, Performance, etc.
    Description NVARCHAR(500) NULL,
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedBy NVARCHAR(100) NOT NULL,
    
    -- Indexes
    INDEX IX_Settings_SettingKey (SettingKey),
    INDEX IX_Settings_SettingType (SettingType)
);
```

**Sample Data:**
```sql
INSERT INTO Settings (SettingKey, SettingValue, SettingType, Description, UpdatedBy)
VALUES 
('theme.primaryColor', '#1e3a8a', 'Theme', 'Primary brand color', 'admin'),
('theme.secondaryColor', '#f59e0b', 'Theme', 'Secondary brand color', 'admin'),
('theme.fontHeading', 'Outfit', 'Theme', 'Heading font family', 'admin'),
('theme.fontBody', 'Inter', 'Theme', 'Body font family', 'admin'),
('seo.siteName', 'Gloucester Fraternity Club', 'SEO', 'Site name for SEO', 'admin'),
('seo.defaultOgImage', '/images/og-default.jpg', 'SEO', 'Default Open Graph image', 'admin');
```

---

### 11. Animations

**Purpose:** Store animation configurations

```sql
CREATE TABLE Animations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SectionId INT NOT NULL,
    AnimationType NVARCHAR(100) NOT NULL,
    -- FadeIn, SlideUp, Parallax, etc.
    Trigger NVARCHAR(50) NOT NULL,
    -- OnLoad, OnScroll, OnHover, OnClick
    Duration DECIMAL(5,2) NOT NULL DEFAULT 1.0,
    -- Seconds
    Delay DECIMAL(5,2) NOT NULL DEFAULT 0.0,
    -- Seconds
    Easing NVARCHAR(50) NOT NULL DEFAULT 'ease',
    -- ease, linear, ease-in, ease-out, etc.
    ConfigJson NVARCHAR(MAX) NULL,
    -- Additional animation config
    OrderIndex INT NOT NULL DEFAULT 0,
    -- For multiple animations
    IsEnabled BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Foreign Keys
    CONSTRAINT FK_Animations_Sections FOREIGN KEY (SectionId) 
        REFERENCES Sections(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_Animations_SectionId (SectionId)
);
```

**Sample Data:**
```sql
INSERT INTO Animations (SectionId, AnimationType, Trigger, Duration, Delay, OrderIndex)
VALUES 
(1, 'FadeIn', 'OnLoad', 1.0, 0.0, 0),
(1, 'SlideUp', 'OnLoad', 1.0, 0.5, 1),
(2, 'FadeIn', 'OnScroll', 0.8, 0.0, 0);
```

---

### 12. PageAnalytics

**Purpose:** Track page performance (future)

```sql
CREATE TABLE PageAnalytics (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PageId INT NOT NULL,
    ViewCount INT NOT NULL DEFAULT 0,
    UniqueVisitors INT NOT NULL DEFAULT 0,
    AvgTimeOnPage INT NULL,
    -- Seconds
    BounceRate DECIMAL(5,2) NULL,
    -- Percentage
    RecordedDate DATE NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    
    -- Foreign Keys
    CONSTRAINT FK_PageAnalytics_Pages FOREIGN KEY (PageId) 
        REFERENCES Pages(Id) ON DELETE CASCADE,
    
    -- Indexes
    INDEX IX_PageAnalytics_PageId_RecordedDate (PageId, RecordedDate DESC)
);
```

---

## 🔗 Relationships Summary

### One-to-Many Relationships

```
Pages (1) ──────► Sections (N)
Pages (1) ──────► Drafts (N)
Sections (1) ───► Animations (N)
Forms (1) ───────► FormFields (N)
Forms (1) ───────► FormSubmissions (N)
MediaFolders (1) ► Media (N)
MediaFolders (1) ► MediaFolders (N) [Self-referencing]
```

### Optional Relationships

```
Media (N) ──────► MediaFolders (0..1)
Sections (N) ───► Media (0..1) [via ContentJson reference]
```

---

## 📊 Indexes Strategy

### Primary Indexes (Clustered)
- All tables use `Id INT IDENTITY` as clustered primary key
- Optimal for INSERT performance
- Good for range queries

### Secondary Indexes (Non-Clustered)

**High-Priority (Query Performance):**
```sql
-- Pages
IX_Pages_Slug (Slug) -- Unique, for URL lookups
IX_Pages_Status (Status) -- Filter published pages
IX_Pages_IsDeleted (IsDeleted) -- Exclude deleted

-- Sections
IX_Sections_PageId (PageId) -- Join to Pages
IX_Sections_OrderIndex (OrderIndex) -- Sort sections

-- Drafts
IX_Drafts_PageId_Version (PageId, Version DESC) -- Version history

-- Media
IX_Media_MediaType (MediaType) -- Filter by type
IX_Media_FolderId (FolderId) -- Folder organization
```

**Medium-Priority (Reporting):**
```sql
-- Templates
IX_Templates_Category (Category)
IX_Templates_UsageCount (UsageCount DESC)

-- FormSubmissions
IX_FormSubmissions_FormId_SubmittedAt (FormId, SubmittedAt DESC)
IX_FormSubmissions_IsRead (IsRead)
```

---

## 🔄 Data Flow Diagrams

### Page Creation Flow

```
User Creates Page in Studio
        ↓
INSERT INTO Pages
(Title, Slug, Status='Draft')
        ↓
Auto-create first Draft
INSERT INTO Drafts
(PageId, Version=1, ContentJson='[]')
        ↓
Return PageId to Studio
        ↓
User adds Sections
INSERT INTO Sections
(PageId, ComponentType, ContentJson)
        ↓
Auto-save Draft
UPDATE Drafts
(ContentJson with all sections)
```

### Page Publishing Flow

```
User clicks Publish
        ↓
Validate Page
(Check required fields, SEO)
        ↓
UPDATE Pages
SET Status='Published',
    PublishedAt=GETUTCDATE(),
    PublishedBy=CurrentUser
        ↓
Create Published Snapshot
INSERT INTO Drafts
(PageId, Version++, ContentJson)
        ↓
Trigger Website Rebuild
(Next.js regenerates page)
        ↓
Return Success
```

### Media Upload Flow

```
User uploads file
        ↓
Validate file
(Type, size, dimensions)
        ↓
Optimize image
(Compress, WebP conversion)
        ↓
Save to disk
(/uploads/YYYY/MM/filename)
        ↓
INSERT INTO Media
(FileName, FilePath, FileSize, etc.)
        ↓
Return Media record
        ↓
User selects in component
UPDATE Sections
SET ContentJson (with MediaId reference)
```

---

## 🔒 Data Integrity

### Constraints

**Foreign Keys with Cascade:**
```sql
-- Delete page → Delete all sections
FK_Sections_Pages ON DELETE CASCADE

-- Delete page → Delete all drafts
FK_Drafts_Pages ON DELETE CASCADE

-- Delete section → Delete all animations
FK_Animations_Sections ON DELETE CASCADE

-- Delete form → Delete all fields
FK_FormFields_Forms ON DELETE CASCADE
```

**Foreign Keys with Set NULL:**
```sql
-- Delete folder → Keep media, set FolderId=NULL
FK_Media_MediaFolders ON DELETE SET NULL
```

**Unique Constraints:**
```sql
-- Page slugs must be unique
UNIQUE (Slug) ON Pages

-- Setting keys must be unique
UNIQUE (SettingKey) ON Settings
```

---

## 💾 Sample Queries

### Get Page with All Sections

```sql
SELECT 
    p.Id AS PageId,
    p.Title,
    p.Slug,
    p.Status,
    s.Id AS SectionId,
    s.ComponentType,
    s.OrderIndex,
    s.ContentJson,
    s.AnimationJson
FROM Pages p
LEFT JOIN Sections s ON p.Id = s.PageId
WHERE p.Slug = '/hall-rentals'
  AND p.IsDeleted = 0
ORDER BY s.OrderIndex;
```

### Get Latest Draft for Page

```sql
SELECT TOP 1
    d.Id,
    d.Version,
    d.ContentJson,
    d.CreatedAt,
    d.CreatedBy
FROM Drafts d
WHERE d.PageId = @PageId
ORDER BY d.Version DESC;
```

### Get Media by Folder

```sql
SELECT 
    m.Id,
    m.FileName,
    m.FilePath,
    m.FileSize,
    m.MediaType,
    m.Width,
    m.Height,
    m.AltText,
    m.UsageCount
FROM Media m
WHERE m.FolderId = @FolderId
  AND m.IsDeleted = 0
ORDER BY m.UploadedAt DESC;
```

### Get Form with Fields

```sql
SELECT 
    f.Id AS FormId,
    f.Name AS FormName,
    f.FormType,
    ff.Id AS FieldId,
    ff.FieldName,
    ff.FieldLabel,
    ff.FieldType,
    ff.IsRequired,
    ff.OrderIndex
FROM Forms f
LEFT JOIN FormFields ff ON f.Id = ff.FormId
WHERE f.Id = @FormId
ORDER BY ff.OrderIndex;
```

### Get Published Pages

```sql
SELECT 
    Id,
    Title,
    Slug,
    MetaTitle,
    MetaDescription,
    PublishedAt
FROM Pages
WHERE Status = 'Published'
  AND IsDeleted = 0
ORDER BY UpdatedAt DESC;
```

---

## 🚀 Performance Optimization

### Query Optimization

**Use Covering Indexes:**
```sql
-- For page list queries
CREATE NONCLUSTERED INDEX IX_Pages_List
ON Pages (Status, IsDeleted)
INCLUDE (Id, Title, Slug, UpdatedAt);
```

**Partition Large Tables (Future):**
```sql
-- Partition FormSubmissions by year
CREATE PARTITION FUNCTION PF_Year (DATETIME2)
AS RANGE RIGHT FOR VALUES 
('2024-01-01', '2025-01-01', '2026-01-01');
```

### Caching Strategy

**Cache at Application Level:**
- Published pages (cache for 1 hour)
- Global settings (cache for 24 hours)
- Media library (cache for 1 hour)
- Templates (cache for 1 hour)

**Invalidate Cache On:**
- Page publish
- Settings update
- Media upload/delete
- Template create/update

---

## 📦 Backup Strategy

### Daily Backups
```sql
-- Full backup daily at 2 AM
BACKUP DATABASE GFC
TO DISK = 'C:\Backups\GFC_Full_YYYYMMDD.bak'
WITH COMPRESSION, INIT;
```

### Transaction Log Backups
```sql
-- Every hour
BACKUP LOG GFC
TO DISK = 'C:\Backups\GFC_Log_YYYYMMDD_HH.trn'
WITH COMPRESSION;
```

### Retention Policy
- Full backups: 30 days
- Log backups: 7 days
- Draft history: 90 days
- Form submissions: Indefinite

---

## 🔄 Migration Strategy

### Initial Migration

```csharp
// EF Core Migration
public partial class InitialStudioSchema : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Create Pages table
        migrationBuilder.CreateTable(
            name: "Pages",
            columns: table => new
            {
                Id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Title = table.Column<string>(maxLength: 200, nullable: false),
                Slug = table.Column<string>(maxLength: 200, nullable: false),
                // ... other columns
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Pages", x => x.Id);
            });
        
        // Create indexes
        migrationBuilder.CreateIndex(
            name: "IX_Pages_Slug",
            table: "Pages",
            column: "Slug",
            unique: true);
        
        // ... create other tables
    }
}
```

### Seed Data

```csharp
public class StudioDbSeeder
{
    public static void Seed(GfcDbContext context)
    {
        // Seed default settings
        if (!context.Settings.Any())
        {
            context.Settings.AddRange(
                new Setting { SettingKey = "theme.primaryColor", SettingValue = "#1e3a8a", SettingType = "Theme" },
                new Setting { SettingKey = "theme.secondaryColor", SettingValue = "#f59e0b", SettingType = "Theme" }
            );
        }
        
        // Seed default templates
        if (!context.Templates.Any())
        {
            context.Templates.AddRange(
                new Template { Name = "Blank Page", Category = "Layout", ComponentType = "Container", ContentJson = "{}" }
            );
        }
        
        context.SaveChanges();
    }
}
```

---

## ✅ Schema Validation Checklist

- [x] All tables have primary keys
- [x] All foreign keys defined
- [x] Appropriate indexes created
- [x] Cascade delete rules set
- [x] Default values specified
- [x] Audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy)
- [x] Soft delete support (IsDeleted)
- [x] Sample data provided
- [x] Query examples documented
- [x] Performance considerations addressed
- [x] Backup strategy defined

---

**This database schema provides a solid foundation for GFC Studio with scalability, performance, and data integrity built in.**
