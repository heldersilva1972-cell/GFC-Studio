// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<ActionResult<List<PublicReview>>> GetReviews()
        {
            return await _reviewService.GetAllReviewsAsync();
        }

        [HttpGet("featured")]
        public async Task<ActionResult<List<PublicReview>>> GetFeaturedReviews()
        {
            return await _reviewService.GetApprovedAndFeaturedReviewsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PublicReview>> GetReview(int id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        public async Task<ActionResult<PublicReview>> PostReview(PublicReview review)
        {
            await _reviewService.CreateReviewAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, PublicReview review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            await _reviewService.UpdateReviewAsync(review);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            await _reviewService.DeleteReviewAsync(id);
            return NoContent();
        }
    }
}
