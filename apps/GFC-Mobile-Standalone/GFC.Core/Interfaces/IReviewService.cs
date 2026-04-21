// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IReviewService
    {
        Task<List<PublicReview>> GetAllReviewsAsync();
        Task<PublicReview> GetReviewByIdAsync(int id);
        Task CreateReviewAsync(PublicReview review);
        Task UpdateReviewAsync(PublicReview review);
        Task DeleteReviewAsync(int id);
        Task<List<PublicReview>> GetApprovedAndFeaturedReviewsAsync();
    }
}
