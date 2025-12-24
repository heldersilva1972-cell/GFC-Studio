// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly GfcDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DocumentService(GfcDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<ProtectedDocument> UploadDocumentAsync(IBrowserFile file, string description, string visibility)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            var uploadsFolderPath = Path.Combine(_env.ContentRootPath, "App_Data", "ProtectedDocuments");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = $"{Guid.NewGuid()}_{file.Name}";
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            await using (var stream = file.OpenReadStream(long.MaxValue))
            await using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await stream.CopyToAsync(fs);
            }

            var document = new ProtectedDocument
            {
                FileName = file.Name,
                Description = description,
                ContentType = file.ContentType,
                FilePath = fileName, // Store only the unique filename, not a web path
                Visibility = visibility,
                UploadedAt = DateTime.UtcNow
            };

            _context.ProtectedDocuments.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }

        public async Task<List<ProtectedDocument>> GetAllDocumentsAsync()
        {
            return await _context.ProtectedDocuments.ToListAsync();
        }

        public async Task DeleteDocumentAsync(int id)
        {
            var document = await _context.ProtectedDocuments.FindAsync(id);
            if (document != null)
            {
                var secureFolderPath = Path.Combine(_env.ContentRootPath, "App_Data", "ProtectedDocuments");
                var filePath = Path.Combine(secureFolderPath, document.FilePath);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                _context.ProtectedDocuments.Remove(document);
                await _context.SaveChangesAsync();
            }
        }
    }
}
