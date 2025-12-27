// [NEW]
using GFC.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IFormService
    {
        // Form Management
        Task<List<Form>> GetAllFormsAsync();
        Task<Form> GetFormByIdAsync(int id);
        Task<Form> CreateFormAsync(Form form);
        Task<Form> UpdateFormAsync(Form form);
        Task DeleteFormAsync(int id);

        // Form Field Management
        Task<FormField> AddFieldToFormAsync(int formId, FormField field);
        Task<FormField> UpdateFormFieldAsync(FormField field);
        Task DeleteFormFieldAsync(int fieldId);

        // Submissions & "Save for Later"
        Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission);
        Task SaveSubmissionAsync(FormSubmission submission);
        Task<FormSubmission> GetSubmissionByTokenAsync(string token);
        Task<HallRentalInquiry> SaveRentalInquiryForLaterAsync(string formData, string userEmail);
        Task<HallRentalInquiry> GetRentalInquiryByTokenAsync(string token);
    }
}
