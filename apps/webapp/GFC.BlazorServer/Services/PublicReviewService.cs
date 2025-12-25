// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class PublicReviewService : IPublicReviewService
    {
        private readonly GfcDbContext _context;

        public PublicReviewService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<PublicReview>> GetAllReviewsAsync()
        {
            return await _context.PublicReviews.ToListAsync();
        }

        public async Task<List<PublicReview>> GetApprovedAndFeaturedReviewsAsync()
        {
            return await _context.PublicReviews
                .Where(r => r.IsApproved && r.IsFeatured)
                .ToListAsync();
        }

        public async Task CreateReviewAsync(PublicReview review)
        {
            _context.PublicReviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateReviewAsync(PublicReview review)
        {
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(int id)
        {
            var review = await _context.PublicReviews.FindAsync(id);
            if (review != null)
            {
                _context.PublicReviews.Remove(review);
                await _context.SaveChangesAsync();
            }
        }
    }
}
