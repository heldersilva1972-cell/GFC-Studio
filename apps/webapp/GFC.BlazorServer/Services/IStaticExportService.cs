// [NEW]
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IStaticExportService
    {
        Task<byte[]> ExportSiteAsZipAsync();
    }
}
