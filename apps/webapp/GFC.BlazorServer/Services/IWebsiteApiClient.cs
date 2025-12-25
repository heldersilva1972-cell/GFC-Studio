// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IWebsiteApiClient
    {
        Task TriggerRevalidation(string tag);
    }
}
