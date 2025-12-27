// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GFC.BlazorServer.Services
{
    public class FormService : IFormService
    {
        private readonly GfcDbContext _context;
        private readonly INotificationService _notificationService;
        private readonly NavigationManager _navigationManager;

        public FormService(GfcDbContext context, INotificationService notificationService, NavigationManager navigationManager)
        {
            _context = context;
            _notificationService = notificationService;
            _navigationManager = navigationManager;
        }

        // Form Management
        public async Task<List<Form>> GetAllFormsAsync() => await _context.Forms.Include(f => f.FormFields).ToListAsync();
        public async Task<Form> GetFormByIdAsync(int id) => await _context.Forms.Include(f => f.FormFields).FirstOrDefaultAsync(f => f.Id == id);
        public async Task<Form> CreateFormAsync(Form form)
        {
            _context.Forms.Add(form);
            await _context.SaveChangesAsync();
            return form;
        }
        public async Task<Form> UpdateFormAsync(Form form)
        {
            _context.Forms.Update(form);
            await _context.SaveChangesAsync();
            return form;
        }
        public async Task DeleteFormAsync(int id)
        {
            var form = await GetFormByIdAsync(id);
            if (form != null)
            {
                _context.Forms.Remove(form);
                await _context.SaveChangesAsync();
            }
        }

        // Form Field Management
        public async Task<FormField> AddFieldToFormAsync(int formId, FormField field)
        {
            field.FormId = formId;
            _context.FormFields.Add(field);
            await _context.SaveChangesAsync();
            return field;
        }
        public async Task<FormField> UpdateFormFieldAsync(FormField field)
        {
            _context.FormFields.Update(field);
            await _context.SaveChangesAsync();
            return field;
        }
        public async Task DeleteFormFieldAsync(int fieldId)
        {
            var field = await _context.FormFields.FindAsync(fieldId);
            if (field != null)
            {
                _context.FormFields.Remove(field);
                await _context.SaveChangesAsync();
            }
        }

        // Submissions & "Save for Later"
        public async Task<FormSubmission> CreateSubmissionAsync(FormSubmission submission)
        {
            _context.FormSubmissions.Add(submission);
            await _context.SaveChangesAsync();
            // Here you would add logic to route the submission, e.g., create a HallRentalRequest
            return submission;
        }

        public async Task<HallRentalInquiry> SaveRentalInquiryForLaterAsync(string formData, string userEmail)
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var inquiry = new HallRentalInquiry
            {
                ResumeToken = token,
                FormData = formData,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(14) // Link expires in 2 weeks
            };

            _context.HallRentalInquiries.Add(inquiry);
            await _context.SaveChangesAsync();

            // Send email with the magic link
            var baseUri = _navigationManager.BaseUri;
            var resumeUrl = $"{baseUri}rentals/resume?token={token}";
            var subject = "Your GFC Hall Rental Application - Save & Resume";
            var body = $"<p>You have saved your hall rental application. To resume, please click the link below:</p><a href='{resumeUrl}'>{resumeUrl}</a><p>This link will expire in 14 days.</p>";
            await _notificationService.SendEmailAsync(userEmail, subject, body);

            return inquiry;
        }

        public async Task SaveSubmissionAsync(FormSubmission submission)
        {
            if (submission.Id == 0)
            {
                _context.FormSubmissions.Add(submission);
            }
            else
            {
                _context.FormSubmissions.Update(submission);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<FormSubmission> GetSubmissionByTokenAsync(string token)
        {
            return await _context.FormSubmissions
                .FirstOrDefaultAsync(s => s.ResumeToken == token);
        }

        public async Task<HallRentalInquiry> GetRentalInquiryByTokenAsync(string token)
        {
            return await _context.HallRentalInquiries
                .FirstOrDefaultAsync(i => i.ResumeToken == token && i.ExpiresAt > DateTime.UtcNow);
        }
    }
}
