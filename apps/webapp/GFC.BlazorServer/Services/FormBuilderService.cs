// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Interfaces;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class FormBuilderService : IFormBuilderService
    {
        private readonly IDbContextFactory<GfcDbContext> _dbContextFactory;

        public FormBuilderService(IDbContextFactory<GfcDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<DynamicForm> GetFormByNameAsync(string name)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.DynamicForms.FirstOrDefaultAsync(f => f.Name == name);
        }

        public async Task SaveFormAsync(DynamicForm form)
        {
            using var context = await _dbContextFactory.CreateDbContextAsync();
            var existingForm = await context.DynamicForms.FirstOrDefaultAsync(f => f.Name == form.Name);

            if (existingForm != null)
            {
                existingForm.SchemaJson = form.SchemaJson;
                context.DynamicForms.Update(existingForm);
            }
            else
            {
                context.DynamicForms.Add(form);
            }

            await context.SaveChangesAsync();
        }
    }
}
