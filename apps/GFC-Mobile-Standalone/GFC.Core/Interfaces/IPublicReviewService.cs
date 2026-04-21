// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IPublicReviewService
    {
        Task<List<PublicReview>> GetAllReviewsAsync();
        Task<List<PublicReview>> GetApprovedAndFeaturedReviewsAsync();
        Task CreateReviewAsync(PublicReview review);
        Task UpdateReviewAsync(PublicReview review);
        Task DeleteReviewAsync(int id);
    }
}
