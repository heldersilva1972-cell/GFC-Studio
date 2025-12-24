// [NEW]
using GFC.BlazorServer.Data;
using GFC.BlazorServer.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public class AuthorizedUserService : IAuthorizedUserService
    {
        private readonly GfcDbContext _context;

        public AuthorizedUserService(GfcDbContext context)
        {
            _context = context;
        }

        public async Task<List<AuthorizedUser>> GetAuthorizedUsersAsync()
        {
            return await _context.AuthorizedUsers.Include(u => u.User).ToListAsync();
        }

        public async Task AddAuthorizedUserAsync(AuthorizedUser user)
        {
            _context.AuthorizedUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAuthorizedUserAsync(int authorizedUserId)
        {
            var user = await _context.AuthorizedUsers.FindAsync(authorizedUserId);
            if (user != null)
            {
                _context.AuthorizedUsers.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
