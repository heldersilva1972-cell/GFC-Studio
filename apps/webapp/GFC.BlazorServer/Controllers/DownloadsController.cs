// [NEW]
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadsController : ControllerBase
    {
        private readonly GfcDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DownloadsController(GfcDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpGet("{documentId}")]
        public async Task<IActionResult> GetDocument(int documentId)
        {
            var document = await _context.ProtectedDocuments.FindAsync(documentId);

            if (document == null)
            {
                return NotFound();
            }

            if (document.Visibility == "MembersOnly")
            {
                // In a real app with authentication, you would check roles/claims here.
                // For this context, we check if the user is authenticated.
                if (!User.Identity.IsAuthenticated)
                {
                    return Forbid(); // User is not logged in, forbid access.
                }
            }

            var secureFolderPath = Path.Combine(_env.ContentRootPath, "App_Data", "ProtectedDocuments");
            var filePath = Path.Combine(secureFolderPath, document.FilePath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("File not found on server.");
            }

            var memory = new MemoryStream();
            await using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, document.ContentType, document.FileName);
        }
    }
}
