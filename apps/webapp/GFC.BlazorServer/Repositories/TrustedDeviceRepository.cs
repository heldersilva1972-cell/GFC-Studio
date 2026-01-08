// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models.Security;
using GFC.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
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
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.TrustedDevices.FirstOrDefaultAsync(d => d.DeviceToken == token);
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return null;
            }
        }

        public async Task CreateAsync(TrustedDevice device)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                context.TrustedDevices.Add(device);
                await context.SaveChangesAsync();
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                // Ignore if table missing
            }
        }

        public async Task UpdateAsync(TrustedDevice device)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                context.TrustedDevices.Update(device);
                await context.SaveChangesAsync();
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                // Ignore if table missing
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                var device = await context.TrustedDevices.FindAsync(id);
                if (device != null)
                {
                    context.TrustedDevices.Remove(device);
                    await context.SaveChangesAsync();
                }
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                // Ignore if table missing
            }
        }

        public async Task<List<TrustedDevice>> GetActiveDevicesForUserAsync(int userId)
        {
            try
            {
                await using var context = await _contextFactory.CreateDbContextAsync();
                return await context.TrustedDevices
                    .Where(d => d.UserId == userId && !d.IsRevoked && d.ExpiresAtUtc > DateTime.UtcNow)
                    .ToListAsync();
            }
            catch (SqlException ex) when (ex.Number == 208)
            {
                return new List<TrustedDevice>();
            }
        }
    }
}
