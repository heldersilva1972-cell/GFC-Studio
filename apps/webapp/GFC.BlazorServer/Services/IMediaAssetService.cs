// [NEW]
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IMediaAssetService
    {
        Task<MediaAsset> CreateAssetAsync(IBrowserFile file, string usage);
        Task<List<MediaAsset>> GetAllAssetsAsync();
        Task<MediaAsset> GetAssetByIdAsync(int id);
        Task DeleteAssetAsync(int id);
    }
}
