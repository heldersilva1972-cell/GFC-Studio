// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IFormService
    {
        Task<Form> CreateFormAsync(Form form);
        Task<Form> GetFormByIdAsync(int formId);
        Task UpdateFormAsync(Form form);
        Task DeleteFormAsync(int formId);
        Task SaveSubmissionAsync(FormSubmission submission);
        Task<List<FormSubmission>> GetSubmissionsByFormIdAsync(int formId);
        Task<List<Form>> GetAllFormsAsync();
        Task UpdateSubmissionStatusAsync(int submissionId, string status);
        Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission);
        Task<HallRentalInquiry> SaveRentalInquiryForLaterAsync(string formData, string email);
    }
}
