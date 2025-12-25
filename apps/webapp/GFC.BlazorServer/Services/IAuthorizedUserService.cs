// [NEW]
using GFC.BlazorServer.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GFC.BlazorServer.Services
{
    public interface IAuthorizedUserService
    {
        Task<List<AuthorizedUser>> GetAuthorizedUsersAsync();
        Task AddAuthorizedUserAsync(AuthorizedUser user);
        Task RemoveAuthorizedUserAsync(int userId);
    }
}
