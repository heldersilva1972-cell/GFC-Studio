// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models.Security;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Repositories
{
    public class TrustedDeviceRepository : ITrustedDeviceRepository
    {
        private readonly IDbContextFactory<GfcDbContext> _contextFactory;

        public TrustedDeviceRepository(IDbContextFactory<GfcDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<TrustedDevice?> GetByTokenAsync(string token)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            return await context.TrustedDevices.FirstOrDefaultAsync(d => d.DeviceToken == token);
        }

        public async Task CreateAsync(TrustedDevice device)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.TrustedDevices.Add(device);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TrustedDevice device)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.TrustedDevices.Update(device);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            var device = await context.TrustedDevices.FindAsync(id);
            if (device != null)
            {
                context.TrustedDevices.Remove(device);
                await context.SaveChangesAsync();
            }
        }
    }
}
