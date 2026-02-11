using System.ComponentModel.DataAnnotations;
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GFC.BlazorServer.Services;

public class ReimbursementService
{
    private readonly IDbContextFactory<GfcDbContext> _contextFactory;
    private readonly ILogger<ReimbursementService> _logger;
    private readonly ReceiptStorageService _receiptStorage;
    private readonly IMemberRepository _memberRepository;
    private readonly INotificationService _notificationService;

    public ReimbursementService(
        IDbContextFactory<GfcDbContext> contextFactory,
        ILogger<ReimbursementService> logger,
        ReceiptStorageService receiptStorage,
        IMemberRepository memberRepository,
        INotificationService notificationService)
    {
        _contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _receiptStorage = receiptStorage ?? throw new ArgumentNullException(nameof(receiptStorage));
        _memberRepository = memberRepository ?? throw new ArgumentNullException(nameof(memberRepository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    public async Task<ReimbursementRequest> CreateDraftAsync(int memberId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        var request = new ReimbursementRequest
        {
            RequestorMemberId = memberId,
            RequestDate = DateTime.Today,
            Status = "Draft",
            TotalAmount = 0,
            CreatedUtc = DateTime.UtcNow,
            UpdatedUtc = DateTime.UtcNow,
            EditedFlag = false
        };

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.ReimbursementRequests.Add(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created draft reimbursement request {RequestId} for member {MemberId}", request.Id, memberId);

        return request;
    }

    public async Task<ReimbursementItem> AddItemAsync(int requestId, ReimbursementItemDto itemDto, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        if (!itemDto.Amount.HasValue || itemDto.Amount.Value <= 0)
        {
            throw new ArgumentException("Amount must be greater than 0.", nameof(itemDto));
        }

        if (itemDto.ExpenseDate == default)
        {
            throw new ArgumentException("ExpenseDate is required.", nameof(itemDto));
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Draft")
        {
            throw new InvalidOperationException("Cannot add items to a request that is not in Draft status.");
        }

        var category = await dbContext.ReimbursementCategories
            .FirstOrDefaultAsync(c => c.Id == itemDto.CategoryId && c.IsActive, cancellationToken);

        if (category == null)
        {
            throw new InvalidOperationException($"Category {itemDto.CategoryId} not found or is not active.");
        }

        var item = new ReimbursementItem
        {
            RequestId = requestId,
            ExpenseDate = itemDto.ExpenseDate,
            Amount = itemDto.Amount.Value,
            CategoryId = itemDto.CategoryId,
            Vendor = itemDto.Vendor,
            Notes = itemDto.Notes
        };

        dbContext.ReimbursementItems.Add(item);
        
        // Update total amount - sum all items including the newly added one
        request.TotalAmount = request.Items.Sum(i => i.Amount);
        request.UpdatedUtc = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Added item {ItemId} to request {RequestId}", item.Id, requestId);

        return item;
    }

    public async Task<ReceiptFile> AddReceiptAsync(int itemId, string fileName, string relativePath, int uploaderId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var item = await dbContext.ReimbursementItems
            .FirstOrDefaultAsync(i => i.Id == itemId, cancellationToken);

        if (item == null)
        {
            throw new InvalidOperationException($"Reimbursement item {itemId} not found.");
        }

        var receiptFile = new ReceiptFile
        {
            RequestItemId = itemId,
            FileName = fileName,
            RelativePath = relativePath,
            UploadedUtc = DateTime.UtcNow,
            UploadedByMemberId = uploaderId
        };

        dbContext.ReceiptFiles.Add(receiptFile);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Added receipt file {ReceiptId} to item {ItemId}", receiptFile.Id, itemId);

        return receiptFile;
    }

    public async Task UpdateDraftAsync(int requestId, ReimbursementRequestDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Draft")
        {
            throw new InvalidOperationException("Cannot update a request that is not in Draft status.");
        }

        request.Notes = dto.Notes;
        request.UpdatedUtc = DateTime.UtcNow;
        request.EditedFlag = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated draft request {RequestId}", requestId);
    }

    public async Task SubmitAsync(int requestId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .ThenInclude(i => i.ReceiptFiles)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Draft")
        {
            throw new InvalidOperationException("Only Draft requests can be submitted.");
        }

        if (request.Items.Count == 0)
        {
            throw new InvalidOperationException("Cannot submit a request with no items.");
        }

        // Validate all items have valid amounts
        if (request.Items.Any(i => i.Amount <= 0))
        {
            throw new InvalidOperationException("All items must have an amount greater than 0.");
        }

        // Check receipt requirement
        var settings = await GetSettingsAsync(cancellationToken);
        if (settings.ReceiptRequired)
        {
            var itemsWithoutReceipts = request.Items.Where(i => !i.ReceiptFiles.Any()).ToList();
            if (itemsWithoutReceipts.Any())
            {
                throw new InvalidOperationException($"Receipts are required. The following items are missing receipts: {string.Join(", ", itemsWithoutReceipts.Select(i => $"Item #{i.Id}"))}");
            }
        }

        var oldStatus = request.Status;
        request.Status = "Submitted";
        request.UpdatedUtc = DateTime.UtcNow;

        await LogChangeAsync(requestId, request.RequestorMemberId, "Status", oldStatus, "Submitted", cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        // Reload request with items for notification
        request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        // Send notification
        if (request != null)
        {
            await ReimbursementNotificationService.SendOnSubmittedAsync(request, _memberRepository, dbContext, _notificationService, _logger, cancellationToken);
        }

        _logger.LogInformation("Submitted reimbursement request {RequestId}", requestId);
    }
    public async Task UpdateRequestAsync(int requestId, int memberId, ReimbursementRequestDto dto, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        var request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId && r.RequestorMemberId == memberId, cancellationToken);
            
        if (request == null) throw new InvalidOperationException("Request not found or access denied.");
        if (request.Status != "Submitted") throw new InvalidOperationException("Only submitted requests can be edited.");

        request.Notes = dto.Notes;
        request.UpdatedUtc = DateTime.UtcNow;
        request.EditedFlag = true;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateItemAsync(int itemId, int memberId, ReimbursementItemDto dto, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var item = await dbContext.ReimbursementItems
            .Include(i => i.Request)
                .ThenInclude(r => r.Items)
            .FirstOrDefaultAsync(i => i.Id == itemId, cancellationToken);
            
        if (item == null || item.Request.RequestorMemberId != memberId) throw new InvalidOperationException("Item not found.");
        if (item.Request.Status != "Submitted") throw new InvalidOperationException("Request cannot be edited.");

        item.Amount = dto.Amount ?? 0;
        item.CategoryId = dto.CategoryId;
        item.Notes = dto.Notes;
        item.ExpenseDate = dto.ExpenseDate;
        
        // Recalculate request total
        item.Request.TotalAmount = item.Request.Items.Sum(i => i.Amount);
        item.Request.UpdatedUtc = DateTime.UtcNow;
        item.Request.EditedFlag = true;
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ApproveAsync(int requestId, int approverId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Submitted")
        {
            throw new InvalidOperationException("Only Submitted requests can be approved.");
        }

        var oldStatus = request.Status;
        request.Status = "Approved";
        request.ApprovedByMemberId = approverId;
        request.ApprovedDateUtc = DateTime.UtcNow;
        request.UpdatedUtc = DateTime.UtcNow;

        await LogChangeAsync(requestId, approverId, "Status", oldStatus, "Approved", cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        // Reload request with items for notification
        request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        // Send notification
        if (request != null)
        {
            await ReimbursementNotificationService.SendOnApprovedAsync(request, _memberRepository, _notificationService, _logger, cancellationToken);
        }

        _logger.LogInformation("Approved reimbursement request {RequestId} by member {ApproverId}", requestId, approverId);
    }

    public async Task RejectAsync(int requestId, int approverId, string reason, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Rejection reason is required.", nameof(reason));
        }

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Submitted")
        {
            throw new InvalidOperationException("Only Submitted requests can be rejected.");
        }

        var oldStatus = request.Status;
        request.Status = "Rejected";
        request.RejectedByMemberId = approverId;
        request.RejectedDateUtc = DateTime.UtcNow;
        request.RejectReason = reason.Trim();
        request.UpdatedUtc = DateTime.UtcNow;

        await LogChangeAsync(requestId, approverId, "Status", oldStatus, "Rejected", cancellationToken);
        await LogChangeAsync(requestId, approverId, "RejectReason", null, reason.Trim(), cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        // Reload request with items for notification
        request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        // Send notification
        if (request != null)
        {
            await ReimbursementNotificationService.SendOnRejectedAsync(request, _memberRepository, _notificationService, _logger, cancellationToken);
        }

        _logger.LogInformation("Rejected reimbursement request {RequestId} by member {ApproverId}", requestId, approverId);
    }

    public async Task MarkPaidAsync(int requestId, int approverId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var request = await dbContext.ReimbursementRequests
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null)
        {
            throw new InvalidOperationException($"Reimbursement request {requestId} not found.");
        }

        if (request.Status != "Approved" && request.Status != "Submitted")
        {
            throw new InvalidOperationException("Only Approved or Submitted requests can be marked as paid.");
        }

        var oldStatus = request.Status;
        request.Status = "Paid";
        request.PaidByMemberId = approverId;
        request.PaidDateUtc = DateTime.UtcNow;
        request.UpdatedUtc = DateTime.UtcNow;

        await LogChangeAsync(requestId, approverId, "Status", oldStatus, "Paid", cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        // Reload request with items for notification
        request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        // Send notification
        if (request != null)
        {
            await ReimbursementNotificationService.SendOnPaidAsync(request, _memberRepository, _notificationService, _logger, cancellationToken);
        }

        _logger.LogInformation("Marked reimbursement request {RequestId} as paid by member {ApproverId}", requestId, approverId);
    }

    public async Task LogChangeAsync(int requestId, int memberId, string fieldName, string? oldValue, string? newValue, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var log = new ReimbursementChangeLog
        {
            RequestId = requestId,
            ChangedByMemberId = memberId,
            ChangeUtc = DateTime.UtcNow,
            FieldName = fieldName,
            OldValue = oldValue,
            NewValue = newValue
        };

        dbContext.ReimbursementChangeLogs.Add(log);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ReimbursementChangeLog>> GetHistoryAsync(int requestId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.ReimbursementChangeLogs
            .Where(c => c.RequestId == requestId)
            .OrderByDescending(c => c.ChangeUtc)
            .ToListAsync(cancellationToken);
    }

    private async Task EnsureReimbursementSchemaAsync(CancellationToken cancellationToken)
    {
        var sql = @"
IF OBJECT_ID(N'[dbo].[ReimbursementRequests]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementRequests](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestorMemberId] INT NOT NULL,
        [RequestDate] DATE NOT NULL,
        [Status] NVARCHAR(50) NOT NULL,
        [TotalAmount] DECIMAL(10,2) NOT NULL,
        [Notes] NVARCHAR(MAX) NULL,
        [CreatedUtc] DATETIME2 NOT NULL,
        [UpdatedUtc] DATETIME2 NOT NULL,
        [EditedFlag] BIT NOT NULL DEFAULT(0),
        [ApprovedByMemberId] INT NULL,
        [ApprovedDateUtc] DATETIME2 NULL,
        [RejectedByMemberId] INT NULL,
        [RejectedDateUtc] DATETIME2 NULL,
        [RejectReason] NVARCHAR(MAX) NULL,
        [PaidByMemberId] INT NULL,
        [PaidDateUtc] DATETIME2 NULL
    );
END

IF OBJECT_ID(N'[dbo].[ReimbursementItems]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementItems](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestId] INT NOT NULL,
        [ExpenseDate] DATE NOT NULL,
        [Amount] DECIMAL(10,2) NOT NULL,
        [CategoryId] INT NOT NULL,
        [Vendor] NVARCHAR(200) NULL,
        [Notes] NVARCHAR(MAX) NULL
    );
END

IF OBJECT_ID(N'[dbo].[ReimbursementCategories]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementCategories](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Name] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL
    );
END

-- Seed categories and update existing ones to remove descriptions
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Supplies' WHERE [Name] = N'Supplies (General house items)';
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Bar / Kitchen' WHERE [Name] = N'Bar / Kitchen (Food, garnishes, napkins)';
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Repairs & Maintenance' WHERE [Name] = N'Repairs & Maintenance (Building or equipment fixes)';
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Events' WHERE [Name] = N'Events (Specific costs for a club party or event)';
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Office' WHERE [Name] = N'Office (Postage, paper, ink)';
UPDATE [dbo].[ReimbursementCategories] SET [Name] = N'Other' WHERE [Name] = N'Other (Miscellaneous)';

IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Supplies')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Supplies', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Bar / Kitchen')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Bar / Kitchen', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Repairs & Maintenance')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Repairs & Maintenance', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Events')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Events', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Office')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Office', 1);
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories] WHERE [Name] = N'Other')
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Other', 1);

IF OBJECT_ID(N'[dbo].[ReceiptFiles]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReceiptFiles](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestItemId] INT NOT NULL,
        [FileName] NVARCHAR(200) NOT NULL,
        [RelativePath] NVARCHAR(500) NOT NULL,
        [UploadedUtc] DATETIME2 NOT NULL,
        [UploadedByMemberId] INT NOT NULL
    );
END

IF OBJECT_ID(N'[dbo].[ReimbursementChangeLogs]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementChangeLogs](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [RequestId] INT NOT NULL,
        [ChangedByMemberId] INT NOT NULL,
        [ChangeUtc] DATETIME2 NOT NULL,
        [FieldName] NVARCHAR(100) NOT NULL,
        [OldValue] NVARCHAR(MAX) NULL,
        [NewValue] NVARCHAR(MAX) NULL
    );
END

IF OBJECT_ID(N'[dbo].[ReimbursementSettings]', N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ReimbursementSettings](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [ReceiptRequired] BIT NOT NULL,
        [NotificationRecipients] NVARCHAR(MAX) NULL
    );

    INSERT INTO [dbo].[ReimbursementSettings] (ReceiptRequired, NotificationRecipients)
    VALUES (0, NULL);
END

-- Fallback: Ensure categories are NEVER empty
IF NOT EXISTS (SELECT 1 FROM [dbo].[ReimbursementCategories])
BEGIN
    INSERT INTO [dbo].[ReimbursementCategories] (Name, IsActive) VALUES (N'Supplies', 1), (N'Bar / Kitchen', 1), (N'Repairs & Maintenance', 1), (N'Events', 1), (N'Office', 1), (N'Other', 1);
END
";
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
    }

    public async Task<ReimbursementSettings> GetSettingsAsync(CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var settings = await dbContext.ReimbursementSettings
            .FirstOrDefaultAsync(cancellationToken);

        if (settings == null)
        {
            settings = new ReimbursementSettings
            {
                ReceiptRequired = false,
                NotificationRecipients = null
            };

            dbContext.ReimbursementSettings.Add(settings);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return settings;
    }

    public async Task UpdateSettingsAsync(ReimbursementSettings settings, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var existing = await dbContext.ReimbursementSettings
            .FirstOrDefaultAsync(cancellationToken);

        if (existing == null)
        {
            dbContext.ReimbursementSettings.Add(settings);
        }
        else
        {
            existing.ReceiptRequired = settings.ReceiptRequired;
            existing.NotificationRecipients = settings.NotificationRecipients;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ReimbursementRequest>> GetRequestsAsync(
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        int? memberId = null,
        CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var query = dbContext.ReimbursementRequests
            .Include(r => r.Items)
                .ThenInclude(i => i.Category)
            .Include(r => r.Items)
                .ThenInclude(i => i.ReceiptFiles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(r => r.Status == status);
        }

        if (startDate.HasValue)
        {
            query = query.Where(r => r.RequestDate >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(r => r.RequestDate <= endDate.Value);
        }

        if (memberId.HasValue)
        {
            query = query.Where(r => r.RequestorMemberId == memberId.Value);
        }

        return await query
            .OrderByDescending(r => r.RequestDate)
            .ThenByDescending(r => r.CreatedUtc)
            .ToListAsync(cancellationToken);
    }

    private async Task<string> LoadTemplateAsync(string fileName)
    {
        try
        {
            var templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NotificationTemplates", fileName);
            if (File.Exists(templatePath))
            {
                return await File.ReadAllTextAsync(templatePath);
            }
        }
        catch
        {
        }
        return string.Empty;
    }

    private async Task SendEmailAsync(string to, string subject, string body, ILogger logger)
    {
        logger.LogInformation("Email would be sent to {To} with subject: {Subject}", to, subject);
        await Task.CompletedTask;
    }

    public async Task<ReimbursementRequest?> GetRequestAsync(int requestId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.ReimbursementRequests
            .Include(r => r.Items)
                .ThenInclude(i => i.Category)
            .Include(r => r.Items)
                .ThenInclude(i => i.ReceiptFiles)
            .Include(r => r.ChangeLogs)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);
    }

    public async Task<List<ReimbursementCategory>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.ReimbursementCategories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task DeleteItemAsync(int itemId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);

        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        var item = await dbContext.ReimbursementItems
            .Include(i => i.Request)
            .Include(i => i.ReceiptFiles)
            .FirstOrDefaultAsync(i => i.Id == itemId, cancellationToken);

        if (item == null)
        {
            throw new InvalidOperationException($"Reimbursement item {itemId} not found.");
        }

        if (item.Request.Status != "Draft")
        {
            throw new InvalidOperationException("Cannot delete items from a request that is not in Draft status.");
        }

        // Delete receipt files from disk
        foreach (var receipt in item.ReceiptFiles)
        {
            try
            {
                if (_receiptStorage.FileExists(receipt.RelativePath))
                {
                    var filePath = _receiptStorage.GetFilePath(receipt.RelativePath);
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to delete receipt file {RelativePath}", receipt.RelativePath);
            }
        }

        // Update total amount
        item.Request.TotalAmount = item.Request.Items.Where(i => i.Id != itemId).Sum(i => i.Amount);
        item.Request.UpdatedUtc = DateTime.UtcNow;

        dbContext.ReimbursementItems.Remove(item);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted item {ItemId} from request {RequestId}", itemId, item.RequestId);
    }

    public async Task DeleteRequestAsync(int requestId, int memberId, CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var request = await dbContext.ReimbursementRequests
            .Include(r => r.Items)
                .ThenInclude(i => i.ReceiptFiles)
            .FirstOrDefaultAsync(r => r.Id == requestId, cancellationToken);

        if (request == null) throw new InvalidOperationException("Request not found.");
        
        // Safety check: Only owner or admin (though we only have memberId here)
        if (request.RequestorMemberId != memberId) throw new InvalidOperationException("Access denied.");
        
        if (request.Status == "Paid") throw new InvalidOperationException("Cannot delete a paid request.");

        // Delete all receipt files for all items
        foreach (var item in request.Items)
        {
            foreach (var receipt in item.ReceiptFiles)
            {
                try
                {
                    if (_receiptStorage.FileExists(receipt.RelativePath))
                    {
                        var filePath = _receiptStorage.GetFilePath(receipt.RelativePath);
                        File.Delete(filePath);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete receipt file {RelativePath} during request deletion", receipt.RelativePath);
                }
            }
        }

        dbContext.ReimbursementRequests.Remove(request);
        await dbContext.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted reimbursement request {RequestId} by member {MemberId}", requestId, memberId);
    }

    public async Task<List<ReimbursementCategory>> ForceSeedCategoriesAsync(CancellationToken cancellationToken = default)
    {
        await EnsureReimbursementSchemaAsync(cancellationToken);
        await using var dbContext = await _contextFactory.CreateDbContextAsync(cancellationToken);
        
        var count = await dbContext.ReimbursementCategories.CountAsync(cancellationToken);
        if (count == 0 || !await dbContext.ReimbursementCategories.AnyAsync(c => c.Name == "Supplies", cancellationToken))
        {
            var cats = new[] { "Supplies", "Bar / Kitchen", "Repairs & Maintenance", "Events", "Office", "Other" };
            foreach (var c in cats)
            {
                if (!await dbContext.ReimbursementCategories.AnyAsync(x => x.Name == c, cancellationToken))
                {
                    dbContext.ReimbursementCategories.Add(new ReimbursementCategory { Name = c, IsActive = true });
                }
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        
        return await dbContext.ReimbursementCategories.Where(c => c.IsActive).OrderBy(x => x.Name).ToListAsync(cancellationToken);
    }
}

public class ReimbursementItemDto
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Expense date is required.")]
    public DateTime ExpenseDate { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Amount must be greater than 0.")]
    public decimal? Amount { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    [MaxLength(200)]
    public string? Vendor { get; set; }

    public string? Notes { get; set; }
}

public class ReimbursementRequestDto
{
    public string? Notes { get; set; }
}
