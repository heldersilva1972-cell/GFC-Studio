// [NEW]
using GFC.Core.Models;
using GFC.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraPermissionService : ICameraPermissionService
    {
        private readonly ApplicationDbContext _context;

        public CameraPermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CameraPermission>> GetPermissionsForCameraAsync(int cameraId)
        {
            return await _context.CameraPermissions
                .Where(p => p.CameraId == cameraId)
                .ToListAsync();
        }

        public async Task<List<CameraPermission>> GetPermissionsForUserAsync(string userId)
        {
            return await _context.CameraPermissions
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task AddPermissionAsync(CameraPermission permission)
        {
            _context.CameraPermissions.Add(permission);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePermissionAsync(int permissionId)
        {
            var permission = await _context.CameraPermissions.FindAsync(permissionId);
            if (permission != null)
            {
                _context.CameraPermissions.Remove(permission);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasPermissionAsync(string userId, int cameraId, CameraAccessLevel accessLevel)
        {
            return await _context.CameraPermissions
                .AnyAsync(p => p.UserId == userId && p.CameraId == cameraId && p.AccessLevel >= accessLevel);
        }
    }
}
