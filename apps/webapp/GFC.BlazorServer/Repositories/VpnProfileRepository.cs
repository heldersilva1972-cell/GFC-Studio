// [NEW]
using GFC.Core.Interfaces;
using GFC.Core.Models;
using GFC.BlazorServer.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Repositories
{
    public class VpnProfileRepository : IVpnProfileRepository
    {
        private readonly GfcDbContext _context;

        public VpnProfileRepository(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<VpnProfile> GetByIdAsync(int id)
        {
            return await _context.VpnProfiles.FindAsync(id);
        }

        public async Task<VpnProfile> GetByUserIdAsync(int userId)
        {
            return await _context.VpnProfiles
                .Where(p => p.UserId == userId && p.RevokedAt == null)
                .FirstOrDefaultAsync();
        }

        public async Task<List<VpnProfile>> GetAllAsync()
        {
            return await _context.VpnProfiles.ToListAsync();
        }

        public async Task AddAsync(VpnProfile profile)
        {
            _context.VpnProfiles.Add(profile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(VpnProfile profile)
        {
            _context.Entry(profile).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<string> GetNextAvailableIpAddressAsync()
        {
            // In the future, this will be loaded from settings
            var vpnSubnet = "10.8.0.";
            int startIp = 2; // 10.8.0.1 is usually the server

            var usedIps = await _context.VpnProfiles
                .Select(p => p.AssignedIP)
                .ToListAsync();

            for (int i = startIp; i < 255; i++)
            {
                var ip = vpnSubnet + i;
                if (!usedIps.Contains(ip))
                {
                    return ip;
                }
            }

            // If we run out of IPs, we have a problem.
            // For now, we'll throw an exception.
            throw new System.Exception("No available VPN IP addresses.");
        }
    }
}
