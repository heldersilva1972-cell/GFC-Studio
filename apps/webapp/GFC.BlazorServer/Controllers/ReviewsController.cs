// [NEW]
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly GfcDbContext _context;

        public ReviewsController(GfcDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PublicReview review)
        {
            if (review == null)
            {
                return BadRequest();
            }

            review.IsApproved = false; // All reviews are unapproved by default
            _context.PublicReviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reviews = await _context.PublicReviews.Where(r => r.IsApproved).ToListAsync();
            return Ok(reviews);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _context.PublicReviews.ToListAsync();
            return Ok(reviews);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PublicReview review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _context.PublicReviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.PublicReviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
