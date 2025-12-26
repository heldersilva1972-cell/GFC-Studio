// [NEW]
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.Core.Interfaces
{
    public interface IMediaAssetService
    {
        Task<MediaAsset> CreateMediaAssetAsync(IBrowserFile file, string tag, string uploadedBy);
        Task<IEnumerable<MediaAsset>> GetMediaAssetsAsync();
        Task<IEnumerable<MediaAsset>> GetPublicWebsiteGalleryAsync();
        Task DeleteMediaAssetAsync(int id);
    }
}
