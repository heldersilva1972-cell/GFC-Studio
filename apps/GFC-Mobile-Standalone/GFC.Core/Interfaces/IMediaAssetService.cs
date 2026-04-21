using GFC.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IMediaAssetService
    {
        Task<MediaAsset> CreateMediaAssetAsync(Stream fileStream, string fileName, string tag, string uploadedBy);
        Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync();
        Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync();
        Task DeleteMediaAssetAsync(int id);
        Task UpdateAssetRoleAsync(int id, string? role);
    }
}
