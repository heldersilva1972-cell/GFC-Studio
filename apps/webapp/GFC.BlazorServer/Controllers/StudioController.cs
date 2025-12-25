// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudioController : ControllerBase
    {
        private readonly GfcDbContext _context;

        public StudioController(GfcDbContext context)
        {
            _context = context;
        }

        // PUT: api/Studio/pages/{id}/seo
        [HttpPut("pages/{id}/seo")]
        public async Task<IActionResult> UpdateSeo(int id, [FromBody] StudioPage seoData)
        {
            if (id != seoData.Id)
            {
                return BadRequest();
            }

            var page = await _context.StudioPages.FindAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            page.MetaTitle = seoData.MetaTitle;
            page.MetaDescription = seoData.MetaDescription;
            page.OgImage = seoData.OgImage;

            _context.Entry(page).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.StudioPages.AnyAsync(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
    }
}
