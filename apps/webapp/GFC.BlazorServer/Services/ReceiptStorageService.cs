using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components.Forms;

namespace GFC.BlazorServer.Services;

public class ReceiptStorageService
{
    private readonly string _receiptRoot;
    private readonly ILogger<ReceiptStorageService> _logger;
    private readonly IWebHostEnvironment _environment;

    public ReceiptStorageService(IConfiguration configuration, ILogger<ReceiptStorageService> logger, IWebHostEnvironment environment)
    {
        _receiptRoot = configuration["Reimbursements:ReceiptRoot"] ?? "data/receipts";
        _logger = logger;
        _environment = environment;
    }

    public async Task<(string fileName, string relativePath)> SaveFileAsync(IBrowserFile file, int requestId)
    {
        if (file == null || file.Size == 0)
        {
            throw new ArgumentException("File is empty or null.", nameof(file));
        }

        // Sanitize file name
        var sanitizedFileName = SanitizeFileName(file.Name);
        var extension = Path.GetExtension(sanitizedFileName);
        var baseFileName = Path.GetFileNameWithoutExtension(sanitizedFileName);
        var uniqueFileName = $"{Guid.NewGuid()}_{baseFileName}{extension}";

        // Create path: ReceiptRoot/yyyy/MM/reqId/
        var now = DateTime.UtcNow;
        var yearMonthPath = Path.Combine(now.Year.ToString(), now.Month.ToString("D2"));
        var requestPath = Path.Combine(yearMonthPath, requestId.ToString());
        var fullDirectoryPath = Path.Combine(_environment.ContentRootPath, _receiptRoot, requestPath);

        // Ensure directory exists
        Directory.CreateDirectory(fullDirectoryPath);

        var fullFilePath = Path.Combine(fullDirectoryPath, uniqueFileName);
        var relativePath = Path.Combine(_receiptRoot, requestPath, uniqueFileName).Replace('\\', '/');

        // Save file
        using (var stream = new FileStream(fullFilePath, FileMode.Create))
        {
            await file.OpenReadStream().CopyToAsync(stream);
        }

        _logger.LogInformation("Saved receipt file: {RelativePath} for request {RequestId}", relativePath, requestId);

        return (uniqueFileName, relativePath);
    }

    public async Task<(string fileName, string relativePath)> SaveFileFromBytesAsync(byte[] fileBytes, string originalFileName, int requestId)
    {
        if (fileBytes == null || fileBytes.Length == 0)
        {
            throw new ArgumentException("File bytes are empty or null.", nameof(fileBytes));
        }

        // Sanitize file name
        var sanitizedFileName = SanitizeFileName(originalFileName);
        var extension = Path.GetExtension(sanitizedFileName);
        var baseFileName = Path.GetFileNameWithoutExtension(sanitizedFileName);
        var uniqueFileName = $"{Guid.NewGuid()}_{baseFileName}{extension}";

        // Create path: ReceiptRoot/yyyy/MM/reqId/
        var now = DateTime.UtcNow;
        var yearMonthPath = Path.Combine(now.Year.ToString(), now.Month.ToString("D2"));
        var requestPath = Path.Combine(yearMonthPath, requestId.ToString());
        var fullDirectoryPath = Path.Combine(_environment.ContentRootPath, _receiptRoot, requestPath);

        // Ensure directory exists
        Directory.CreateDirectory(fullDirectoryPath);

        var fullFilePath = Path.Combine(fullDirectoryPath, uniqueFileName);
        var relativePath = Path.Combine(_receiptRoot, requestPath, uniqueFileName).Replace('\\', '/');

        // Save file
        await File.WriteAllBytesAsync(fullFilePath, fileBytes);

        _logger.LogInformation("Saved receipt file from bytes: {RelativePath} for request {RequestId}", relativePath, requestId);

        return (uniqueFileName, relativePath);
    }

    public string GetFilePath(string relativePath)
    {
        return Path.Combine(_environment.ContentRootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
    }

    public bool FileExists(string relativePath)
    {
        var fullPath = GetFilePath(relativePath);
        return File.Exists(fullPath);
    }

    private static string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return "file";
        }

        // Remove path separators and dangerous characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = string.Join("_", fileName.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

        // Remove any remaining dangerous patterns
        sanitized = Regex.Replace(sanitized, @"[^\w\.-]", "_");

        // Limit length
        if (sanitized.Length > 200)
        {
            var ext = Path.GetExtension(sanitized);
            sanitized = sanitized.Substring(0, 200 - ext.Length) + ext;
        }

        return sanitized;
    }
}
