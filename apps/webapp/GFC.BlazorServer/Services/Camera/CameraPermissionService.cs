// [NEW]
using GFC.BlazorServer.Data;
using GFC.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services.Camera
{
    public class CameraPermissionService : ICameraPermissionService
    {
        private readonly GfcDbContext _context;

        public CameraPermissionService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<CameraPermission>> GetPermissionsForCameraAsync(int cameraId)
        {
            return await _context.CameraPermissions
                .Where(p => p.CameraId == cameraId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<List<CameraPermission>> GetPermissionsForUserAsync(int userId)
        {
            return await _context.CameraPermissions
                .Where(p => p.UserId == userId)
                .Include(p => p.Camera)
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

        public async Task<bool> HasPermissionAsync(int userId, int cameraId, CameraAccessLevel accessLevel)
        {
            var permission = await _context.CameraPermissions
                .FirstOrDefaultAsync(p => p.UserId == userId && p.CameraId == cameraId);

            if (permission == null)
                return false;

            // Check if the user's access level is sufficient
            return permission.AccessLevel >= accessLevel;
        }

        public async Task<bool> UserHasAnyCameraPermissionAsync(int userId)
        {
            return await _context.CameraPermissions
                .AnyAsync(p => p.UserId == userId);
        }
    }
}
