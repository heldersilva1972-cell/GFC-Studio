// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IMediaStorageService
    {
        Task<string> SaveImageFromUrlAsync(string imageUrl);
    }
}
