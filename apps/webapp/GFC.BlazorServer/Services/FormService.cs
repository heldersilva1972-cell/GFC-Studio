// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class FormService : IFormService
    {
        private readonly GfcDbContext _context;

        public FormService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<Form> CreateFormAsync(Form form)
        {
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }

        public async Task<Form> GetFormByIdAsync(int formId)
        {
            return await _context.Forms
                .Include(f => f.FormFields)
                .FirstOrDefaultAsync(f => f.Id == formId);
        }

        public async Task UpdateFormAsync(Form form)
        {
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFormAsync(int formId)
        {
            var form = await _context.Forms.FindAsync(formId);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveSubmissionAsync(FormSubmission submission)
        {
            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FormSubmission>> GetSubmissionsByFormIdAsync(int formId)
        {
            return await _context.FormSubmissions
                .Where(s => s.FormId == formId)
                .ToListAsync();
        }

        public async Task<List<Form>> GetAllFormsAsync()
        {
            return await _context.Forms.ToListAsync();
        }

        public async Task UpdateSubmissionStatusAsync(int submissionId, string status)
        {
            var submission = await _context.FormSubmissions.FindAsync(submissionId);
            if (submission != null)
            {
                submission.Status = status;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission)
        {
            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            return submission;
        }

        public async Task<HallRentalInquiry> SaveRentalInquiryForLaterAsync(string formData, string email)
        {
            var inquiry = new HallRentalInquiry
            {
                ResumeToken = System.Guid.NewGuid().ToString("N"),
                FormData = formData,
                CreatedAt = System.DateTime.UtcNow,
                ExpiresAt = System.DateTime.UtcNow.AddDays(30)
            };
            _context.HallRentalInquiries.Add(inquiry);
            await _context.SaveChangesAsync();
            
            // Note: In a real implementation, you would send the 'resumeToken' to the user's email here.
            
            return inquiry;
        }
    }
}
