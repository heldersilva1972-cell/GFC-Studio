# IMPORTANT: Guidelines for Future Implementation

## 🚨 CRITICAL RULES - READ BEFORE STARTING ANY TASK

### **1. File Locations**
✅ **DO**: Create files in `apps/webapp/GFC.Core/` or `apps/webapp/GFC.BlazorServer/`
❌ **DON'T**: Create files in root `GFC.Core/` or `GFC.BlazorServer/`

**Why**: The root folders are NOT part of the project. Files must be in the `apps/webapp/` directory structure.

---

### **2. Razor File Syntax**
✅ **DO**: Remove ALL HTML comments from Razor files
❌ **DON'T**: Use `<!-- comments -->` in Razor files

**Example - WRONG**:
```razor
<!-- [NEW] -->
@using GFC.Core.Models
```

**Example - CORRECT**:
```razor
@using GFC.Core.Models
```

**Why**: Razor files don't support HTML comments at the top level.

---

### **3. Razor Text Content**
✅ **DO**: Wrap plain text in `<text>` elements when mixing with HTML
❌ **DON'T**: Put plain text directly after HTML elements in code blocks

**Example - WRONG**:
```razor
@if (condition)
{
    <i class="bi bi-icon"></i> Some Text
}
```

**Example - CORRECT**:
```razor
@if (condition)
{
    <i class="bi bi-icon"></i>
    <text> Some Text</text>
}
```

**Why**: Razor needs explicit markup boundaries.

---

### **4. Namespaces**
✅ **DO**: Use the EXACT correct namespace
❌ **DON'T**: Guess or use partial namespaces

**Example - WRONG**:
```csharp
using GFC.Core.Models; // HealthStatus is NOT here
```

**Example - CORRECT**:
```csharp
using GFC.Core.Models.Diagnostics; // HealthStatus IS here
```

**Why**: Types must be in the exact namespace where they're defined.

---

### **5. Dependency Injection**
✅ **DO**: Check `Program.cs` to see what's registered
❌ **DON'T**: Inject services that aren't registered

**Example - WRONG**:
```csharp
public MyService(IDbContextFactory<GfcDbContext> factory) // NOT registered!
```

**Example - CORRECT**:
```csharp
public MyService(GfcDbContext dbContext) // This IS registered
```

**Why**: You can only inject services that are registered in the DI container.

---

### **6. Service Lifetimes**
✅ **DO**: Match service lifetimes correctly
❌ **DON'T**: Inject Scoped services into Singleton services

**Rules**:
- **Singleton** → Can only depend on other Singletons
- **Scoped** → Can depend on Singleton or Scoped
- **Transient** → Can depend on anything

**Example - WRONG**:
```csharp
builder.Services.AddSingleton<MyService>(); // MyService depends on DbContext (Scoped)
```

**Example - CORRECT**:
```csharp
builder.Services.AddScoped<MyService>(); // Now it can use DbContext
```

**Why**: Service lifetime hierarchy must be respected.

---

### **7. Entity Framework Usage**
✅ **DO**: Use `GfcDbContext` directly in scoped services
❌ **DON'T**: Use `IDbContextFactory<GfcDbContext>` (not registered)

**Example - WRONG**:
```csharp
private readonly IDbContextFactory<GfcDbContext> _factory;
using var dbContext = await _factory.CreateDbContextAsync();
```

**Example - CORRECT**:
```csharp
private readonly GfcDbContext _dbContext;
await _dbContext.MyTable.ToListAsync();
```

**Why**: The factory pattern is not set up in this project.

---

### **8. Migration Files**
✅ **DO**: Review generated migration files for syntax errors
❌ **DON'T**: Assume auto-generated code is perfect

**Common Issues**:
- Double periods: `modelBuilder..Entity`
- Missing semicolons
- Incorrect type names

**Why**: Code generators can make mistakes.

---

### **9. Model Properties**
✅ **DO**: Include ALL properties that will be used
❌ **DON'T**: Create incomplete models

**Example - WRONG**:
```csharp
public class MyModel
{
    public string Name { get; set; }
    // Missing: Description, CreatedAt, etc.
}
```

**Example - CORRECT**:
```csharp
public class MyModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    // All properties defined upfront
}
```

**Why**: Adding properties later requires migrations and updates.

---

### **10. Using Directives**
✅ **DO**: Add all necessary using statements
❌ **DON'T**: Forget to import namespaces

**Example - WRONG**:
```csharp
// Missing: using GFC.Core.Models.Diagnostics;
public HealthStatus Status { get; set; } // ERROR!
```

**Example - CORRECT**:
```csharp
using GFC.Core.Models.Diagnostics;

public HealthStatus Status { get; set; } // Works!
```

**Why**: Types must be imported before use.

---

## 📋 Pre-Implementation Checklist

Before starting ANY task, verify:

- [ ] I know the EXACT file location (apps/webapp/...)
- [ ] I have checked what's registered in Program.cs
- [ ] I know the correct namespaces for all types
- [ ] I understand the service lifetimes needed
- [ ] I will NOT use HTML comments in Razor files
- [ ] I will wrap text content in `<text>` elements
- [ ] I will use GfcDbContext directly (not factory)
- [ ] I will review all auto-generated code
- [ ] I will include ALL necessary properties in models
- [ ] I will add all required using directives

---

## 🎯 Quality Standards

### **Code Must Be**:
- ✅ In the correct project location
- ✅ Using correct namespaces
- ✅ Following DI lifetime rules
- ✅ Free of HTML comments (in Razor)
- ✅ Syntactically correct
- ✅ Complete (no missing properties)
- ✅ Properly documented

### **Before Submitting**:
1. Double-check file locations
2. Verify all namespaces
3. Test compilation mentally
4. Review for common errors
5. Ensure completeness

---

## 🚀 Success Formula

1. **Read the requirements carefully**
2. **Check existing code patterns**
3. **Verify file locations**
4. **Use correct namespaces**
5. **Follow DI rules**
6. **Review before submitting**
7. **Test thoroughly**

---

## ⚠️ If You're Unsure

**ASK BEFORE IMPLEMENTING!**

Better to ask for clarification than to create errors that need fixing.

---

**Remember**: Quality over speed. Taking 5 extra minutes to verify is better than spending 30 minutes fixing errors!

---

**Last Updated**: December 22, 2025
**Purpose**: Prevent common implementation errors
**Audience**: Future AI agents and developers
