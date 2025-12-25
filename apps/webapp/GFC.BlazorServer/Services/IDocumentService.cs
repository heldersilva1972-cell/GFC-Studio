// [NEW]
using GFC.Core.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IDocumentService
    {
        Task<ProtectedDocument> UploadDocumentAsync(IBrowserFile file, string description, string visibility);
        Task<List<ProtectedDocument>> GetAllDocumentsAsync();
        Task DeleteDocumentAsync(int id);
    }
}
